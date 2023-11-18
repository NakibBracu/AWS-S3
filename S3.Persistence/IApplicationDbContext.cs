using Microsoft.EntityFrameworkCore;
using S3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<S3BucketItem> BucketItems { get; set; }
    }
}
