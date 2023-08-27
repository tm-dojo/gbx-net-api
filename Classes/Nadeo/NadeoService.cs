using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;

public static class NadeoService
{
    public static NadeoAccount nadeoAccount;
    public static NadeoSession nadeoSession;
    public static NadeoToken nadeoToken;

    public static void Init(NadeoAccount account)
    {
        nadeoAccount = account;
        GetNadeoToken();
    }

    static async void GetNadeoToken()
    {
        var plainTextBytes = Encoding.UTF8.GetBytes($"{nadeoAccount.email}:{nadeoAccount.password}");

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"TMDojo / {nadeoAccount.email}");

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://public-ubiservices.ubi.com/v3/profiles/sessions"))
            {
                requestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");

                requestMessage.Headers.Add("Ubi-AppId", "86263886-327a-4328-ac69-527f0d20a237");
                requestMessage.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(plainTextBytes)}");

                var response = await httpClient.SendAsync(requestMessage);
                var jsonContent = await response.Content.ReadAsStringAsync();

                nadeoSession = JsonSerializer.Deserialize<NadeoSession>(jsonContent);
            }

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://prod.trackmania.core.nadeo.online/v2/authentication/token/ubiservices"))
            {
                requestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");

                requestMessage.Headers.Add("Authorization", $"ubi_v1 t={nadeoSession.Ticket}");

                var response = await httpClient.SendAsync(requestMessage);
                var jsonContent = await response.Content.ReadAsStringAsync();

                nadeoToken = JsonSerializer.Deserialize<NadeoToken>(jsonContent);
            }
        }
    }

    static async Task<NadeoMapInfos> GetNadeoMapInfos(string mapUid)
    {
        string mapInfosUrl = $"https://prod.trackmania.core.nadeo.online/maps/?mapUidList={mapUid}";

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, mapInfosUrl))
            {
                requestMessage.Content = new StringContent("", Encoding.UTF8, "application/json");
                //requestMessage.Headers.Add("Authorization", nadeoToken.);
                requestMessage.Headers.Add("Authorization", $"nadeo_v1 t={nadeoToken.AccessToken}");

                var response = await httpClient.SendAsync(requestMessage);
                var jsonContent = await response.Content.ReadAsStringAsync();

                List<NadeoMapInfos> nadeoMap = JsonSerializer.Deserialize<List<NadeoMapInfos>>(jsonContent);

                return nadeoMap[0];
            }
        }
    }

    public static async Task<MemoryStream> GetMemoryStreamFromUrlAsync(string fileUrl)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(fileUrl))
                {
                    response.EnsureSuccessStatusCode();

                    var contentBytes = await response.Content.ReadAsByteArrayAsync();
                    return new MemoryStream(contentBytes);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public static async Task<Stream> GetFileStreamFromUrlAsync(string fileUrl)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                // Send a GET request to the file URL and get the response
                using (var response = await httpClient.GetAsync(fileUrl))
                {
                    response.EnsureSuccessStatusCode(); // Ensure the request was successful

                    // Get the content as a stream
                    return await response.Content.ReadAsStreamAsync();
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during the process
            // For example, handle HttpClientException, ArgumentException, etc.
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public static async Task<Stream> GetMapFileStreamFromUrlAsync(string mapUid)
    {
        NadeoMapInfos mapInfos = await GetNadeoMapInfos(mapUid);
        Stream mapFileStream = await GetMemoryStreamFromUrlAsync(mapInfos.FileUrl.AbsoluteUri);

        return mapFileStream;
    }
}