using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Sensors;

public class DefaultInfoSensorDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public string ClientSecretKey { get; set; }
}
