﻿using _305.Application.Filters.Pagination;
using _305.Domain.Common;
using System.Linq.Expressions;

namespace _305.Application.IBaseRepository;
/// <summary>
/// اینترفیس مخزن داده عمومی برای عملیات پایه‌ای روی موجودیت‌ها.
/// </summary>
/// <typeparam name="TEntity">نوع موجودیت که باید کلاس و IBaseEntity باشد.</typeparam>
public interface IRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<bool> ExistsAsync();
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    int Count();
    int Count(Expression<Func<TEntity, bool>> predicate);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<TEntity> AddAsyncReturnId(TEntity entity);

    Task<TEntity?> FindSingle(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    List<TEntity> FindList(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    Task<List<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null, CancellationToken cancellationToken = default);
    Task<List<TEntity>> FindListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null, CancellationToken cancellationToken = default);

    Task<TEntity?> FindSingleAsNoTracking(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    Task<TEntity?> FindFirstAsNoTracking(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    List<TEntity> FindListAsNoTracking(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);

    Task<PaginatedList<TEntity>> GetPagedResultAsync(
    DefaultPaginationFilter filter,
    Expression<Func<TEntity, bool>>? predicate = null,
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
    params Expression<Func<TEntity, string?>>[]? searchFields);
}
