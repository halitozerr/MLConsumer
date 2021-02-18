using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MLConsumer.DeviceAndParserServices.ParserMethods
{
    class RegexParser
    {
        Dictionary<string, int> _selectedRegex = new Dictionary<string, int>();
        readonly IDatabaseService<ErrorLog> _errorLogService;
        public RegexParser(IDatabaseService<ErrorLog> errorLogService)
        {
            _errorLogService = errorLogService;
        }
        public bool Work(string log, List<string> regexStatements, ref Dictionary<string, string> values)
        {
            var result = RegexSelector(log, regexStatements, ref _selectedRegex);
            if (result)
            {
                var regex = _selectedRegex.FirstOrDefault(x => x.Value == _selectedRegex.Max(a => a.Value)).Key;
                return Parse(log, ref values, regex);
            }
            else
            {
                return false;
            }
        }
        public bool RegexSelector(string log, List<string> regexStatements, ref Dictionary<string, int> selectedRegex)
        {

            try
            {
                foreach (var regexStatement in regexStatements)
                {
                    Regex regex = new Regex(regexStatement, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    MatchCollection matches = regex.Matches(log);
                    if (matches.Count > 0)
                    {
                        selectedRegex.Add(regexStatement, matches[0].Groups.Count);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                var frame = st.GetFrame(st.FrameCount - 1);
                var line = frame.GetFileLineNumber();
                _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "Error", ErrorMessage = e.Message, ErrorLine = line.ToString() });
                return false;
            }


        }
        protected static bool Parse(string log, ref Dictionary<string, string> values, string regex)
        {
            Regex rg = new Regex(regex);
            var matches = rg.Match(log);
            if (matches.Success)
            {
                for (int i = 1; i < matches.Groups.Count; i++)
                {
                    values.Add(matches.Groups[i].Name, matches.Groups[i].Value);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
