﻿/// UnityVRDevice
/// Last Modified Date: 01/07/2017
#if UNITY_ANDROID
#define OCULUS_SDK
#endif
using UnityEngine;
using UnityEngine.XR;

namespace Demonixis.Toolbox.XR
{
    /// <summary>
    /// The UnityVRDevice is an abstract device that uses the UnityEngine.VR implementation.
    /// </summary>
    public class UnityXRDevice : XRDeviceBase
    {
        #region Public Fields

        public override float RenderScale
        {
            get { return XRSettings.eyeTextureResolutionScale; }
            set { XRSettings.eyeTextureResolutionScale = value; }
        }

        public override int EyeTextureWidth { get { return XRSettings.eyeTextureWidth; } }

        public override int EyeTextureHeight { get { return XRSettings.eyeTextureHeight; } }

        public override XRDeviceType VRDeviceType { get { return XRDeviceType.UnityXR; } }

        public override Vector3 HeadPosition { get { return InputTracking.GetLocalPosition(XRNode.Head); } }

        public override bool IsAvailable { get { return XRDevice.isPresent && XRSettings.enabled; } }

        #endregion

        public override void Recenter()
        {
            InputTracking.Recenter();
        }

        public override void SetActive(bool active)
        {
            if (XRSettings.enabled != active)
                XRSettings.enabled = active;

#if OCULUS_SDK
            var manager = gameObject.AddComponent<OVRManager>();
            OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;
#endif
        }

        public override void SetTrackingSpaceType(TrackingSpaceType type, Transform headTransform, float height)
        {
#if UNITY_ANDROID
            if (XRManager.Instance.ForceSeatedOnMobile)
                type = TrackingSpaceType.Stationary;
#endif
            XRDevice.SetTrackingSpaceType(type);

            var position = headTransform.localPosition;
            headTransform.localPosition = new Vector3(position.x, type == TrackingSpaceType.RoomScale ? 0 : height, position.z);

            InputTracking.Recenter();
        }
    }
}