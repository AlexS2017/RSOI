using Common.ServiceMessages;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            HttpClient client1 = new HttpClient();
            string url1 = "http://localhost:1002/api/stat/getallstat";

            string clientName = "client_imgapp_internal";
            string secret = "secret123";
            string api = "api_img_internal";

            TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, clientName, secret);

            TokenResponse tokenResponse = await tokenClient.RequestClientCredentialsAsync(api);            

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            else
            {
                Console.WriteLine(tokenResponse.Json);

                client1.SetBearerToken(tokenResponse.AccessToken);

                var response1 = await client1.GetAsync($"{url1}");
                if (!response1.IsSuccessStatusCode)
                {
                    Console.WriteLine(response1.StatusCode);
                }
                else
                {
                    var respcontent1 = await response1.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(respcontent1));
                }
            }

        }

        public static async Task MainAsync1()
        {
            DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            if (false)
            {
                string clientName = "client_imgapp";
                string secret = "secret";
                string api = "api_img";
                TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, clientName, secret);
                TokenResponse tokenResponse = null;

                if (clientName != "client_imgapp_internal")
                {
                    tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("q@q.com", "111111", "api_img");
                }
                else
                {
                    tokenResponse = await tokenClient.RequestClientCredentialsAsync(api);
                }

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }

                Console.WriteLine(tokenResponse.Json);
                Console.WriteLine("\n\n");

                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);
            }

            LoginRequest login = new LoginRequest()
            {
                appsecret = "secret",
                clientid = "client_imgapp",
                login = "q@q.com",
                password = "111111"
            };

            string url = "http://localhost:1001/api/identity/login";
            string url1 = "http://localhost:1001/api/PhotoMsg/getcomments";

            string jsonToPost = JsonConvert.SerializeObject(login);
            HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");

            HttpClient client1 = new HttpClient();
            var response = await client1.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var respcontent = await response.Content.ReadAsStringAsync();
                LoginResponse resp = JsonConvert.DeserializeObject<LoginResponse>(respcontent);
                Console.WriteLine(respcontent);

                client1.SetBearerToken(resp.Data.access_token);

                var response1 = await client1.GetAsync($"{url1}/847505e3-8d2d-47c5-85d6-294ccf5ffa20");
                if (!response1.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var respcontent1 = await response1.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(respcontent1));
                }
            }

        }
    }
}
