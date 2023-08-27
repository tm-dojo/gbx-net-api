using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using GbxNetApi.Classes;
using System.Text.Json;

public static class S3
{
    private static S3Credentials credentials;
    private static IAmazonS3 client;

    public static void Init(S3Credentials s3Credentials)
    {
        credentials = s3Credentials;

        BasicAWSCredentials credentials1 = new BasicAWSCredentials(s3Credentials.accessKeyId, s3Credentials.secretAccessKey);
        client = new AmazonS3Client(credentials1, RegionEndpoint.EUCentral1);

    }

    public static async Task<MapBlocksData> GetMapBlockFromS3(string mapUid)
    {
        HttpClient client = new HttpClient();
        string mapBlocksUrl = $"{credentials.mapBlocksJsonCdnUrl}/map-blocks/{mapUid}.json";

        try
        {
            using HttpResponseMessage response = await client.GetAsync(mapBlocksUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            MapBlocksData blockData = JsonSerializer.Deserialize<MapBlocksData>(responseBody);

            return blockData;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
        return null;
    }

    public static async Task<PutObjectResponse> UploadBlocksJson(MapBlocksData mapBlocksData, string mapUid)
    {
        var putRequest1 = new PutObjectRequest
        {
            BucketName = credentials.bucketName,
            Key = $"map-blocks/{mapUid}.json",
            ContentBody = JsonSerializer.Serialize(mapBlocksData)
        };

        PutObjectResponse putObjectResponse = await client.PutObjectAsync(putRequest1);

        return putObjectResponse;
    }
}