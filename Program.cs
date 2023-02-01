﻿using System;
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

            //the await keyword is important part that lets us wait for the code. Fetching data from a server and receiving back a Stream!
            var responseBodyAsStream = await client.GetStreamAsync("https://one-list-api.herokuapp.com/items?access_token=fernando");

            //                                  Describe the shape of the data (array in JSON => List, Object in JSON =>Item). This lives in the cloud.
            //                                               V          V
            var items = await JsonSerializer.DeserializeAsync<List<Item>>(responseBodyAsStream);

            foreach (var item in items)
            {
                Console.WriteLine($"The task {item.Text} was created on {item.CreatedAt} and has completion of {item.Complete}");
            }
        }
    }
}
