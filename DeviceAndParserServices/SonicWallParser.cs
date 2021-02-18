using MLConsumer.DatabaseObjects.Devices;
using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseObjects.RegisteredDevices;
using MLConsumer.DatabaseServices.MongoDB;
using MLConsumer.DatabaseServices.MongoDB.InterFaces;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MLConsumer.DeviceAndParserServices.InterFaces;
using MLConsumer.DeviceAndParserServices.ParserMethods;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

namespace MLConsumer.DeviceAndParserServices
{
    public class SonicWallParser : IParser
    {
        readonly IDatabaseService<ErrorLog> _errorLogService;
        readonly ILogService<SonicWall> _logService;
        SonicWall _sonicWall;
        Dictionary<string, string> values = new Dictionary<string, string>();
        readonly RegexParser _regexParser;
        readonly CodeParser _codeParser;
        public SonicWallParser(IDatabaseService<ErrorLog> errorLogService, IConfiguration iConfig)
        {
            _codeParser = new CodeParser(errorLogService);
            _regexParser = new RegexParser(errorLogService);
            _errorLogService = errorLogService;
            
            _logService = new LogService<SonicWall>(new LogDatabaseSettings { ConnectionString = iConfig.GetValue<string>("DatabaseSettings:ConnectionString"), DatabaseName = iConfig.GetValue<string>("DatabaseSettings:DatabaseName") });
        }
        public void Work(string log, RegisteredDevice device)
        {           
            if (device.DeviceParseMethod == "Regex")
            {
                if (_regexParser.Work(log, device.RegexStatements, ref values))
                {
                    CheckToProcesses(device.Id, true);
                }
                else
                {
                    _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "Level=Error", ErrorMessage = "Message=Log is not parsed" });
                }
            }
            else if (device.DeviceParseMethod == "Code")
            {
                if (_codeParser.Work(log, ref values))
                {
                    CheckToProcesses(device.Id, true);
                }
                else
                {
                    _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "Level=Error", ErrorMessage = "Message=Log is not parsed" });
                }
            }
        }
        protected void CheckToProcesses(string ownedDeviceId, bool result)
        {
            try
            {
                if (result)
                {
                    AddToDatabase(values, ownedDeviceId);
                }
                else
                {
                    _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "Hata", ErrorMessage = ownedDeviceId + " cihazına ait bir log parse edilemedi" });
                }
            }
            catch (Exception e)
            {
                _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "Hata", ErrorMessage = "Veritabanına yazma işlemi sırasında " + e.Message + " hatası ile karşılaşıldı ve işlem gerçekleştirilemedi." });
            }
            finally
            {
                values.Clear();
            }
        }
        protected bool AddToDatabase(Dictionary<string, string> values, string ownedDeviceId)
        {
            try
            {
                _sonicWall = new SonicWall();

                if (values.ContainsKey("time"))
                {
                    var date = values.Where(a => a.Key == "time").Select(a => a.Value).First();
                    _sonicWall.time = DateTime.Parse(date, null);
                }
                _sonicWall.logid = values.Where(a => a.Key == "id").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.sn = values.Where(a => a.Key == "sn").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.fw = values.Where(a => a.Key == "fw").Select(a => a.Value).DefaultIfEmpty(null).First();

                _sonicWall.pri = values.Where(a => a.Key == "pri").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.c = values.Where(a => a.Key == "pri").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.m = values.Where(a => a.Key == "m").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.msg = values.Where(a => a.Key == "msg").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.app = values.Where(a => a.Key == "app").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.n = values.Where(a => a.Key == "n").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.src = values.Where(a => a.Key == "src").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.dst = values.Where(a => a.Key == "dst").Select(a => a.Value).DefaultIfEmpty(null).First();



                _sonicWall.dstMac = values.Where(a => a.Key == "dstMac").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.proto = values.Where(a => a.Key == "proto").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.sent = values.Where(a => a.Key == "sent").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.dpi = values.Where(a => a.Key == "dpi").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.rule = values.Where(a => a.Key == "rule").Select(a => a.Value).DefaultIfEmpty(null).First();
                _sonicWall.fw_action = values.Where(a => a.Key == "fw_action").Select(a => a.Value).DefaultIfEmpty(null).First();

                _logService.CreateConnection(ownedDeviceId);
                _logService.Create(_sonicWall);
                return true;
            }
            catch (Exception e)
            {

                _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "hate", ErrorMessage = e.Message });
                return false;
            }

        }
  
    }
}
