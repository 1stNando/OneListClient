using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleTables;

namespace OneListClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();

            //the await keyword is important part that lets us wait for the code. Fetching data from a server and receiving back a Stream!
            var responseBodyAsStream = await client.GetStreamAsync("https://one-list-api.herokuapp.com/items?access_token=fernando");

            //                                  Describe the shape of the data (array in JSON => List, Object in JSON =>Item). This lives in the cloud.
            //                                               V          V
            var items = await JsonSerializer.DeserializeAsync<List<Item>>(responseBodyAsStream);

            var table = new ConsoleTable("Description", "Created At", "Completed");

            foreach (var item in items)
            {
                //Notice the critical importance of the order in which these rows are being created, they have to match the
                //order of the "var table" lay out.
                table.AddRow(item.Text, item.CreatedAt, item.CompletedStatus);
            }

            //Write the table
            table.Write();
        }
    }
}
