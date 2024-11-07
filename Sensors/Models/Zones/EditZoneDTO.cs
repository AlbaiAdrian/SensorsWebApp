using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Zones;

public class EditZoneDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}
