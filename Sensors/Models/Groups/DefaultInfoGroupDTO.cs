using System.ComponentModel.DataAnnotations;

namespace Sensors.Models.Groups
{
    public class DefaultInfoGroupDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
