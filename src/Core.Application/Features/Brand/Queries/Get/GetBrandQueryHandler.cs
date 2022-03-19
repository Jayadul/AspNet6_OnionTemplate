using AutoMapper;
using Core.Application.Contracts.Features.Brand.Queries.Get;
using Core.Domain.Persistence.Contracts;
using Core.Domain.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Resources;

namespace Core.Application.Features.Brand.Queries.Get
{
    public class GetBrandQueryHandler : IRequestHandler<GetBrandQuery, Response<GetBrandQueryViewModel>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBrandQueryHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private readonly List<string> _validationError;

        public GetBrandQueryHandler(IPersistenceUnitOfWork persistenceUnitOfWork, IMapper mapper,
            ILogger<GetBrandQueryHandler> logger)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _mapper = mapper;
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(GetBrandQueryHandlerResource));
            _validationError = new List<string>();
        }
        public async Task<Response<GetBrandQueryViewModel>> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _persistenceUnitOfWork.Brand.GetByIdAsync(request.Id);
                if (brand == null)
                {
                    _logger.LogError(_resourceManager.GetString("No_Valid_Brand"));
                    _validationError.Add(_resourceManager.GetString("No_Valid_Brand"));
                    _persistenceUnitOfWork.Dispose();
                }
                else
                {
                    var brandModel = new GetBrandQueryViewModel();
                    brandModel.Id = brand.Id;
                    brandModel.Name = brand.Name??"";
                    brandModel.EffectiveEndDate = brand.EffectiveEndDate;
                    return Response<GetBrandQueryViewModel>.Success(brandModel, _resourceManager.GetString("Success"));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, _resourceManager.GetString("Fail_Error"));
                _validationError.Add( _resourceManager.GetString("Fail_Error"));
                _persistenceUnitOfWork.Dispose();
            }
            return Response<GetBrandQueryViewModel>.Fail(_resourceManager.GetString("Fail"), _validationError);
        }
    }
}
