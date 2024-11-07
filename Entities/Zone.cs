using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Zone : Entity
{
    [Key]
    public int Id { get; set; } // Primary key

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar")]
    public string Name { get; set; } // Required with a max length of 100 characters

    [MaxLength(1000)]
    [Column(TypeName = "varchar")]
    public string? Description { get; set; } // Optional description field with varchar(max) type

    public int GroupId { get; set; } // Foreign key to Group

    [ForeignKey("GroupId")]
    public Group? Group { get; set; }

    [Required]
    [MaxLength(6)]
    [Column(TypeName = "varchar")]
    public string ClientSecretKey { get; set; }
}
