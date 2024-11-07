using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public interface IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
