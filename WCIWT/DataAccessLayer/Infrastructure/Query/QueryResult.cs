using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCIWT.Infrastructure.Query
{
    public class QueryResult<TEntity> where TEntity : IEntity
    {
        public QueryResult(IList<TEntity> items, long totalItemsCount, int pageSize = 10, int? requestedPageNumber = 0)
        {
            TotalItemsCount = totalItemsCount;
            RequestedPageNumber = requestedPageNumber;
            PageSize = pageSize;
            Items = items;
        }

        public long TotalItemsCount { get; }

        public int? RequestedPageNumber { get; }

        public int PageSize { get; }

        public IList<TEntity> Items { get; }

        public bool PaginationEnabled => RequestedPageNumber != null;

        protected bool Equals(QueryResult<TEntity> other)
        {
            return TotalItemsCount == other.TotalItemsCount &&
                   RequestedPageNumber == other.RequestedPageNumber &&
                   PageSize == other.PageSize &&
                   Items.All(entity => other.Items.Select(item => item.Id).Contains(entity.Id));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((QueryResult<TEntity>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TotalItemsCount.GetHashCode();
                hashCode = (hashCode * 397) ^ RequestedPageNumber.GetHashCode();
                hashCode = (hashCode * 397) ^ PageSize;
                hashCode = (hashCode * 397) ^ (Items != null ? Items.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
