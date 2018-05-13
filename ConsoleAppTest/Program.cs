using IdentityModel.Client;
using System;
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
            DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:1002");
            if(disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, "client_imgapp", "secret");
            TokenResponse tokenResponse = await tokenClient.RequestClientCredentialsAsync("api_img");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
        }
    }
}
