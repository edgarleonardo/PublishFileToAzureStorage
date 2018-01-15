using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PublishDataToAzureStorage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpFileCollectionBase fileUpload)
        {
            string path = Server.MapPath("~/");
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var extension = Path.GetExtension(file.FileName).Replace(".", "");
                var filename = Path.GetFileName(file.FileName);
                path += filename;
                file.SaveAs(path);
                UploadFile(filename, path);
            }
            return View();
        }
        private void UploadFile(string filename, string localFile)
        {
            var connStr = CloudConfigurationManager.GetSetting("StorageConnectionString");
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connStr);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["StorageContainer"]);

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromFile(localFile);
            blockBlob.Properties.CacheControl = "public, max-age=1";
        }
    }
}