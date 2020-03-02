﻿namespace TESUnity.ESM
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
            if (record is STATRecord)
            {
                return ((STATRecord)record).Model;
            }
            else if (record is DOORRecord)
            {
                return ((DOORRecord)record).Model;
            }
            else if (record is MISCRecord)
            {
                return ((MISCRecord)record).Model;
            }
            else if (record is WEAPRecord)
            {
                return ((WEAPRecord)record).Model;
            }
            else if (record is CONTRecord)
            {
                return ((CONTRecord)record).MODL.value;
            }
            else if (record is LIGHRecord)
            {
                return ((LIGHRecord)record).Model;
            }
            else if (record is ARMORecord)
            {
                return ((ARMORecord)record).MODL.value;
            }
            else if (record is CLOTRecord)
            {
                return ((CLOTRecord)record).MODL.value;
            }
            else if (record is REPARecord)
            {
                return ((REPARecord)record).Model;
            }
            else if (record is ACTIRecord)
            {
                return ((ACTIRecord)record).Model;
            }
            else if (record is APPARecord)
            {
                return ((APPARecord)record).Model;
            }
            else if (record is LOCKRecord)
            {
                return ((LOCKRecord)record).Model;
            }
            else if (record is PROBRecord)
            {
                return ((PROBRecord)record).Model;
            }
            else if (record is INGRRecord)
            {
                return ((INGRRecord)record).Model;
            }
            else if (record is BOOKRecord)
            {
                return ((BOOKRecord)record).Model;
            }
            else if (record is ALCHRecord)
            {
                return ((ALCHRecord)record).Model;
            }
            else if (record is CREARecord)
            {
                return ((CREARecord)record).Model;
            }

            return null;
        }
    }
}
