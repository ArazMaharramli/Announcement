using System.Linq;
using System.Linq.Expressions;

namespace Application.Common.Extentions
{
    public static class QueryableOrderByExtentions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var orderByDescendingMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
            var orderByDescendingGeneric = orderByDescendingMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByDescendingGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }
    }
}
