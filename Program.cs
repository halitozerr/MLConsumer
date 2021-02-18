using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MLConsumer.Configuration;
using MLConsumer.DatabaseObjects;
using MLConsumer.DatabaseObjects.Devices;
using MLConsumer.DatabaseObjects.Eps;
using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseObjects.RegisteredDevices;
using MLConsumer.DatabaseObjects.UnRegisteredDevices;
using MLConsumer.DatabaseObjects.UsedDevices;
using MLConsumer.DatabaseServices.MongoDB;
using MLConsumer.DatabaseServices.MongoDB.InterFaces;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;
using MLConsumer.QueueServices.RabbitMQ;

namespace MLConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureServices((hostContext, services) =>
             {
                 System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                 IConfiguration configuration = hostContext.Configuration;

                 // Gelen Logların işleneceği servis yapılandırması //
                 services.Configure<LogDatabaseSettings>(configuration.GetSection(nameof(LogDatabaseSettings)));
                 services.AddSingleton<ILogDatabaseSettings>(sp => sp.GetRequiredService<IOptions<LogDatabaseSettings>>().Value);
                 services.AddSingleton<ILogService<FortiGate>, LogService<FortiGate>>();
                 services.AddSingleton<ILogService<SonicWall>, LogService<SonicWall>>();
                 services.AddSingleton<ILogService<VmwareEsxi>, LogService<VmwareEsxi>>();

                 // Eklenen cihazlar servis yapılandırması //
                 services.Configure<DatabaseSettings<RegisteredDevice>>(configuration.GetSection(nameof(RegisteredDeviceDatabaseSettings)));
                 services.AddSingleton<IDatabaseSettings<RegisteredDevice>>(sp => sp.GetRequiredService<IOptions<DatabaseSettings<RegisteredDevice>>>().Value);
                 services.AddTransient<IDatabaseService<RegisteredDevice>, MongodbCrudService<RegisteredDevice>>();

                 // Eps servis yapılandırması //
                 services.Configure<DatabaseSettings<EpsData>>(configuration.GetSection(nameof(EpsDatabaseSettings)));
                 services.AddSingleton<IDatabaseSettings<EpsData>>(sp => sp.GetRequiredService<IOptions<DatabaseSettings<EpsData>>>().Value);
                 services.AddTransient<IDatabaseService<EpsData>, MongodbCrudService<EpsData>>();

                 // Kullanılan cihazlar servis yapılandırması //
                 services.Configure<DatabaseSettings<UsedDevice>>(configuration.GetSection(nameof(UsedDeviceDatabaseSettings)));
                 services.AddSingleton<IDatabaseSettings<UsedDevice>>(sp => sp.GetRequiredService<IOptions<DatabaseSettings<UsedDevice>>>().Value);
                 services.AddTransient<IDatabaseService<UsedDevice>, MongodbCrudService<UsedDevice>>();

                 // Keşfedilen cihazlar servis yapılandırması //
                 services.Configure<DatabaseSettings<UnRegisteredDevice>>(configuration.GetSection(nameof(UnRegisteredDeviceDatabaseSettings)));
                 services.AddSingleton<IDatabaseSettings<UnRegisteredDevice>>(sp => sp.GetRequiredService<IOptions<DatabaseSettings<UnRegisteredDevice>>>().Value);
                 services.AddTransient<IDatabaseService<UnRegisteredDevice>, MongodbCrudService<UnRegisteredDevice>>();

                 // Hata Raporları servis yapılandırması //
                 services.Configure<DatabaseSettings<ErrorLog>>(configuration.GetSection(nameof(ErrorDatabaseSettings)));
                 services.AddSingleton<IDatabaseSettings<ErrorLog>>(sp => sp.GetRequiredService<IOptions<DatabaseSettings<ErrorLog>>>().Value);
                 services.AddTransient<IDatabaseService<ErrorLog>, MongodbCrudService<ErrorLog>>();

                 //RabbitServer Servis Yapılandırması //
                 services.AddSingleton<RabbitServer>();

                 services.AddSingleton<ConsumerService, ConsumerService>();
                 services.AddSingleton<LogFormat>();
                 services.AddSingleton<ConsumerConfiguration>();
                 services.AddSingleton<IDatabaseObjects, MongoDBObjects>();


                 var sp = services.BuildServiceProvider();
                 var consumerService = sp.GetService<ConsumerService>();
                 consumerService.StartConsumer();

             });
    }
}
