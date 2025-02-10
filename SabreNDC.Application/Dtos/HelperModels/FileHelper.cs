using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabreNDC.Application.Dtos.HelperModels;

public abstract class FileHelper
{
    private static readonly string bucketName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("awsS3")["BucketName"];
    public static void ToWriteJson(string fileName, string folderName, string jsonString)
    {
        string path = Environment.CurrentDirectory + "/" + folderName + "/" + fileName + ".json";

        if (!Directory.Exists(path))
        {
            string dirPath = Path.GetDirectoryName(path);
            if (dirPath == null) throw new InvalidOperationException("Failure to save local security settings");
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        }
        File.WriteAllText(path, jsonString, Encoding.ASCII);
        if (Convert.ToBoolean(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("awsS3")["IsLive"]))
        {
            S3ServiceApis s3Services = new S3ServiceApis(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("awsS3")["AccessKeyId"], new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("awsS3")["SecretAccessKey"]);

            if (s3Services.UploadFileAsync(bucketName, $"{folderName}/{fileName}.json", path).Result == true)
            {
                File.Delete(path);
            }
        }
    }
    public static string ToReadJson(string fileName)
    {
        try
        {

            S3ServiceApis s3Services = new S3ServiceApis(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("awsS3")["AccessKeyId"], new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("awsS3")["SecretAccessKey"]);

            var s3Result = Convert.ToBoolean(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("awsS3")["IsLive"]) ?
            s3Services.ReadObjectFromBucketAsync(bucketName, $"{fileName}.json", "").Result :
                (false, string.Empty, string.Empty);
            if (s3Result.Item1)
            {
                return s3Result.Item2;
            }
            else
            {
                string path = Environment.CurrentDirectory + "/" + fileName + ".json";
                return File.ReadAllText(path);
            }
        }
        catch (Exception ex)
        {
            return string.Empty;
        }

    }
    public static string ToReadJson(string fileName, string folderName)
    {
        return ToReadJson(folderName + "/" + fileName);

    }
}
