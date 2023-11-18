using Amazon.S3.Transfer;
using Amazon.S3;
using Autofac;
using S3.Application.Features.Services;
using S3.WEB.Constants;

namespace S3.WEB.Models
{
    public class S3BucketItemCreateModel
    {


        private IS3BucketService _s3BucketService;
        public S3BucketItemCreateModel() { }

        public S3BucketItemCreateModel(IS3BucketService s3BucketService)
        {
            _s3BucketService = s3BucketService;
        }

        internal void ResolveDependency(ILifetimeScope scope)
        {
            _s3BucketService = scope.Resolve<IS3BucketService>(); //same type object like thing done!
        }


        internal void CreateItem(string name)
        {
            _s3BucketService.CreateItem(name);
        }

        public void UploadFilesToS3(List<IFormFile> files)
        {
            if (files.Count > 0)
            {
                var awsS3Config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1,
                };

                using (var s3Client = new AmazonS3Client(awsS3Config))
                {
                    var fileTransferUtility = new TransferUtility(s3Client);

                    foreach (var file in files)
                    {
                        string fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        CreateItem(fileName);
                        var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                        {
                            BucketName = AWSConstants.bucketName,
                            InputStream = file.OpenReadStream(),
                            Key = fileName, // Using the original file name as the key
                        };

                        fileTransferUtility.Upload(fileTransferUtilityRequest);
                    }
                }
            }
        }
    }
}

