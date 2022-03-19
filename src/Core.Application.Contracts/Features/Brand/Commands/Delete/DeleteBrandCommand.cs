using Core.Domain.Shared.Wrappers;
using MediatR;

namespace Core.Application.Contracts.Features.Brand.Commands.Delete
{
    public class DeleteBrandCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }
}