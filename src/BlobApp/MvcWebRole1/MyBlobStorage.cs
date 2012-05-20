using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace MvcWebRole1
{
    public class MyBlobStorage
    {
        private CloudBlobContainer _imagesContainer;

        public MyBlobStorage()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
            var client = account.CreateCloudBlobClient();
            _imagesContainer = client.GetContainerReference("images");
        }

        public CloudBlobContainer GetImagesContainer()
        {
            return _imagesContainer;
        }
    }
}