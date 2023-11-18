using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using S3.Application.Features.Services;
using S3.Infrastructure;
using S3.WEB.Constants;
using S3.WEB.Controllers;
using Amazon.S3.Transfer;
using Azure;
using Microsoft.Net.Http.Headers;

namespace S3.WEB.Models
{
    public class S3BucketItemListModel
    {
        private IS3BucketService _s3BucketService;
       private ILogger<S3Controller> _logger;
        public S3BucketItemListModel() { }

        public S3BucketItemListModel(IS3BucketService s3BucketService, ILogger<S3Controller> logger)
        {
            _s3BucketService = s3BucketService;
            _logger = logger;
        }

        internal void DeleteItem(Guid id)
        {
            _s3BucketService.DeleteItem(id);
        }

        internal string GetFileName(Guid Id)
        {
            return _s3BucketService.GetItem(Id).ItemName;
        }

        public void DeleteFileFromS3(string fileName,Guid id)
        {
            string bucketName = AWSConstants.bucketName;

            
            var awsS3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1, 
            };

            using (var s3Client = new AmazonS3Client(awsS3Config))
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                };

                try
                {
                    s3Client.DeleteObjectAsync(deleteObjectRequest).Wait(); // Wait for the deletion to complete
                    DeleteItem(id);
                    _logger.LogInformation("File deleted successfully");
                   
                }
                catch (AmazonS3Exception ex)
                {
                   
                   _logger.LogInformation("File deleted Failed!!");
                }
            }
        }

        public Stream GetFileAsStreamFromS3(string fileName)
        {
            string bucketName = AWSConstants.bucketName;

            var awsS3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1,
            };

            using (var s3Client = new AmazonS3Client(awsS3Config))
            {
                var fileTransferUtility = new TransferUtility(s3Client);

                // Retrieve the file as a stream
                var fileStream = fileTransferUtility.OpenStream(bucketName, fileName);

                return fileStream;
            }
        }




       
        public async Task<object> GetS3BucketItemsAsync(DataTablesAjaxRequestUtility dataTablesUtility)
        {
            var data = await _s3BucketService.GetPagedBucketItemsAsync(
                dataTablesUtility.PageIndex,
                dataTablesUtility.PageSize,
                dataTablesUtility.SearchText,
                dataTablesUtility.GetSortText(new string[] { "ItemName", "Id" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.ItemName,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

    }
}
