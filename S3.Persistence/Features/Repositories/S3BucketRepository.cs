using Microsoft.EntityFrameworkCore;
using S3.Application.Features.Repositories;
using S3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace S3.Persistence.Features.Repositories
{
    public class S3BucketRepository : Repository<S3BucketItem, Guid>, IS3BucketItemsRepo
    {
        public S3BucketRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task<(IList<S3BucketItem> records, int total, int totalDisplay)>
            GetTableDataAsync(Expression<Func<S3BucketItem, bool>> expression,
            string orderBy, int pageIndex, int pageSize)
        {
            return await GetDynamicAsync(expression, orderBy, null,
                pageIndex, pageSize, true);
        }

       
    }
}
