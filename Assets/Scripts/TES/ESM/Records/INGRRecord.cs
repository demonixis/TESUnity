﻿namespace TESUnity.ESM
{
    public struct IngrediantData
    {
        public float Weight;
        public int Value;
        public int[] EffectID;
        public int[] SkillID;
        public int[] AttributeID;
    }

    public class INGRRecord : Record, IIdRecord, IModelRecord
    {
        public string Id { get; private set; }
        public string Model { get; private set; }
        public string Name { get; private set; }
        public IngrediantData Data { get; private set; }
        public string Icon { get; private set; }
        public string Enchantment { get; private set; }
        public string Script { get; private set; }

        public override void DeserializeSubRecord(UnityBinaryReader reader, string subRecordName, uint dataSize)
        {
            if (subRecordName == "NAME")
            {
                Id = reader.ReadPossiblyNullTerminatedASCIIString((int)dataSize);
            }
            else if (subRecordName == "MODL")
            {
                Model = reader.ReadPossiblyNullTerminatedASCIIString((int)dataSize);
            }
            else if (subRecordName == "FNAM")
            {
                Name = reader.ReadPossiblyNullTerminatedASCIIString((int)dataSize);
            }
            else if (subRecordName == "IRDT")
            {
                Data = new IngrediantData
                {
                    Weight = reader.ReadLESingle(),
                    Value = reader.ReadLEInt32(),
                    EffectID = ReadInt32Array(reader, 4),
                    SkillID = ReadInt32Array(reader, 4),
                    AttributeID = ReadInt32Array(reader, 4)
                };
            }
            else if (subRecordName == "ITEX")
            {
                Icon = reader.ReadPossiblyNullTerminatedASCIIString((int)dataSize);
            }
            else if (subRecordName == "ENAM")
            {
                Enchantment = reader.ReadPossiblyNullTerminatedASCIIString((int)dataSize);
            }
            else if (subRecordName == "SCRI")
            {
                Script = reader.ReadPossiblyNullTerminatedASCIIString((int)dataSize);
            }
            else
            {
                ReadMissingSubRecord(reader, subRecordName, dataSize);
            }
        }

        #region Deprecated
        public override bool NewFetchMethod => true;
        public override SubRecord CreateUninitializedSubRecord(string subRecordName, uint dataSize) => null;
        #endregion
    }
}
