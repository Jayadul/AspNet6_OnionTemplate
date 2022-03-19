using Core.Domain.Shared.Wrappers;
using MediatR;

namespace Core.Application.Contracts.Features.Brand.Queries.Get
{
    public class GetBrandQuery : IRequest<Response<GetBrandQueryViewModel>>
    {
        public int Id { get; set; }
    }
}
