using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Sensors;

public class EditSensorDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}
