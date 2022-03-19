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
using Core.Application.Contracts.Features.Brand.Commands;
using Core.Application.Features.Brand.Commands.Create;
using Core.Application.Contracts.Features.Brand.Commands.Create;

namespace Core.Application.Features.Brand.Commands.Create
{

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Response<int>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly ILogger<CreateBrandCommandHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private List<String> _validationError;
        private readonly IFileManagementRepository _fileManagementRepository;

        public CreateBrandCommandHandler(IPersistenceUnitOfWork persistenceUnitOfWork, ILogger<CreateBrandCommandHandler> logger, IFileManagementRepository fileManagementRepository)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(CreateBrandCommandHandlerResource));
            _validationError = new List<string>();
            _fileManagementRepository = fileManagementRepository;
        }

        public async Task<Response<int>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (Validate(command))
                {
                    await _persistenceUnitOfWork.BeginTranscationAsync();
                    var newBrand = new Domain.Persistence.Entities.Brand
                    {
                        Name = command.Name,
                        CreationDate= DateTime.Now,
                    };
                    var transaction = await _persistenceUnitOfWork.Brand.AddAsync(newBrand);
                    await _persistenceUnitOfWork.SaveChangesAsync();
                    await _persistenceUnitOfWork.CommitTransactionAsync();
                    return Response<int>.Success(Convert.ToInt32(transaction.Id), _resourceManager.GetString("Success"));
                }
            }
            catch (Exception e)
            {
                await _persistenceUnitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, _resourceManager.GetString("Failed"));
                _persistenceUnitOfWork.Dispose();
            }
            return Response<int>.Fail(_resourceManager.GetString("Fail"), _validationError);

        }

        private bool Validate(CreateBrandCommand command)
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
