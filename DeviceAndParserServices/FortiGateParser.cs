using Microsoft.Extensions.Configuration;
using MLConsumer.DatabaseObjects.Devices;
using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseObjects.RegisteredDevices;
using MLConsumer.DatabaseServices.MongoDB;
using MLConsumer.DatabaseServices.MongoDB.InterFaces;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MLConsumer.DeviceAndParserServices.InterFaces;
using MLConsumer.DeviceAndParserServices.ParserMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace MLConsumer.DeviceAndParserServices
{
    public class FortiGateParser : IParser
    {
        readonly IDatabaseService<ErrorLog> _errorLogService;
        readonly ILogService<FortiGate> _logService;
        FortiGate _fortigate;
        Dictionary<string, string> values = new Dictionary<string, string>();
        readonly RegexParser _regexParser;
        readonly CodeParser _codeParser;
        string ownedDevice;
        public FortiGateParser(IDatabaseService<ErrorLog> errorLogService, IConfiguration iConfig)
        {
            _codeParser = new CodeParser(errorLogService);
            _regexParser = new RegexParser(errorLogService);
            _errorLogService = errorLogService;
            _logService = new LogService<FortiGate>(new LogDatabaseSettings { ConnectionString = iConfig.GetValue<string>("DatabaseSettings:ConnectionString"), DatabaseName = iConfig.GetValue<string>("DatabaseSettings:DatabaseName") });
        }
        public void Work(string log, RegisteredDevice device)
        {
            if (device.DeviceParseMethod == "Regex")
            {
                if (_regexParser.Work(log, device.RegexStatements, ref values))
                {
                    AddToDatabase(device.Id, true);
                }
                else
                {
                    _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "Level=Error", ErrorMessage = "Message=Log is not parsed" }); ;
                }

            }
            else if (device.DeviceParseMethod == "Code")
            {
                if (_codeParser.Work(log, ref values))
                {
                    AddToDatabase(device.Id, true);
                }
            }
        }
        protected void AddToDatabase(string ownedDeviceId, bool result)
        {
            try
            {
                if (result)
                {
                    CreateLogWithSelectedRegex(values, ownedDeviceId);
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
        protected bool CreateLogWithSelectedRegex(Dictionary<string, string> values, string ownedDeviceId)
        {
            ownedDevice = ownedDeviceId;
            try
            {
                _fortigate = new FortiGate();

                if ((values.ContainsKey("date")) && (values.ContainsKey("time")))
                {
                    var date = values.Where(a => a.Key == "date").Select(a => a.Value).First() + " " + values.Where(a => a.Key == "time").Select(a => a.Value).First();
                    _fortigate.date = date;
                }
                _fortigate.devname = values.Where(a => a.Key == "devname").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.devid = values.Where(a => a.Key == "devid").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.eventtime = values.Where(a => a.Key == "eventtime").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.tz = values.Where(a => a.Key == "tz").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.logid = values.Where(a => a.Key == "logid").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.type = values.Where(a => a.Key == "type").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.subtype = values.Where(a => a.Key == "subtype").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.eventtype = values.Where(a => a.Key == "eventtype").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.level = values.Where(a => a.Key == "level").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.vd = values.Where(a => a.Key == "vd").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.appid = values.Where(a => a.Key == "appid").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.countapp = values.Where(a => a.Key == "countapp").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.srcip = values.Where(a => a.Key == "srcip").Select(a => a.Value).DefaultIfEmpty(null).First();              
                _fortigate.srcport = values.Where(a => a.Key == "srcport").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.srcintf = values.Where(a => a.Key == "srcintf").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.srcname = values.Where(a => a.Key == "srcname").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.srcintfrole = values.Where(a => a.Key == "srcintfrole").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.devtype = values.Where(a => a.Key == "devtype").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.osname = values.Where(a => a.Key == "osname").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.mastersrcmac = values.Where(a => a.Key == "mastersrcmac").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.srcmac = values.Where(a => a.Key == "srcmac").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.transport = values.Where(a => a.Key == "transport").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.transport = values.Where(a => a.Key == "transport").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.transip = values.Where(a => a.Key == "transip").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.dstip = values.Where(a => a.Key == "dstip").Select(a => a.Value).DefaultIfEmpty(null).First();            
                _fortigate.dstport = values.Where(a => a.Key == "dstport").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.dstintf = values.Where(a => a.Key == "dstintf").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.dstname = values.Where(a => a.Key == "dstname").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.dstintfrole = values.Where(a => a.Key == "dstintfrole").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.srccountry = values.Where(a => a.Key == "srccountry").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.dstcountry = values.Where(a => a.Key == "dstcountry").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.sessionid = values.Where(a => a.Key == "sessionid").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.proto = values.Where(a => a.Key == "proto").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.action = values.Where(a => a.Key == "action").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.policyid = values.Where(a => a.Key == "policyid").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.applist = values.Where(a => a.Key == "applist").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.policytype = values.Where(a => a.Key == "policytype").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.policymode = values.Where(a => a.Key == "policymode").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.poluuid = values.Where(a => a.Key == "poluuid").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.policyname = values.Where(a => a.Key == "policyname").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.service = values.Where(a => a.Key == "service").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.direction = values.Where(a => a.Key == "direction").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.trandisp = values.Where(a => a.Key == "trandisp").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.duration = values.Where(a => a.Key == "duration").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.sentbyte = values.Where(a => a.Key == "sentbyte").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.rcvdbyte = values.Where(a => a.Key == "rcvdbyte").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.sentpkt = values.Where(a => a.Key == "sentpkt").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.rcvdpkt = values.Where(a => a.Key == "rcvdpkt").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.vpn = values.Where(a => a.Key == "vpn").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.vpntype = values.Where(a => a.Key == "vpntype").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.appcat = values.Where(a => a.Key == "appcat").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.app = values.Where(a => a.Key == "app").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.incidentserialno = values.Where(a => a.Key == "incidentserialno").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.utmaction = values.Where(a => a.Key == "utmaction").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.utmref = values.Where(a => a.Key == "utmref").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.msg = values.Where(a => a.Key == "msg").Select(a => a.Value).DefaultIfEmpty(null).First();
                _fortigate.apprisk = values.Where(a => a.Key == "apprisk").Select(a => a.Value).DefaultIfEmpty(null).First();




                _logService.CreateConnection(ownedDeviceId);
                _logService.Create(_fortigate);
                return true;
            }
            catch (Exception e)
            {
              
                var st2 = new StackTrace(e, true);
                var frame = st2.GetFrame(st2.FrameCount - 1);
                var line = frame.GetFileLineNumber();
                _errorLogService.Create(new ErrorLog { Date = DateTime.Now.ToString(), Level = "hata", ErrorMessage = e.Message +" Forti " +  ownedDevice});
                return false;
            }
        }

    }
}
