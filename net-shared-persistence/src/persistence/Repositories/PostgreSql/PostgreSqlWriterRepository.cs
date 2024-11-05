﻿using Net.Shared.Abstractions.Models.Data;
using Net.Shared.Persistence.Abstractions.Interfaces.Entities;
using Net.Shared.Persistence.Abstractions.Models.Contexts;
using Net.Shared.Persistence.Contexts;
using Net.Shared.Persistence.Abstractions.Interfaces.Repositories;

namespace Net.Shared.Persistence.Repositories.PostgreSql;

public class PostgreSqlWriterRepository<TContext, TEntity>(TContext context) : IPersistenceWriterRepository<TEntity>
    where TContext : PostgreSqlContext
    where TEntity : IPersistentSql
{
    private readonly TContext _context = context;

    public Task CreateOne<T>(T entity, CancellationToken cToken) where T : class, TEntity => 
        _context.CreateOne(entity, cToken);
    public async Task<Result<T>> TryCreateOne<T>(T entity, CancellationToken cToken) where T : class, TEntity
    {
        try
        {
            await CreateOne(entity, cToken);
            return new Result<T>(new[] { entity });
        }
        catch (Exception exception)
        {
            return new Result<T>(exception);
        }
    }

    public Task CreateMany<T>(IReadOnlyCollection<T> entities, CancellationToken cToken) where T : class, TEntity => 
        _context.CreateMany(entities, cToken);
    public async Task<Result<T>> TryCreateMany<T>(IReadOnlyCollection<T> entities, CancellationToken cToken) where T : class, TEntity
    {
        try
        {
            await CreateMany(entities, cToken);
            return new Result<T>(entities);
        }
        catch (Exception exception)
        {
            return new Result<T>(exception);
        }
    }

    public Task<T[]> Update<T>(PersistenceUpdateOptions<T> options, CancellationToken cToken) where T : class, TEntity => 
        _context.Update(options, cToken);
    public async Task<Result<T>> TryUpdate<T>(PersistenceUpdateOptions<T> options, CancellationToken cToken) where T : class, TEntity
    {
        try
        {
            var entities = await Update(options, cToken);
            return new Result<T>(entities);
        }
        catch (Exception exception)
        {
            return new Result<T>(exception);
        }
    }

    public Task<long> Delete<T>(PersistenceQueryOptions<T> options, CancellationToken cToken) where T : class, TEntity => 
        _context.Delete(options, cToken);
    public async Task<Result<long>> TryDelete<T>(PersistenceQueryOptions<T> options, CancellationToken cToken) where T : class, TEntity
    {
        try
        {
            var count = await Delete(options, cToken);
            return new Result<long>(count);
        }
        catch (Exception exception)
        {
            return new Result<long>(exception);
        }
    }

    #region Specialized API

    public Task UpdateOne<T>(T entity, CancellationToken cToken) where T : class, TEntity => 
        _context.UpdateOne(entity, cToken);
    public Task UpdateMany<T>(IEnumerable<T> entities, CancellationToken cToken) where T : class, TEntity => 
        _context.UpdateMany(entities, cToken);

    public Task DeleteOne<T>(T entity, CancellationToken cToken) where T : class, TEntity => 
        _context.DeleteOne(entity, cToken);
    public Task DeleteMany<T>(IEnumerable<T> entities, CancellationToken cToken) where T : class, TEntity => 
        _context.DeleteMany(entities, cToken);

    #endregion
}
