using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities;

public class Group : Entity
{
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar")]
    public string Name { get; set; }

    [MaxLength(1000)]
    [Column(TypeName = "varchar")]
    public string? Description { get; set; }

    // Foreign key to AspNetUsers table
    [Required(ErrorMessage = "User id is required.")]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }
}
