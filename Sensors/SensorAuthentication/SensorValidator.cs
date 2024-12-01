using Sensors.Data;

namespace Sensors.SensorAuthentication;

public class SensorValidator : ISensorValidator
{
    private readonly ApplicationDbContext _context;

    public SensorValidator(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Validate(string clientKey, string zoneKey, string sensorKey)
    {
        var clientSecret = _context.ClientSecrets.FirstOrDefault(cs => cs.ClientSecretKey == clientKey);
        if (clientSecret == null)
        {
            return false;
        }
        var sensor = _context.Sensors.FirstOrDefault(s=>s.ClientSecretKey == sensorKey && s.Zone.ClientSecretKey == zoneKey && s.Zone.Group.User.Id == clientSecret.UserId);
        if (sensor == null)
        {
            return false;
        }
        return true;
    }
}
