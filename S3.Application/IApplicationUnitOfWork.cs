using S3.Application.Features.Repositories;
using S3.Domain.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        IS3BucketItemsRepo S3Bucket { get; }
    }
}
