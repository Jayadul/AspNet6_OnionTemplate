using Core.Domain.Shared.Wrappers;
using MediatR;
using System;

namespace Core.Application.Contracts.Features.Brand.Commands.UpdateDate
{
    public class UpdateBrandEffectiveDateCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DateTime? EffectiveEndDate { get; set; }

    }
}
