using Core.Domain.Shared.Wrappers;
using MediatR;
using System;

namespace Core.Application.Contracts.Features.Brand.Commands.Update
{
    public class UpdateBrandCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = !string.IsNullOrEmpty(value) ? value.Trim() : value; }
        }
    }
}
