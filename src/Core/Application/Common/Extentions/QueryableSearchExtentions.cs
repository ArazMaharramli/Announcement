using System;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Common.Extentions
{
    public static class QueryableSearchExtentions
    {
        public static IQueryable<TSource> ContainsByField<TSource>(this IQueryable<TSource> q, string field, string value)
        {
            var eParam = Expression.Parameter(typeof(TSource), "x");
            var method = field.GetType().GetMethods().First(x => x.Name == "Contains" && x.GetParameters().Any(y => y.ParameterType == typeof(string)));
            var call = Expression.Call(Expression.Property(eParam, field), method, Expression.Constant(value.ToLower()));

            var lambdaExpression = Expression.Lambda<Func<TSource, bool>>(
                Expression.AndAlso(
                    Expression.NotEqual(Expression.Property(eParam, field), Expression.Constant(null)),
                    call
                ),
                eParam
            );

            return q.Where(lambdaExpression);
        }
    }
}
