using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Sensor: Entity
{
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar")]
    public string Name { get; set; } // Required with a max length of 100 characters

    [MaxLength(1000)]
    [Column(TypeName = "varchar")]
    public string? Description { get; set; } // Optional description field with varchar(max) type

    public int ZoneId { get; set; }

    [ForeignKey("ZoneId")]
    public Zone Zone { get; set; }

    [Required]
    [MaxLength(6)]
    [Column(TypeName = "varchar")]
    public string ClientSecretKey { get; set; }

}
