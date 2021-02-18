using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MLConsumer.DeviceAndParserServices.ParserMethods
{
    class CodeParser
    {
        readonly IDatabaseService<ErrorLog> _errorLogService;
        public CodeParser(IDatabaseService<ErrorLog> errorLogService)
        {
            _errorLogService = errorLogService;
        }
        public const string bosKey = "-bos-";
        public bool Work(string log, ref Dictionary<string, string> values)
        {
            return Parse(log, ref values);
        }
        private bool Parse(string line, ref Dictionary<string, string> values)
        {
            try
            {
                line = line.Insert(line.Length, " a");
                line = line.Replace(" src zone=", " src_zone=");
                line = line.Replace(" dst zone=", " dst_zone=");
                line = line.Remove(0, 5);
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                int keyPreEndPos = 0;
                int keyStartPos = 0;

                List<string> keys = new List<string>();
                List<string> val = new List<string>();

                keys.Add(bosKey);
                if (line.Length > 0)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ')
                            keyStartPos = i + 1;
                        else if (line[i] == '=' || i == line.Length - 1)
                        {
                            //string value = line.Substring(keyPreEndPos, keyStartPos - keyPreEndPos - 1);
                            int say = keyStartPos - keyPreEndPos - 1;
                            if (say < 0) say = 0;
                            string value = line.Substring(keyPreEndPos, say);
                            val.Add(value.Trim(' ').Trim(',').Trim('"'));
                            keys.Add(line.Substring(keyStartPos, i - keyStartPos).Trim());
                            keyStartPos = i + 1;
                            keyPreEndPos = i + 1;
                        }
                    }
                }
                val.RemoveAt(0);
                keys.RemoveAt(0);
                keys.RemoveAt(keys.Count - 1);

                for (int i = 0; i < keys.Count; i++)
                {
                    values.Add(keys[i], val[i]);
                }

                return true;
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                var frame = st.GetFrame(st.FrameCount - 1);
                var errorline = frame.GetFileLineNumber();
                _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "Error", ErrorMessage = e.Message, ErrorLine = errorline.ToString() });
                return false;
            }

        }
    }
}
