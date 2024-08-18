using AutoMapper;
using AutoMapper.QueryableExtensions;
using ires.Domain.Common;
using ires.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ires.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> source,
            IPaginationInfo info
            )
        {
            if (info.currentPage == 0)
                return source;
            return source.Skip((info.currentPage - 1) * info.PageSize).Take(info.PageSize);
        }
        public static async Task<PaginatedResult<T>> AsPaginatedResult<T>(
            this IQueryable<T> source,
            IPaginationInfo info
            )
        {
            if (info.currentPage == 0)
                return new PaginatedResult<T>
                {
                    totalRecord = source.Count(),
                    currentPage = info.currentPage,
                    totalPages = 1,
                    data = await source.ToListAsync()
                };
            return new PaginatedResult<T>
            {
                totalRecord = source.Count(),
                currentPage = info.currentPage,
                totalPages = source.Count() / info.PageSize + (source.Count() % info.PageSize > 0 ? 1 : 0),
                data = await source.Paginate(info).ToListAsync()
            };
        }
        public static async Task<PaginatedResult<T2>> AsPaginatedResult<T, T2>(
            this IQueryable<T> source,
            IPaginationInfo info,
            IConfigurationProvider configurationProvider
            )
        {
            if (info.currentPage == 0)
                return new PaginatedResult<T2>
                {
                    totalRecord = source.Count(),
                    currentPage = info.currentPage,
                    totalPages = 1,
                    data = await source.ProjectTo<T2>(configurationProvider).ToListAsync()
                };
            return new PaginatedResult<T2>
            {
                totalRecord = source.Count(),
                currentPage = info.currentPage,
                totalPages = source.Count() / info.PageSize + (source.Count() % info.PageSize > 0 ? 1 : 0),
                data = await source.Paginate(info).ProjectTo<T2>(configurationProvider).ToListAsync()
            };
        }
    }
}
