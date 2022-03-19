using Core.Domain.Persistence.Common;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Persistence.Entities
{
    public class Brand : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
