namespace Sensors.SensorAuthentication;

public interface ISensorValidator
{
    public bool Validate(string clientKey, string zoneKey, string sensorKey);
}
