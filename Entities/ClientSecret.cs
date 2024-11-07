using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities;

public class ClientSecret : Entity
{
    [Required]
    [MaxLength(6)]
    [Column(TypeName = "varchar")]
    public string ClientSecretKey { get; set; }

    // Foreign key to AspNetUsers table
    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser User { get; set; }
}