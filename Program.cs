using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            //the await keyword is important part that lets us wait for the code. Fetching data from a server and receiving back a string.
            var responseBodyAsString = await client.GetStringAsync("https://one-list-api.herokuapp.com/items?access_token=fernando");

            Console.WriteLine(responseBodyAsString);
        }
    }
}
