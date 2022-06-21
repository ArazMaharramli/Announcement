using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Persistence.Extentions
{
    public static class ExtentionMethods
    {
        public static void ApplyGlobalQueryFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> expression)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.BaseType != null && entityType.ClrType.BaseType == typeof(T))
                {
                    var newParam = Expression.Parameter(entityType.ClrType);
                    var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(Expression.Lambda(newBody, newParam));
                }
            }
        }
    }
}

