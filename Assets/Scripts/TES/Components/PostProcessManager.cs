﻿using Demonixis.Toolbox.XR;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR;

namespace TESUnity.Components
{
    [RequireComponent(typeof(PostProcessLayer))]
    public sealed class PostProcessManager : MonoBehaviour
    {
        private IEnumerator Start()
        {
#if UNITY_ANDROID
            var layer = GetComponent<PostProcessLayer>();
            var volume = FindObjectOfType<PostProcessVolume>();
            layer.enabled = false;
            volume.enabled = false;
            yield break;
#else
            yield return new WaitForEndOfFrame();

#if UNITY_EDITOR
            if (TESManager.instance._bypassINIConfig)
                yield break;
#endif
            UpdateEffects();
#endif
        }

        public void UpdateEffects()
        {
            var settings = TESManager.instance;
            var layer = GetComponent<PostProcessLayer>();
            var volume = FindObjectOfType<PostProcessVolume>();
            var profile = volume.profile;
            var xrEnabled = XRManager.Enabled;

            if (settings.postProcessingQuality == TESManager.PostProcessingQuality.Low)
            {
                volume.enabled = false;
                layer.antialiasingMode = PostProcessLayer.Antialiasing.None;
                return;
            }

            if (settings.postProcessingQuality == TESManager.PostProcessingQuality.Medium)
            {
                UpdateEffect<Bloom>(profile, (bloom) =>
                {
                    bloom.fastMode.value = true;
                });

                DisableEffect<AmbientOcclusion>(profile);
                DisableEffect<AutoExposure>(profile);
                DisableEffect<MotionBlur>(profile);
                DisableEffect<ScreenSpaceReflections>(profile);
                SetPostProcessEffectEnabled<Vignette>(profile, false);

                layer.fastApproximateAntialiasing.fastMode = true;
                layer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
            }
            else if (settings.postProcessingQuality == TESManager.PostProcessingQuality.High)
            {
                UpdateEffect<Bloom>(profile, (bloom) =>
                {
                    bloom.fastMode.value = true;
                });

                UpdateEffect<ScreenSpaceReflections>(profile, (ssr) =>
                {
                    ssr.preset.value = ScreenSpaceReflectionPreset.Low;
                });

                DisableEffect<ScreenSpaceReflections>(profile);
            }

            // SMAA is not supported in VR.
            if (xrEnabled && settings.antiAliasing == PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing)
                settings.antiAliasing = PostProcessLayer.Antialiasing.TemporalAntialiasing;

            layer.antialiasingMode = (PostProcessLayer.Antialiasing)settings.antiAliasing;

            if (xrEnabled)
            {
                layer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                layer.fastApproximateAntialiasing.fastMode = true;

                UpdateEffect<Bloom>(profile, (bloom) =>
                {
                    bloom.dirtTexture.value = null;
                    bloom.dirtIntensity.value = 0;
                    bloom.fastMode.value = true;
                });

                UpdateEffect<ScreenSpaceReflections>(profile, (ssr) =>
                {
                    if (settings.postProcessingQuality == TESManager.PostProcessingQuality.High)
                        ssr.preset.value = ScreenSpaceReflectionPreset.Medium;
                });

                SetPostProcessEffectEnabled<Vignette>(profile, false);
                DisableEffect<MotionBlur>(profile);
            }
        }

        private void SetEffectEnabled<T>(bool isEnabled) where T : MonoBehaviour
        {
            var component = GetComponent<T>();
            if (component != null)
                component.enabled = isEnabled;
            else
                Debug.LogWarningFormat("The component {0} doesn't exists", typeof(T));
        }

        private static void UpdateEffect<T>(PostProcessProfile profile, Action<T> callback) where T : PostProcessEffectSettings
        {
            T outParam;
            if (profile.TryGetSettings<T>(out outParam))
                callback(outParam);
        }

        public static void SetPostProcessEffectEnabled<T>(PostProcessProfile profile, bool isEnabled) where T : PostProcessEffectSettings
        {
            UpdateEffect<T>(profile, (e) => e.enabled.value = isEnabled);
        }

        private static void DisableEffect<T>(PostProcessProfile profile) where T : PostProcessEffectSettings
        {
            if (!profile.HasSettings<T>())
                return;

            profile.RemoveSettings<T>();
        }
    }
}
