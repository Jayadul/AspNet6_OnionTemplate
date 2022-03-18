using Core.Domain.Persistence.Contracts;
using Core.Domain.Persistence.Entities;
using Infrastructure.Persistence.Context;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PersistenceUnitOfWork : IPersistenceUnitOfWork
    {
        public IRepositoryAsync<Audit> Audit { get; }
        private readonly AppDbContext _dbContext;
        private bool _disposed;

        public PersistenceUnitOfWork(AppDbContext appDbContext,
            IRepositoryAsync<Audit> auditRepository)
        {
            _dbContext = appDbContext;
            Audit = auditRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public DataConnection Linq2Db => _dbContext.CreateLinqToDbConnection();



        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _dbContext.Dispose();
            _disposed = true;
        }

        public async Task<IDbContextTransaction> BeginTranscationAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
