using Microsoft.Win32;
using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MLConsumer.Configuration
{
  public class ConsumerConfiguration
    {
        private IDatabaseService<ErrorLog> _errorLogService;
        public ConsumerConfiguration(IDatabaseService<ErrorLog> errorLogService)
        {
            _errorLogService = errorLogService;
        }

        public void RegistryConfiguration(int serviceSize = 3)
        {
            try
            {
                Dictionary<string, object> rgChecks = new Dictionary<string, object>();
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rgChecks.Add("MLConsumer1", rkApp.GetValue("MLConsumer1"));
                rgChecks.Add("MLConsumer2", rkApp.GetValue("MLConsumer2"));
                rgChecks.Add("MLConsumer3", rkApp.GetValue("MLConsumer3"));
                foreach (var rgCheck in rgChecks)
                {
                    if (rgCheck.Value == null)
                    {
                        var exe = System.AppDomain.CurrentDomain.BaseDirectory + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
                        rkApp.SetValue(rgCheck.Key, exe);
                    }
                }
               
            }
            catch (Exception e)
            {

                _errorLogService.Create(new ErrorLog { Level = "Error", Date = DateTime.Now.ToString(), ErrorMessage = e.Message + " Registy Configuration Process" });
            }
           
        }
    }
}
