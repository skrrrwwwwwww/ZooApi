/*using ZooApi.Application.DTOs;
using ZooApi.Domain.Entities;

namespace ZooApi.Infrastructure.Extensions;

public static class QuerybleExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }
}*/