using MLConsumer.DatabaseObjects.RegisteredDevices;

namespace MLConsumer.DeviceAndParserServices.InterFaces
{
    public interface IParser
    {
        public void Work(string log, RegisteredDevice device);
    }
}
