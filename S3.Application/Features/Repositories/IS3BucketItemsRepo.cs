using S3.Domain.Entities;
using S3.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace S3.Application.Features.Repositories
{
    public interface IS3BucketItemsRepo : IRepositoryBase<S3BucketItem, Guid>
    {
        Task<(IList<S3BucketItem> records, int total, int totalDisplay)>
            GetTableDataAsync(Expression<Func<S3BucketItem, bool>> expression, string orderBy,
            int pageIndex, int pageSize);
        
    }
}
