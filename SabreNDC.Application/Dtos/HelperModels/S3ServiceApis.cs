using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Dtos.HelperModels;

public class S3ServiceApis
{
    private readonly IAmazonS3 client;
    public S3ServiceApis(string key, string sectet)
    {
        client = new AmazonS3Client(key, sectet, RegionEndpoint.APSoutheast1);
    }
    //public S3ServiceApis(IAmazonS3 _client)
    //{
    //    this.client = _client;
    //}

    public async Task<bool> CopyObjectInBucketAsync(string bucketName, string objectName, string folderName)
    {
        try
        {
            var request = new CopyObjectRequest
            {
                SourceBucket = bucketName,
                SourceKey = objectName,
                DestinationBucket = bucketName,
                DestinationKey = $"{folderName}\\{objectName}",
            };
            var response = await client.CopyObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error copying object: '{ex.Message}'");
            return false;
        }
    }

    public async Task<(bool, string)> CreateBucketAsync(string bucketName)
    {
        try
        {
            if (await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName) == false)
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true,
                };

                var response = await client.PutBucketAsync(request);
                return (response.HttpStatusCode == System.Net.HttpStatusCode.OK, "Success");
            }
            else { return (false, $"This {bucketName} bucket already exists"); }
        }
        catch (AmazonS3Exception ex)
        {
            //Console.WriteLine($"Error creating bucket: '{ex.Message}'");
            return (false, $"Error creating bucket: '{ex.Message}'");
        }
    }
    public async Task<dynamic> GetAllLogFilesFromS3(string bucketName, List<string> fileNames)
    {
        try
        {
            List<Dictionary<string, string>> fileNameValue = new List<Dictionary<string, string>>();
            foreach (var files in fileNames)
            {

                var request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    Prefix = files,
                };

                var response = await client.ListObjectsAsync(request);
                foreach (var obj in response.S3Objects)
                {
                    var getObjectRequest = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = obj.Key
                    };

                    using (var getObjectResponse = await client.GetObjectAsync(getObjectRequest))
                    using (var streamReader = new StreamReader(getObjectResponse.ResponseStream))
                    {
                        var contents = await streamReader.ReadToEndAsync();
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(obj.Key, contents.ToString());
                        fileNameValue.Add(keyValues);
                    }
                }
            }
            return fileNameValue;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<bool> DeleteBucketAsync(string bucketName)
    {
        var request = new DeleteBucketRequest
        {
            BucketName = bucketName,
        };

        var response = await client.DeleteBucketAsync(request);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<bool> DeleteBucketContentsAsync(string bucketName)
    {
        // Iterate over the contents of the bucket and delete all objects.
        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
        };

        try
        {
            var response = await client.ListObjectsV2Async(request);

            do
            {
                response.S3Objects
                    .ForEach(async obj => await client.DeleteObjectAsync(bucketName, obj.Key));

                // If the response is truncated, set the request ContinuationToken
                // from the NextContinuationToken property of the response.
                request.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);

            return true;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error deleting objects: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DownloadObjectFromBucketAsync(string bucketName, string objectName, string filePath)
    {
        // Create a GetObject request
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
        };
        string responseBody = string.Empty;
        string contenType = string.Empty;
        try
        {
            // Issue request and remember to dispose of the response
            using GetObjectResponse response = await client.GetObjectAsync(request);

            //Save object to local file
            await response.WriteResponseStreamToFileAsync($"{filePath}\\{objectName}", true, System.Threading.CancellationToken.None);
            return (response.HttpStatusCode == System.Net.HttpStatusCode.OK);
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error saving {objectName}: {ex.Message}");
            return (false);
        }
    }

    public async Task<(bool, string, string)> ReadObjectFromBucketAsync(string bucketName, string objectName, string filePath)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
        };
        string responseBody = string.Empty;
        string contenType = string.Empty;
        try
        {

            using GetObjectResponse response = await client.GetObjectAsync(request);
            using (var responseStream = response.ResponseStream)
            {
                var title = response.Metadata["x-amz-meta-title"];
                contenType = response.Headers["Content-Type"];

                if (contenType.Contains("image"))
                {
                    var stream = new MemoryStream();
                    responseStream.CopyToAsync(stream).Wait();
                    stream.Position = 0;
                    var inputAsString = Convert.ToBase64String(stream.ToArray());
                    responseBody = inputAsString;
                }
                else
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }

            }

            return (response.HttpStatusCode == System.Net.HttpStatusCode.OK, responseBody, contenType);
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error saving {objectName}: {ex.Message}");
            return (false, string.Empty, ex.Message);
        }
    }

    public async Task<bool> ListBucketContentsAsync(string bucketName)
    {
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                MaxKeys = 5,
            };

            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Listing the contents of {bucketName}:");
            Console.WriteLine("--------------------------------------");

            var response = new ListObjectsV2Response();

            do
            {
                response = await client.ListObjectsV2Async(request);

                response.S3Objects
                    .ForEach(obj => Console.WriteLine($"{obj.Key,-35}{obj.LastModified.ToShortDateString(),10}{obj.Size,10}"));

                // If the response is truncated, set the request ContinuationToken
                // from the NextContinuationToken property of the response.
                request.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);

            return true;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
            return false;
        }
    }

    public async Task<bool> UploadFileAsync(string bucketName, string objectName, string filePath)
    {
        //if (Convert.ToBoolean(_configuration["awsS3:IsLive"]))
        //{

        if (!(await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName)))
        {
            var ok = await CreateBucketAsync(bucketName);
            if (!ok.Item1) return false;
        }
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
            FilePath = filePath,

        };

        var response = await client.PutObjectAsync(request);
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}.");
            return true;
        }
        else
        {
            Console.WriteLine($"Could not upload {objectName} to {bucketName}.");
            return false;
        }
        //}
        //else
        //{
        //    return false;
        //}
    }
}
