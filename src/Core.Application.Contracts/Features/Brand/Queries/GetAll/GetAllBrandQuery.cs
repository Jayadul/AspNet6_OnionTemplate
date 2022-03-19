using Core.Domain.Shared.Wrappers;
using MediatR;

namespace Core.Application.Contracts.Features.Brand.Queries.GetAll
{
    public class GetAllBrandQuery : IRequest<Response<IReadOnlyList<GetAllBrandQueryVm>>>
    {
        public bool? IsActive { get; set; }
    }
}
