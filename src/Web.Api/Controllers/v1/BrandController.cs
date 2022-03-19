using Core.Application.Contracts.Features.Brand.Commands;
using Core.Application.Contracts.Features.Brand.Commands.Create;
using Core.Application.Contracts.Features.Brand.Commands.Delete;
using Core.Application.Contracts.Features.Brand.Commands.Update;
using Core.Application.Contracts.Features.Brand.Commands.UpdateDate;
using Core.Application.Contracts.Features.Brand.Queries.Get;
using Core.Application.Contracts.Features.Brand.Queries.GetAll;
using Core.Domain.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Web.Api.Controllers
{
    [ApiVersion("1.0")]
    public class BrandController : BaseApiController
    { 
        [HttpPost]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status422UnprocessableEntity,
        //    Type = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary))]
        public async Task<IActionResult> Create(CreateBrandCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<GetAllBrandQueryVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(bool? isActive)
        {
            GetAllBrandQuery query = new GetAllBrandQuery { IsActive = isActive };
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetBrandQueryViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(int id)
        {
            GetBrandQuery query = new GetBrandQuery { Id = id };
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response<int>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status422UnprocessableEntity,
        //    Type = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary))]
        public async Task<IActionResult> Update(UpdateBrandCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(DeleteBrandCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }   
    }
}
