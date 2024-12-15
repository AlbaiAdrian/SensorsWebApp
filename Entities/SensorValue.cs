using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class SensorValue : Entity
{
    public string Value { get; set; }

    public int SensorValueTypeId { get; set; }

    [ForeignKey("SensorValueTypeId")]
    public SensorValueType SensorValueType { get; set; }

    public int SensorId { get; set; }

    [ForeignKey("SensorId")]
    public Sensor Sensor { get; set; }
}
