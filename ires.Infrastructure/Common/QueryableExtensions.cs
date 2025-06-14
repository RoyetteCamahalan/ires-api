using AutoMapper;
using AutoMapper.QueryableExtensions;
using ires.Domain.Common;
using ires.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Infrastructure.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> source,
            IPaginationInfo info
            )
        {
            if (info.PageNumber == 0)
                return source;
            return source.Skip((info.PageNumber - 1) * info.PageSize).Take(info.PageSize);
        }
        public static async Task<PaginatedResult<T>> AsPaginatedResult<T>(
            this IQueryable<T> source,
            IPaginationInfo info
            )
        {
            return new PaginatedResult<T>
            {
                totalRecord = source.Count(),
                pageNumber = info.PageNumber,
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
            return new PaginatedResult<T2>
            {
                totalRecord = source.Count(),
                pageNumber = info.PageNumber,
                totalPages = source.Count() / info.PageSize + (source.Count() % info.PageSize > 0 ? 1 : 0),
                data = await source.Paginate(info).ProjectTo<T2>(configurationProvider).ToListAsync()
            };
        }
        public static async Task<PaginatedResult<T2>> AsPaginatedResult<T, T2>(
            this IQueryable<T> source,
            IPaginationInfo info,
            IMapper _mapper
            )
        {
            var data = await source.Paginate(info).ToListAsync();
            return new PaginatedResult<T2>
            {
                totalRecord = source.Count(),
                pageNumber = info.PageNumber,
                totalPages = source.Count() / info.PageSize + (source.Count() % info.PageSize > 0 ? 1 : 0),
                data = _mapper.Map<IEnumerable<T2>>(data)
            };
        }
    }
}
