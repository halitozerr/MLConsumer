using MLConsumer.DatabaseObjects.Eps;
using MLConsumer.DatabaseObjects.Error;
using MLConsumer.DatabaseObjects.RegisteredDevices;
using MLConsumer.DatabaseObjects.UnRegisteredDevices;
using MLConsumer.DatabaseObjects.UsedDevices;
using MLConsumer.DatabaseServices.MongoDB.MongodbGenericStructure.InterFaces;

namespace MLConsumer.DatabaseObjects
{
    public interface IDatabaseObjects
    {
        public IDatabaseService<RegisteredDevice> _registeredDeviceService { get; set; }    
        public  IDatabaseService<EpsData> _eps { get; set; }
        public  IDatabaseService<UsedDevice> _usedDevice { get; set; }
        public  IDatabaseService<UnRegisteredDevice> _unRegisteredDeviceService { get; set; }
        public  IDatabaseService<ErrorLog> _errorLogService { get; set; }
    }
}
