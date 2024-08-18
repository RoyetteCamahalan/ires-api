using AutoMapper;
using ires.Domain.Common;
using ires.Domain.Contracts;

namespace ires.Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T> Paginate<T>(
            this List<T> source,
            IPaginationInfo info
            )
        {
            if (info.currentPage == 0)
                return source;
            return source.Skip((info.currentPage - 1) * info.PageSize).Take(info.PageSize).ToList();
        }
        public static PaginatedResult<T> AsPaginatedResult<T>(
            this List<T> source,
            IPaginationInfo info
            )
        {
            if (info.currentPage == 0)
                return new PaginatedResult<T>
                {
                    totalRecord = source.Count,
                    currentPage = info.currentPage,
                    totalPages = 1,
                    data = source
                };
            return new PaginatedResult<T>
            {
                totalRecord = source.Count,
                currentPage = info.currentPage,
                totalPages = source.Count / info.PageSize + (source.Count % info.PageSize > 0 ? 1 : 0),
                data = source.Paginate(info)
            };
        }
        public static PaginatedResult<T2> AsPaginatedResult<T, T2>(
            this List<T> source,
            IPaginationInfo info,
            IMapper _mapper
            )
        {
            if (info.currentPage == 0)
                return new PaginatedResult<T2>
                {
                    totalRecord = source.Count,
                    currentPage = info.currentPage,
                    totalPages = 1,
                    data = _mapper.Map<List<T2>>(source)
                };
            return new PaginatedResult<T2>
            {
                totalRecord = source.Count,
                currentPage = info.currentPage,
                totalPages = source.Count / info.PageSize + (source.Count % info.PageSize > 0 ? 1 : 0),
                data = _mapper.Map<List<T2>>(source)
            };
        }
    }
}
