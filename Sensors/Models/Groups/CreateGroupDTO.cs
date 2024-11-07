using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Groups;

public class CreateGroupDTO
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
}
