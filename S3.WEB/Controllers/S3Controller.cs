using Amazon.S3.Transfer;
using Amazon.S3;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using S3.WEB.Constants;
using S3.WEB.Models;
using S3.Infrastructure;
using Amazon.S3.Model;
using Microsoft.Net.Http.Headers;

namespace S3.WEB.Controllers
{
    public class S3Controller : Controller
    {
        ILifetimeScope _scope;
        ILogger<S3Controller> _logger;
        public S3Controller(ILifetimeScope scope, ILogger<S3Controller> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var model = _scope.Resolve<S3BucketItemListModel>();

            return View(model);
        }

        [HttpGet]
        public IActionResult UploadFileToS3()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFileToS3(List<IFormFile> files, S3BucketItemCreateModel model)
        {
            model.ResolveDependency(_scope);

            // Upload files to S3 using the model
            model.UploadFilesToS3(files);

            return RedirectToAction("Index");
        }


        public IActionResult DownloadSpecificFile(Guid id)
        {
            var model = _scope.Resolve<S3BucketItemListModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = model.GetFileName(id);
                    var fileStream = model.GetFileAsStreamFromS3(fileName);

                    // Determine the content type based on the file extension
                    string contentType = GetContentType(fileName);

                    // Set the Content-Disposition header to specify the file name
                    var contentDisposition = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName,
                    };
                    Response.Headers.Add(HeaderNames.ContentDisposition, contentDisposition.ToString());

                    // Return the file stream with the correct content type
                    return File(fileStream, contentType);
                }
                catch (AmazonS3Exception ex)
                {
                    _logger.LogError(ex, "Amazon S3 Exception");
                    return Content("File download failed: Amazon S3 Exception");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Server Error");
                    return Content("File download failed: Server Error");
                }
            }

            return RedirectToAction("Index");
        }

        private string GetContentType(string fileName)
        {
           
            //Here MimeMapping helps us to determine the content type by the extension of files.
            return MimeMapping.MimeUtility.GetMimeMapping(fileName);
        }




        public IActionResult Delete(Guid id)
        {
            var model = _scope.Resolve<S3BucketItemListModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = model.GetFileName(id);
                    model.DeleteFileFromS3(fileName,id);
                }
                catch (AmazonS3Exception ex)
                {
                   
                    _logger.LogError(ex, "Amazon S3 Exception");
                    return Content("File deletion failed: Amazon S3 Exception");
                }
                catch (Exception e)
                {
                  
                    _logger.LogError(e, "Server Error");
                    return Content("File deletion failed: Server Error");
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<JsonResult> GetS3BucketItems()
        {
            var dataTablesModel = new DataTablesAjaxRequestUtility(Request);
            var model = _scope.Resolve<S3BucketItemListModel>();

            var data = await model.GetS3BucketItemsAsync(dataTablesModel);
            return Json(data);
        }
    }
}
