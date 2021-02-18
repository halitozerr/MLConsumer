using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MLConsumer.Configuration;
using MLConsumer.DatabaseObjects;
using MLConsumer.DatabaseObjects.Eps;
using MLConsumer.DatabaseObjects.UnRegisteredDevices;
using MLConsumer.DeviceAndParserServices.InterFaces;
using MLConsumer.QueueServices.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace MLConsumer
{
    public class ConsumerService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly IConfiguration _iConfig;
        private Dictionary<IParser, string> _instances;
        private LogFormat _logFormat;
        private RabbitServer _rabbitServer;
        private ConsumerConfiguration _consumerConfigurations;
        private IDatabaseObjects _databaseObjects;
        public ConsumerService(ILogger<ConsumerService> logger, ConsumerConfiguration consumerConfigurations, LogFormat logFormat, RabbitServer rabbitServer, IConfiguration iConfig, IDatabaseObjects databaseObjects)
        {

            _consumerConfigurations = consumerConfigurations;
            _logger = logger;
            _iConfig = iConfig;
            _logFormat = logFormat;
            _rabbitServer = rabbitServer;
            _instances = new Dictionary<IParser, string>();
            _databaseObjects = databaseObjects;
            var usedDevices = _databaseObjects._usedDevice.Get();
            foreach (var item in usedDevices)
            {
                var targetType = Type.GetType("MLConsumer.DeviceAndParserServices." + item.DeviceClassName);
                _instances.Add(Activator.CreateInstance((targetType), _databaseObjects._errorLogService, _iConfig) as IParser, item.DeviceBrand);
            }

        }
        public void StartConsumer()
        {
            try
            {
                _consumerConfigurations.RegistryConfiguration(3);
                using (var connection = _rabbitServer.Connect())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "log", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var a = _databaseObjects._eps.Get();
                        if (a.Count == 0)
                        {
                            _databaseObjects._eps.Create(new EpsData { Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EpsCalculate = 100 });
                        }
                        else
                        {
                            if (a[0].Date == DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            {
                                var eps = a[0].EpsCalculate + 100;
                                _databaseObjects._eps.Update(a[0].Id, new EpsData { Id = a[0].Id, EpsCalculate = eps, Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                            }
                            else
                            {
                                _databaseObjects._eps.Update(a[0].Id, new EpsData { Id = a[0].Id, EpsCalculate = 100, Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                            }
                        }
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        _logFormat = JsonSerializer.Deserialize<LogFormat>(message);
                        Console.WriteLine(" [x] Received {0}", message);

                        var unregsteredDevice = _databaseObjects._unRegisteredDeviceService.Get().Find(device => device.IPAdress == _logFormat.IPAdress);
                        var registeredDevice = _databaseObjects._registeredDeviceService.Get().Find(device => device.IPAdress == _logFormat.IPAdress);

                        if (registeredDevice != null)
                        {
                            var process = _instances.Where(x => x.Value == registeredDevice.DeviceBrand).Select(x => x.Key).First();
                            process.Work(_logFormat.Log.Replace("\"", ""), registeredDevice);
                            _logFormat.IPAdress = null;
                            unregsteredDevice = null;
                            registeredDevice = null;
                            _logFormat.Log = null;
                        }
                        else if (unregsteredDevice == null)
                        {
                            _databaseObjects._unRegisteredDeviceService.Create(new UnRegisteredDevice { DeviceName = "unKnown", IPAdress = _logFormat.IPAdress, DeviceDescription = "Discovered Device", isActive = true });
                            _logFormat.IPAdress = null;
                            unregsteredDevice = null;
                            registeredDevice = null;
                            _logFormat.Log = null;
                        }


                    };
                    channel.BasicConsume(queue: "log",
                                         autoAck: true,
                                         consumer: consumer);
                    while (true)
                    {
                     
                        Thread.Sleep(10000);
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Bağlantı Kurulamadı");
            }

        }





    }
}
