using AutoMapper;
using Core.Application.Contracts.Features.Brand.Queries.GetAll;
using Core.Application.Helpers;
using Core.Domain.Persistence.Contracts;
using Core.Domain.Persistence.Enums;
using Core.Domain.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Features.Brand.Queries.GetAll
{
    /// <summary>
    /// cqrs for retrieving all clients
    /// </summary>
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandQuery, Response<IReadOnlyList<GetAllBrandQueryVm>>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllBrandQueryHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private readonly List<string> _validationError;

        public GetAllBrandQueryHandler(IPersistenceUnitOfWork persistenceUnitOfWork, IMapper mapper, ILogger<GetAllBrandQueryHandler> logger)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(GetAllBrandQueryHandlerResource));
            _validationError = new List<string>();
        }

        public async Task<Response<IReadOnlyList<GetAllBrandQueryVm>>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allBrand = _persistenceUnitOfWork.Brand.AsNoTracking().ToList();
                if (allBrand == null)
                {
                    _validationError.Add(_resourceManager.GetString("No_brand"));
                    _logger.LogError(_resourceManager.GetString("No_brand"));
                    
                }
                if (request.IsActive != null)
                {
                    if (request.IsActive == true)
                    {
                        allBrand = allBrand.Where(w => w.EffectiveEndDate >= DateTime.Now || w.EffectiveEndDate == null).ToList();
                    }
                    else
                    {
                        allBrand = allBrand.Where(w => w.EffectiveEndDate < DateTime.Now).ToList();
                    }
                }
                var brands = new List<GetAllBrandQueryVm>();
                foreach (var brand in allBrand)
                {
                    var brandModel = new GetAllBrandQueryVm();
                    brandModel.Id = brand.Id;
                    brandModel.Name = brand.Name??"";
                    brandModel.EffectiveEndDate = brand.EffectiveEndDate;
                    brands.Add(brandModel);
                }
                return Response<IReadOnlyList<GetAllBrandQueryVm>>.Success(brands.OrderByDescending(x => x.Id).ToList(), _resourceManager.GetString("Success"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, _resourceManager.GetString("Fail"));
                _persistenceUnitOfWork.Dispose();
            }
            return Response<IReadOnlyList<GetAllBrandQueryVm>>.Fail(_resourceManager.GetString("Fail"), _validationError);
        }
    }
}
