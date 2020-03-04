﻿using TESUnity.ESM.Records;

namespace TESUnity.ESM
{
    public static class RecordUtils
    {
        public static string GetModelFileName(Record record)
        {
            var modelRecord = record as IModelRecord;
            if (modelRecord != null)
            {
                return modelRecord.Model;
            }

            // Will be soon deprecated.
            if (record is CLOTRecord)
            {
                return ((CLOTRecord)record).MODL.value;
            }

            return null;
        }
    }
}
