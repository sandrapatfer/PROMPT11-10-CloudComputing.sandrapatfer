using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace ConsoleApp
{
    internal class Tests
    {
        private const string CONNECTION_STRING = "DefaultEndpointsProtocol=http;AccountName=storagespf;AccountKey=0FsVToOeZ1tDGhxuESR8oZr+BUNtYxk7Uvffmd9U2g24hsJ9e5qj95WyT/OmIb7AXdfjrlSvPv7PUDYXgiVdHA==";

        static void Separator()
        {
            Console.WriteLine("-------------------------------");
        }

        public static void TestBlobStorage1()
        {
            // Constructor - pass in a storage connection string.
            BlobUtilities blobUtilities = new
                BlobUtilities(CONNECTION_STRING);
            try
            {
                Separator();
                List<CloudBlobContainer> containers;
                Console.Write("List Blob containers ");
                // Enumerate the containers in a storage account.
                // Return true on success, false if already exists, throw exception on error
                if (blobUtilities.ListContainers(out containers))
                {
                    Console.WriteLine("true");
                    if (containers != null)
                    {
                        foreach (CloudBlobContainer container in containers)
                            Console.Write(container.Name + " ");
                        Console.WriteLine();
                    }
                }
                else
                    Console.WriteLine("false");
                Separator();
                // Create a blob container.
                // Return true on success, false if already exists, throw exception on error.
                Console.Write("Create container ");
                if (blobUtilities.CreateContainer("samplecontainer1"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Delete container ");
                if (blobUtilities.DeleteContainer("samplecontainer0"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION " + ex.ToString());
            }
        }

        public static void TestBlobStorage2()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                Console.Write("Copy blob ");
                if (blobUtilities.CopyBlob("samplecontainer1", "blob1.txt", "samplecontainer1", "blob2.txt"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Snapshot blob ");
                if (blobUtilities.SnapshotBlob("samplecontainer1", "blob1.txt"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Delete blob ");
                if (blobUtilities.DeleteBlob("samplecontainer1", "blob2.txt"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex) { Console.WriteLine("EXCEPTION " + ex.ToString()); }
        }

        public static void TestBlobStorage3()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                List<CloudBlob> blobs;
                Console.Write("List blobs ");
                if (blobUtilities.ListBlobs("samplecontainer1", out blobs))
                {
                    Console.WriteLine("true");
                    if (blobs != null)
                    {
                        foreach (CloudBlob blob in blobs)
                        {
                            Console.WriteLine(blob.Uri.LocalPath);
                        }
                    }
                }
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Put blob ");
                if (blobUtilities.PutBlob("samplecontainer1", "blob1.txt", "This is a text blob!"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Put page blob ");
                if (blobUtilities.PutBlob("samplecontainer1", "pageblob1.txt", 2048))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex) { Console.WriteLine("EXCEPTION " + ex.ToString()); }
        }

        public static void TestBlobStorage4()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                string[] blockIds = new string[3];
                Console.Write("Put block 0 ");
                if (blobUtilities.PutBlock("samplecontainer1", "largeblob2.txt", 0, ref blockIds, "AAAAAAAAAA"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Put block 1 ");
                if (blobUtilities.PutBlock("samplecontainer1", "largeblob2.txt", 1, ref blockIds, "BBBBBBBBBB"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Put block 2 ");
                if (blobUtilities.PutBlock("samplecontainer1", "largeblob2.txt", 2, ref blockIds, "CCCCCCCCCC"))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");

                Separator();
                Console.Write("Put block list ");
                if (blobUtilities.PutBlockList("samplecontainer1", "largeblob2.txt", blockIds))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");

                Separator();
                string[] getblockids = null;
                Console.Write("Get block list ");
                if (blobUtilities.GetBlockList("samplecontainer1", "largeblob2.txt", out getblockids))
                {
                    Console.WriteLine("true");
                    if (getblockids != null)
                    {
                        for (int i = 0; i < getblockids.Length; i++)
                        {
                            Console.WriteLine("Block " + i + " id = " + getblockids[i]);
                        }
                    }
                }
                else
                    Console.WriteLine("false");
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION " + ex.ToString());
            }
        }

        public static void TestBlobStorage5()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                string page0 = new string('A', 512);
                Console.Write("Put page 0 ");
                if (blobUtilities.PutPage("samplecontainer1", "pageblob1.txt", page0, 0))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                string page1 = new string('B', 512);
                Console.Write("Put page 1 ");
                if (blobUtilities.PutPage("samplecontainer1", "pageblob1.txt", page1, 512))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");

                Separator();
                page0 = String.Empty;
                Console.Write("Get page 0 ");
                if (blobUtilities.GetPage("samplecontainer1", "pageblob1.txt", 0, 512, out page0))
                {
                    Console.WriteLine("true");
                    Console.WriteLine(page0);
                }
                else
                    Console.WriteLine("false");
                Separator();
                page1 = String.Empty;
                Console.Write("Get page 1 ");
                if (blobUtilities.GetPage("samplecontainer1", "pageblob1.txt", 512, 512, out page1))
                {
                    Console.WriteLine("true");
                    Console.WriteLine(page1);
                }
                else
                    Console.WriteLine("false");
                Separator();
                PageRange[] ranges;
                Console.Write("Get page regions ");
                if (blobUtilities.GetPageRegions("samplecontainer1", "pageblob1.txt", out ranges))
                {
                    Console.WriteLine("true");
                    if (ranges != null)
                    {
                        foreach (PageRange range in ranges)
                        {
                            Console.WriteLine(range.StartOffset.ToString() + "-" + range.EndOffset.ToString());
                        }
                    }
                }
                else
                    Console.WriteLine("false");
                Separator();
                string content;
                Console.Write("Get blob ");
                if (blobUtilities.GetBlob("samplecontainer1", "blob1.txt", out content))
                {
                    Console.WriteLine("true");
                    Console.WriteLine(content);
                }
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex) { Console.WriteLine("EXCEPTION " + ex.ToString()); }
        }

        public static void TestBlobStorage6()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                SortedList<string, string> properties = new SortedList<string, string>();
                properties.Add("ContentType", "text/html");
                Console.Write("Set blob properties ");
                if (blobUtilities.SetBlobProperties("samplecontainer1", "blob1.txt", properties))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Get blob properties ");
                if (blobUtilities.GetBlobProperties("samplecontainer1", "blob1.txt", out properties))
                {
                    Console.WriteLine("true");
                    foreach (KeyValuePair<string, string> item in properties)
                    {
                        Console.WriteLine(item.Key + ": " + item.Value);
                    }
                }
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex) { Console.WriteLine("EXCEPTION " + ex.ToString()); }
        }

        public static void TestBlobStorage7()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                string accessLevel;
                // Get container access control.Return true on success, false if not found, throw exception on
                //error. Access level set to container|blob|private.
                Console.Write("Get container ACL ");
                if (blobUtilities.GetContainerACL("samplecontainer1", out accessLevel))
                    Console.WriteLine("true " + accessLevel);
                else
                    Console.WriteLine("false");
                Separator();
                SortedList<string, SharedAccessPolicy> policies = new SortedList<string, SharedAccessPolicy>();
                SharedAccessPolicy policy1 = new SharedAccessPolicy()
                {
                    Permissions = SharedAccessPermissions.List | SharedAccessPermissions.Read |
                    SharedAccessPermissions.Write | SharedAccessPermissions.Delete,
                    SharedAccessStartTime = DateTime.UtcNow,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1)
                };
                policies.Add("Policy1", policy1);
                policies.Add("Policy2", new SharedAccessPolicy()
                {
                    Permissions = SharedAccessPermissions.Read,
                    SharedAccessStartTime = DateTime.Parse("2010-01-01T09:38:05Z"),
                    SharedAccessExpiryTime = DateTime.Parse("2012-12-31T09:38:05Z")
                });
                Console.Write("Set container access policy ");
                if (blobUtilities.SetContainerAccessPolicy("samplecontainer1", policies))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
                Console.Write("Get container access policy ");
                if (blobUtilities.GetContainerAccessPolicy("samplecontainer1", out policies))
                {
                    Console.WriteLine("true");
                    if (policies != null)
                    {
                        foreach (KeyValuePair<string, SharedAccessPolicy> policy in policies)
                        {
                            Console.WriteLine("Policy " + policy.Key);
                        }
                    }
                }
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION " + ex.ToString());
            }
        }

        public static void TestBlobStorage8()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                string signature = String.Empty;
                SharedAccessPolicy policy1 = new SharedAccessPolicy()
                {
                    Permissions = SharedAccessPermissions.List | SharedAccessPermissions.Read |
                    SharedAccessPermissions.Write | SharedAccessPermissions.Delete,
                    SharedAccessStartTime = DateTime.UtcNow,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1)
                };
                Console.Write("Create shared access signature ");
                if (blobUtilities.GenerateSharedAccessSignature("samplecontainer1", policy1, out signature))
                    Console.WriteLine("true " + signature);
                else
                    Console.WriteLine("false");
                Separator();
                signature = String.Empty;
                Console.Write("Create shared access signature from access policy ");
                if (blobUtilities.GenerateSharedAccessSignature("samplecontainer1", "Policy1", out signature))
                    Console.WriteLine("true " + signature);
                else
                    Console.WriteLine("false");
                Separator();
                signature = String.Empty;
                Console.Write("Create shared access signature from access policy 2 ");
                if (blobUtilities.GenerateSharedAccessSignature("samplecontainer1", "Policy2", out signature))
                    Console.WriteLine("true " + signature);
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION " + ex.ToString());
            }
        }

        public static void TestBlobStorage9()
        {
            BlobUtilities blobUtilities = new BlobUtilities(CONNECTION_STRING);
            try
            {
                string leaseAction = "acquire";
                string leaseId = null;
                Console.Write("Lease blob - acquire ");
                if (blobUtilities.LeaseBlob("samplecontainer1", "blob1.txt", leaseAction, ref leaseId))
                    Console.WriteLine("true " + leaseId);
                else
                    Console.WriteLine("false");
                Separator();
                leaseAction = "release";
                Console.Write("Lease blob - release ");
                if (blobUtilities.LeaseBlob("samplecontainer1", "blob1.txt", leaseAction, ref leaseId))
                    Console.WriteLine("true");
                else
                    Console.WriteLine("false");
                Separator();
            }
            catch (Exception ex) { Console.WriteLine("EXCEPTION " + ex.ToString()); }
        }
    }
}