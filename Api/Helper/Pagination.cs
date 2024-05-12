using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace Api.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(IReadOnlyList<T> data, int pageIndex, int pageSize, int count, bool hasNextPage)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
            HasNextPage = hasNextPage;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public bool HasNextPage { get; set; }

        public IReadOnlyList<T> Data { get; set; }

        public static Pagination<T> Paginate(List<T> data, int pageIndex, int pageSize)
        {
            if (pageSize <= 0) 
            {
                var totalCount = data.Count;

                return new(data, pageIndex, totalCount, totalCount, false);
            }
            else 
            {
                var totalCount = data.Count;
                var dataToReturn = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var responseDataCount = dataToReturn.Count;
                bool hasNextPage = pageIndex * pageSize < totalCount;
                return new(dataToReturn, pageIndex, pageSize, responseDataCount, hasNextPage);
            }

        }
    }
}
