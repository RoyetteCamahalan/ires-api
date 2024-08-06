using AutoMapper;
using AutoMapper.QueryableExtensions;
using ires.Domain.Common;
using ires.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> source,
            IPaginationInfo info
            )
        {
            return source.Skip((info.PageNumber - 1) * info.PageSize).Take(info.PageSize);
        }
        public static async Task<PaginatedResult<T>> AsPaginatedResult<T>(
            this IQueryable<T> source,
            IPaginationInfo info
            )
        {
            return new PaginatedResult<T>
            {
                TotalRecord = source.Count(),
                PageNumber = info.PageNumber,
                TotalPages = source.Count() / info.PageSize + (source.Count() % info.PageSize > 0 ? 1 : 0),
                Data = await source.Paginate(info).ToListAsync()
            };
        }
        public static async Task<PaginatedResult<T2>> AsPaginatedResult<T, T2>(
            this IQueryable<T> source,
            IPaginationInfo info,
            IConfigurationProvider configurationProvider
            )
        {
            return new PaginatedResult<T2>
            {
                TotalRecord = source.Count(),
                PageNumber = info.PageNumber,
                TotalPages = source.Count() / info.PageSize + (source.Count() % info.PageSize > 0 ? 1 : 0),
                Data = await source.Paginate(info).ProjectTo<T2>(configurationProvider).ToListAsync()
            };
        }
    }
}
