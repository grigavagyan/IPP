using System.Linq.Expressions;

namespace Application.Common;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, string sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy)) return query;

        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(param, sortBy);
        var lambda = Expression.Lambda(property, param);

        var methodName = sortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

        var result = typeof(Queryable)
            .GetMethods()
            .Single(
                m => m.Name == methodName
                     && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null, new object[] { query, lambda }) as IQueryable<T>;

        return result!;
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
