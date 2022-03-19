using Core.Application.Contracts.Features.Brand.Commands.Delete;
using Core.Application.Features.Brand.Commands.Delete;
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

namespace Core.Application.Features.Brand.Commands.Delete
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Response<bool>>
    {
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly ILogger<DeleteBrandCommandHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private readonly List<string> _validationError;

        public DeleteBrandCommandHandler(IPersistenceUnitOfWork persistenceUnitOfWork, ILogger<DeleteBrandCommandHandler> logger)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(DeleteBrandCommandHandlerResource));
            _validationError = new List<string>();
        }
        public async Task<Response<bool>> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _persistenceUnitOfWork.Brand.GetByIdAsync(command.Id);
                if (brand == null)
                {
                    _logger.LogError(_resourceManager.GetString("Not_found"));
                    _validationError.Add(_resourceManager.GetString("Not_found"));
                    _persistenceUnitOfWork.Dispose();
                }
                else
                {
                    await _persistenceUnitOfWork.Brand.DeleteAsync(brand);
                    await _persistenceUnitOfWork.SaveChangesAsync();
                    return Response<bool>.Success(true, _resourceManager.GetString("Deleted"));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, _resourceManager.GetString("Fail_Error"));
                _persistenceUnitOfWork.Dispose();
            }
            return Response<bool>.Fail(_resourceManager.GetString("Fail"), _validationError);
        }
    }
}
