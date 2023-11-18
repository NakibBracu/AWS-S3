using Microsoft.EntityFrameworkCore;
using S3.Application;
using S3.Application.Features.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3.Persistence
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IS3BucketItemsRepo S3Bucket { get; private set; }

        public ApplicationUnitOfWork(IApplicationDbContext dbContext,
            IS3BucketItemsRepo S3BucketRepository) : base((DbContext)dbContext)
        {
            S3Bucket = S3BucketRepository;
        }
    }
}
