using Core.Domain.Shared.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Persistence.Contracts;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Resources;
using Core.Domain.Shared.Contacts;
using Core.Application.Contracts.Features.Brand.Commands.Update;

namespace Core.Application.Features.Brand.Commands.Update
{

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Response<int>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly ILogger<UpdateBrandCommandHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private List<String> _validationError;

        public UpdateBrandCommandHandler(IPersistenceUnitOfWork persistenceUnitOfWork, ILogger<UpdateBrandCommandHandler> logger)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(UpdateBrandCommandHandlerResource));
            _validationError = new List<string>();
        }

        public async Task<Response<int>> Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (Validate(command))
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
                        brand.Name = command.Name;
                        brand.CreationDate = DateTime.Now;
                        await _persistenceUnitOfWork.Brand.UpdateAsync(brand);
                        await _persistenceUnitOfWork.SaveChangesAsync();
                        return Response<int>.Success(Convert.ToInt32(brand.Id), _resourceManager.GetString("Success"));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, _resourceManager.GetString("Failed"));
                _persistenceUnitOfWork.Dispose();
            }
            return Response<int>.Fail(_resourceManager.GetString("Fail"), _validationError);

        }

        private bool Validate(UpdateBrandCommand command)
        {
            var valid = true;
            try
            {
                if (string.IsNullOrWhiteSpace(command.Name))
                {
                    valid = false;
                    _validationError.Add(_resourceManager.GetString("Name_Required"));
                    _logger.LogError(_resourceManager.GetString("Name_Required"));
                }
             
            }
            catch (Exception)
            {
                valid = false;
            }
            return valid;
        }
    }
}
