using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sensors.Data;
using Sensors.Models.SensorValues;

namespace Sensors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SensorValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "SensorAuthorizationPolicy")]
        [HttpPost]
        public ActionResult PostSensorValue(PostSensorValueDTO postSensorValueDTO)
        {
            if (_context.SensorValue == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SensorValue'  is null.");
            }

            Sensor sensor = _context.Sensor.FirstOrDefault(s => s.ClientSecretKey == postSensorValueDTO.SensorKey);
            if (null == sensor)
            {
                return Problem("Incorrect data passed.");
            }

            SensorValueType sensorValueType = _context.SensorValueType.FirstOrDefault(svt => svt.Code == postSensorValueDTO.SensorTypeCode);
            if (null == sensorValueType)
            {
                return Problem("Incorrect data passed.");
            }

            try
            {
                SensorValue sensorValue = new SensorValue { SensorId = sensor.Id, SensorValueTypeId = sensorValueType.Id, Value = postSensorValueDTO.ReadedValue };
                _context.SensorValue.Add(sensorValue);
                _context.SaveChanges();
            }
            catch {
                return Problem("Could not save sensor value!");
            }

            return Ok();
        }



    }
}
