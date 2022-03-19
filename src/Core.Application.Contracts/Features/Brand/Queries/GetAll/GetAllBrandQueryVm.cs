using System;
namespace Core.Application.Contracts.Features.Brand.Queries.GetAll
{
    public class GetAllBrandQueryVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime? EffectiveEndDate { get; set; }

    }
}
