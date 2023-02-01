using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            //the await keyword is important part that lets us wait for the code. Fetching data from a server and receiving back a string.
            var responseBodyAsStream = await client.GetStreamAsync("https://one-list-api.herokuapp.com/items?access_token=fernando");

            var items = JsonSerializer.DeserializeAsync<List<Item>>(responseBodyAsStream);
        }
    }
}
