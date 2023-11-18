using S3.Application;
using S3.Application.Features.Services;
using S3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3.Infrastructure.Services
{
    public class S3Service: IS3BucketService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public S3Service(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateItem(string name)
        {
            S3BucketItem s3BucketItem = new S3BucketItem()
            {
                ItemName = name,
            };

            _unitOfWork.S3Bucket.Add(s3BucketItem);
            _unitOfWork.Save();
        }

        public void DeleteItem(Guid id)
        {
            _unitOfWork.S3Bucket.Remove(id);
            _unitOfWork.Save();
        }

        public S3BucketItem GetItem(Guid id)
        {
           return _unitOfWork.S3Bucket.GetById(id);
        }

        public IList<S3BucketItem> GetItems()
        {
           return _unitOfWork.S3Bucket.GetAll();
        }

        public async Task<(IList<S3BucketItem> records, int total, int totalDisplay)> GetPagedBucketItemsAsync(int pageIndex,
             int pageSize, string searchText, string orderBy)
        {
            var result = await _unitOfWork.S3Bucket.GetTableDataAsync(
                x => x.ItemName.Contains(searchText), orderBy, pageIndex, pageSize);

            return result;
        }

        public void UpdateItem(Guid id, string name)
        {
            S3BucketItem bucketItem = _unitOfWork.S3Bucket.GetById(id);
            bucketItem.ItemName = name;
            _unitOfWork.Save();
        }
    }
}
