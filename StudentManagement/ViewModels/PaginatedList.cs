using Microsoft.EntityFrameworkCore;

namespace StudentManagement.ViewModels
{
    public class PaginatedList<T>:List<T>
    {
        public int PageIndex { get;}
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {

            var totalCount = await source.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

           
            pageIndex = Math.Clamp(pageIndex, 1, Math.Max(totalPages, 1));

            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
        }
    }
}
