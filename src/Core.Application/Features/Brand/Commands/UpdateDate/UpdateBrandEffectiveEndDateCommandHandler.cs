using Core.Application.Contracts.Features.Brand.Commands.UpdateDate;
using Core.Domain.Persistence.Contracts;
using Core.Domain.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Features.Brand.Commands.UpdateDate
{
    public class UpdateEffectiveEndDateCommandHandler : IRequestHandler<UpdateBrandEffectiveDateCommand, Response<int>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly ILogger<UpdateEffectiveEndDateCommandHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private List<String> _validationError;

        public UpdateEffectiveEndDateCommandHandler(IPersistenceUnitOfWork persistenceUnitOfWork, ILogger<UpdateEffectiveEndDateCommandHandler> logger)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(UpdateBrandEffectiveEndDateCommandHandlerResource));
            _validationError = new List<string>();
        }
        public async Task<Response<int>> Handle(UpdateBrandEffectiveDateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _persistenceUnitOfWork.Brand.GetByIdAsync(command.Id);
                if (brand == null)
                {
                    _validationError.Add(_resourceManager.GetString("Invalid_Brand"));
                    _logger.LogError(_resourceManager.GetString("Invalid_Brand"));
                    _persistenceUnitOfWork.Dispose();
                }
                else
                {
                    brand.EffectiveEndDate = command.EffectiveEndDate;
                    brand.LastUpdatedDate = DateTime.Now;
                    await _persistenceUnitOfWork.Brand.UpdateAsync(brand);
                    await _persistenceUnitOfWork.SaveChangesAsync();
                    return Response<int>.Success(Convert.ToInt32(brand.Id), _resourceManager.GetString("Success"));
                }
            }
            catch (Exception e)
            {
                await _persistenceUnitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, _resourceManager.GetString("Failed"));
                _persistenceUnitOfWork.Dispose();
            }
            return Response<int>.Fail(_resourceManager.GetString("Failed"), _validationError);
        }
    }
}
