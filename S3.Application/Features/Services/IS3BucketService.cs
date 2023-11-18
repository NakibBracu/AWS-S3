using S3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3.Application.Features.Services
{
    public interface IS3BucketService
    {
        void CreateItem(string name);
        void DeleteItem(Guid id);
        S3BucketItem GetItem(Guid id);
        public IList<S3BucketItem> GetItems();
        Task<(IList<S3BucketItem> records, int total, int totalDisplay)> GetPagedBucketItemsAsync(int pageIndex,
             int pageSize, string searchText, string orderBy);
        void UpdateItem(Guid id, string name);
    }
}
