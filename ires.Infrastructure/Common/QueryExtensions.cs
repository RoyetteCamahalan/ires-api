using ires.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace ires.Infrastructure.Common
{
    public static class QueryExtensions
    {
        public static EntityTypeBuilder BelongsToCompany(
            this EntityTypeBuilder builder,
            Type type,
            ICurrentUserContext context)
        {
            var param = Expression.Parameter(type, "x");
            var convertedParam = Expression.Convert(param, type);
            var userContextInstance = Expression.Constant(context);
            var entityCompanyId = Expression.Property(convertedParam, "CompanyId");
            var contextCompanyId = Expression.Property(userContextInstance, typeof(ICurrentUserContext).GetProperty(nameof(ICurrentUserContext.companyid))!);
            var companyFilterExpression = Expression.Equal(entityCompanyId, Expression.Convert(contextCompanyId, entityCompanyId.Type));

            builder.HasIndex("CompanyId").HasDatabaseName($"IX_{type.Name}_CompanyId");
            return builder.HasQueryFilter(Expression.Lambda(companyFilterExpression, param));
        }
    }
}
