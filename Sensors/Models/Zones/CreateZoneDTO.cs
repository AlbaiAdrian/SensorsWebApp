using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Zones;

public class CreateZoneDTO
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}
