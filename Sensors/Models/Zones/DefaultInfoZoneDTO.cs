using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Zones;

public class DefaultInfoZoneDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public string ClientSecretKey { get; set; }
}
