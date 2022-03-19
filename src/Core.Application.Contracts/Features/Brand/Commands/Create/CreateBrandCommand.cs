using Core.Domain.Shared.Wrappers;
using MediatR;

namespace Core.Application.Contracts.Features.Brand.Commands.Create
{
    public class CreateBrandCommand : IRequest<Response<int>>
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = !string.IsNullOrEmpty(value) ? value.Trim() : value; }
        }
    }
}
