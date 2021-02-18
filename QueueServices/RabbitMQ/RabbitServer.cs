using Microsoft.Extensions.Configuration;
using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MLConsumer.QueueServices.RabbitMQ
{
   public class RabbitServer
    {
      
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IDatabaseService<ErrorLog> _errorLogService;
        readonly IConfiguration _iConfig;
        public RabbitServer (IConfiguration iConfig,IDatabaseService<ErrorLog> errorLogService)
            {
            _iConfig = iConfig;
            _errorLogService = errorLogService;

            }
            public IConnection Connect()
            {

                var mres = new ManualResetEventSlim(false); // state is initially false
                int delay = 0;
                while (!mres.Wait(delay)) // loop until state is true, checking every 3s
                {
                    try
                    {
                        Reconnect();
                        mres.Set();
                    
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Rabbit Server not working");
                        _errorLogService.Create(new ErrorLog { Level = "Error", Date = DateTime.Now.ToString(), ErrorMessage = ex.Message });
                        delay = 3000;
                    }
                }
            return _connection;
            }
            private void Reconnect()
            {
                try
                {
                    _factory = new ConnectionFactory() { HostName = _iConfig.GetValue<string>("RabbitMQSettings:ConnectionString") };
                    _factory.RequestedHeartbeat = TimeSpan.FromSeconds(60);
                    _connection = _factory.CreateConnection();
                }
                catch (Exception)
                {

                    throw;
                }


            }

        
    }
}
