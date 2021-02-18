using MLConsumer.DatabaseObjects;
using MLConsumer.DatabaseObjects.Eps;
using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseObjects.RegisteredDevices;
using MLConsumer.DatabaseObjects.UnRegisteredDevices;
using MLConsumer.DatabaseObjects.UsedDevices;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;

namespace MLConsumer.DatabaseServices.MongoDB
{
    public class MongoDBObjects : IDatabaseObjects
    {
        public IDatabaseService<RegisteredDevice> _registeredDeviceService { get; set; }
        public IDatabaseService<EpsData> _eps { get; set; }
        public IDatabaseService<UsedDevice> _usedDevice { get; set; }
        public IDatabaseService<UnRegisteredDevice> _unRegisteredDeviceService { get; set; }
        public IDatabaseService<ErrorLog> _errorLogService { get; set; }

        public MongoDBObjects(
            IDatabaseService<EpsData> eps,
            IDatabaseService<UsedDevice> usedDevice,
            IDatabaseService<ErrorLog> errorLogService,
            IDatabaseService<UnRegisteredDevice> unRegisteredDeviceService,
            IDatabaseService<RegisteredDevice> registeredDeviceService)
        {
            _registeredDeviceService = registeredDeviceService;
            _eps = eps;
            _usedDevice = usedDevice;
            _unRegisteredDeviceService = unRegisteredDeviceService;
            _errorLogService = errorLogService;

        }
    }
}
