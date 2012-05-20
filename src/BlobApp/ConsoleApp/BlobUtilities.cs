using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.StorageClient.Protocol;
using System.IO;

namespace ConsoleApp
{
    class BlobUtilities
    {
        private CloudStorageAccount _account;
        private CloudBlobClient _client;

        public BlobUtilities(string connectionString)
        {
            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudBlobClient();
        }

        internal bool ListContainers(out List<CloudBlobContainer> containers)
        {
            try
            {
                containers = _client.ListContainers().ToList();
                return true;
            }
            catch (StorageClientException)
            {
                containers = null;
                return false;
            }
        }

        internal bool CreateContainer(string containerName)
        {
            var container = _client.GetContainerReference(containerName);
            return container.CreateIfNotExist();
        }

        internal bool DeleteContainer(string containerName)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                container.Delete();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool ListBlobs(string containerName, out List<CloudBlob> blobs)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                blobs = container.ListBlobs().Cast<CloudBlob>().ToList();
                return true;
            }
            catch (StorageClientException)
            {
                blobs = null;
                return false;
            }
        }

        internal bool PutBlob(string containerName, string blobName, string textContent)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetBlobReference(blobName);
                blob.UploadText(textContent);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool PutBlob(string containerName, string blobName, int pageSize)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetPageBlobReference(blobName);
                blob.Create(pageSize);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool CopyBlob(string containerSrc, string fileSrc, string containerDest, string fileDest)
        {
            try
            {
                var containerSrcObj = _client.GetContainerReference(containerSrc);
                var containerDestObj = _client.GetContainerReference(containerDest);
                var blobSrc = containerSrcObj.GetBlobReference(fileSrc);
                var blobDest = containerDestObj.GetBlobReference(fileDest);
                blobDest.CopyFromBlob(blobSrc);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool SnapshotBlob(string containerName, string blobName)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetBlobReference(blobName);
                blob.CreateSnapshot();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool DeleteBlob(string containerName, string blobName)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetBlobReference(blobName);
                blob.Delete();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }


        internal bool PutBlock(string containerName, string blobName, int blockIndex, ref string[] blockIds, string blockContent)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blobBlock = container.GetBlockBlobReference(blobName);
                blockIds[blockIndex] = Convert.ToBase64String(BitConverter.GetBytes(blockIndex));
                using (var stream = new MemoryStream((new UTF8Encoding()).GetBytes(blockContent)))
                {
                    blobBlock.PutBlock(blockIds[blockIndex], stream, null);
                }
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool PutBlockList(string containerName, string blobName, string[] getblockids)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blobBlock = container.GetBlockBlobReference(blobName);
                blobBlock.PutBlockList(getblockids);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool GetBlockList(string containerName, string blobName, out string[] blockIds)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetBlockBlobReference(blobName);
                blockIds = blob.DownloadBlockList().Select(blockItem => blockItem.Name).ToArray();
                return true;
            }
            catch (StorageClientException)
            {
                blockIds = null;
                return false;
            }
        }

        internal bool PutPage(string containerName, string blobName, string pageContent, int offset)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blobPage = container.GetPageBlobReference(blobName);
                using (var stream = new MemoryStream((new UTF8Encoding()).GetBytes(pageContent)))
                {
                    blobPage.WritePages(stream, offset);
                }
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool GetPage(string containerName, string blobName, int offset, int size, out string pageContent)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blobPage = container.GetPageBlobReference(blobName);
                var bytes = new byte[size];
                using (var stream = blobPage.OpenRead())
                {
                    stream.Seek(offset, SeekOrigin.Begin);
                    stream.Read(bytes, 0, size);
                }
                pageContent = (new UTF8Encoding()).GetString(bytes);
                return true;
            }
            catch (StorageClientException)
            {
                pageContent = null;
                return false;
            }
        }

        internal bool GetPageRegions(string containerName, string blobName, out PageRange[] ranges)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blobPage = container.GetPageBlobReference(blobName);
                ranges = blobPage.GetPageRanges().ToArray();
                return true;
            }
            catch (StorageClientException)
            {
                ranges = null;
                return false;
            }
        }

        internal bool GetBlob(string containerName, string blobName, out string content)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetBlobReference(blobName);
                content = blob.DownloadText();
                return true;
            }
            catch (StorageClientException)
            {
                content = null;
                return false;
            }
        }

        internal bool SetBlobProperties(string containerName, string blobName, SortedList<string, string> properties)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetBlobReference(blobName);
                blob.Properties.ContentType = properties.Where((pair, val) => pair.Key == "ContentType").First().Value;
                blob.SetProperties();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool GetBlobProperties(string containerName, string blobName, out SortedList<string, string> properties)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                
                var blob = container.GetBlobReference(blobName);
                
                properties = new SortedList<string,string>();
                blob.FetchAttributes();
                properties.Add("ContentType", blob.Properties.ContentType);
             
                return true;
            }
            catch (StorageClientException)
            {
                properties = null;
                return false;
            }
        }

        internal bool GetContainerACL(string containerName, out string accessLevel)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                switch (container.GetPermissions().PublicAccess)
                {
                    case BlobContainerPublicAccessType.Container:
                        accessLevel = "container";
                        break;
                    case BlobContainerPublicAccessType.Blob:
                        accessLevel = "blob";
                        break;
                    case BlobContainerPublicAccessType.Off:
                        accessLevel = "private";
                        break;
                    default:
                        accessLevel = null;
                        return false;
                }
                return true;
            }
            catch (StorageClientException)
            {
                accessLevel = null;
                return false;
            }
        }

        internal bool SetContainerAccessPolicy(string containerName, SortedList<string, SharedAccessPolicy> policies)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                var permissions = new BlobContainerPermissions();
                foreach (var p in policies)
                {
                    permissions.SharedAccessPolicies.Add(p.Key, p.Value);
                }
                container.SetPermissions(permissions);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool GetContainerAccessPolicy(string containerName, out SortedList<string, SharedAccessPolicy> policies)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                policies = new SortedList<string,SharedAccessPolicy>();
                foreach(var p in container.GetPermissions().SharedAccessPolicies)
                {
                    policies.Add(p.Key, p.Value);
                }
                return true;
            }
            catch (StorageClientException)
            {
                policies = null;
                return false;
            }
        }

        internal bool GenerateSharedAccessSignature(string containerName, SharedAccessPolicy policy, out string signature)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                signature = container.GetSharedAccessSignature(policy);
                return true;
            }
            catch (StorageClientException)
            {
                signature = null;
                return false;
            }
        }

        internal bool GenerateSharedAccessSignature(string containerName, string policy, out string signature)
        {
            try
            {
                var container = _client.GetContainerReference(containerName);
                signature = container.GetSharedAccessSignature(new SharedAccessPolicy(), policy);
                return true;
            }
            catch (StorageClientException)
            {
                signature = null;
                return false;
            }
        }

        internal bool LeaseBlob(string containerName, string blobName, string leaseAction, ref string leaseId)
        {
            LeaseAction action;
            if (leaseAction == "acquire")
            {
                action = LeaseAction.Acquire;
            }
            else if (leaseAction == "release")
            {
                action = LeaseAction.Release;
            }
            else
            {
                throw new ArgumentOutOfRangeException("leaseAction");
            }

            try
            {
                var container = _client.GetContainerReference(containerName);
                var blob = container.GetBlobReference(blobName);
                var cred = blob.ServiceClient.Credentials;
                var sharedUri = cred.TransformUri(blob.Uri.ToString());
                var request = BlobRequest.Lease(new Uri(sharedUri), 10000, action, leaseId);
                cred.SignRequest(request);
                using (var response = request.GetResponse())
                {
                    leaseId = response.Headers["x-ms-lease-id"];
                }
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }
    }
}
