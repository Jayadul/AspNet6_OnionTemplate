using Core.Domain.Persistence.Entities;
using LinqToDB.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Persistence.Contracts
{
    public interface IPersistenceUnitOfWork : IDisposable
    {
        public IRepositoryAsync<Brand> Brand { get; }
        DataConnection Linq2Db { get; }

        Task<int> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTranscationAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
