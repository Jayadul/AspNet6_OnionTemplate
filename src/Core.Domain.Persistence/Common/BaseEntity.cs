using System;

namespace Core.Domain.Persistence.Common
{
    public class BaseEntity
    {
        public bool Predefined { get; set; }
        public string? PredefinedValue { get; set; }
        public DateTime? CreationDate { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? LastUpdatedBy { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool? Archived { get; set; }
    }
}
