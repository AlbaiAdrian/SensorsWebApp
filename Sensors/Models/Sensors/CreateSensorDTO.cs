using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Sensors;

public class CreateSensorDTO
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}
