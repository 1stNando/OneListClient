using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleTables;

namespace OneListClient
{
    class Program
    {
        static async Task GetOneItem(string token, int id)
        {
            try
            {
                var client = new HttpClient();
                // Generate a URL specifically referencing the endpoint for getting a single
                // todo item and provide the id we were supplied
                var url = $"https://one-list-api.herokuapp.com/items/{id}?access_token={token}";
                var responseAsStream = await client.GetStreamAsync(url);
                // Supply that *stream of data* to a Deserialize that will interpret it as a *SINGLE* `Item`
                var item = await JsonSerializer.DeserializeAsync<Item>(responseAsStream);
                var table = new ConsoleTable("ID", "Description", "Created At", "Updated At", "Completed");
                // Add one row to our table
                table.AddRow(item.Id, item.Text, item.CreatedAt, item.UpdatedAt, item.CompletedStatus);
                // Write the table
                table.Write(Format.Minimal);
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("I could not find that item!");
            }
        }


        //This is part of the "Menu", but it is dependent on the specific "token" so we need it at the top of the program.
        static async Task ShowAllItems(string token)
        {
            var client = new HttpClient();

            //the await keyword is important part that lets us wait for the code. Fetching data from a server and receiving back a Stream!
            var responseBodyAsStream = await client.GetStreamAsync($"https://one-list-api.herokuapp.com/items?access_token={token}");

            //Describe the shape of the data (Object in JSON =>Item). This lives in the cloud.
            //                                               V          
            var items = await JsonSerializer.DeserializeAsync<List<Item>>(responseBodyAsStream);

            var table = new ConsoleTable("Description", "Created At", "Completed");

            //loop through all items on the list
            foreach (var item in items)
            {
                //Notice the critical importance of the order in which these rows are being created, they have to match the
                //order of the "var table" lay out.
                table.AddRow(item.Text, item.CreatedAt, item.CompletedStatus);
            }

            //Write the table out 
            table.Write();
        }

        static async Task AddOneItem(string token, Item newItem)
        {
            var client = new HttpClient();

            // Generate a URL specifically referencing the endpoint for adding a todo item
            var url = $"https://one-list-api.herokuapp.com/items?access_token={token}";

            // Take the `newItem` and serialize it into JSON
            var jsonBody = JsonSerializer.Serialize(newItem);

            // We turn this into a StringContent object and indicate we are using JSON
            // by ensuring there is a media type header of `application/json`
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send the POST request to the URL and supply the JSON body
            var response = await client.PostAsync(url, jsonBodyAsContent);

            // Get the response as a stream.
            var responseJson = await response.Content.ReadAsStreamAsync();

            // Supply that *stream of data* to a Deserialize that will interpret it as a *SINGLE* `Item`
            var item = await JsonSerializer.DeserializeAsync<Item>(responseJson);

            // Make a table to output our new item.
            var table = new ConsoleTable("ID", "Description", "Created At", "Updated At", "Completed");

            // Add one row to our table
            table.AddRow(item.Id, item.Text, item.CreatedAt, item.UpdatedAt, item.CompletedStatus);

            // Write the table
            table.Write(Format.Minimal);
        }

        //implement UpdateItem method here:
        static async Task UpdateOneItem(string token, int id, Item updatedItem)
        {
            var client = new HttpClient();

            // Generate a URL specifically referencing the endpoint for getting a single
            // todo item and provide the id we were supplied
            var url = $"https://one-list-api.herokuapp.com/items/{id}?access_token={token}";

            // Take the `newItem` and serialize it into JSON
            var jsonBody = JsonSerializer.Serialize(updatedItem);

            // We turn this into a StringContent object and indicate we are using JSON
            // by ensuring there is a media type header of `application/json`
            var jsonBodyAsContent = new StringContent(jsonBody);
            jsonBodyAsContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Send the PUT request to the URL and supply the JSON body
            var response = await client.PutAsync(url, jsonBodyAsContent);

            // Get the response as a stream.
            var responseJson = await response.Content.ReadAsStreamAsync();

            // Supply that *stream of data* to a Deserialize that will interpret it as a *SINGLE* `Item`
            var item = await JsonSerializer.DeserializeAsync<Item>(responseJson);

            // Make a table to output our new item.
            var table = new ConsoleTable("ID", "Description", "Created At", "Updated At", "Completed");

            // Add one row to our table
            table.AddRow(item.Id, item.Text, item.CreatedAt, item.UpdatedAt, item.CompletedStatus);

            // Write the table
            table.Write(Format.Minimal);
        }

        static async Task DeleteOneItem(string token, int id)
        {
            try
            {
                var client = new HttpClient();

                // Generate a URL specifically referencing the endpoint for getting a single
                // todo item and provide the id we were supplied
                var url = $"https://one-list-api.herokuapp.com/items/{id}?access_token={token}";

                await client.DeleteAsync(url);
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("I could not find that item!");
            }
        }

        //Notice the "async Task" we have started to need ahead of the Main() in order for 
        //the system be become synchronous with await.
        static async Task Main(string[] args)
        {
            //logic to deal with the event that the user tries to dotnet run without including
            //the specific {token}, otherwise known as the name of our list. 
            var token = "";
            if (args.Length == 0)
            {
                Console.Write("What list would you like? ");
                token = Console.ReadLine();
            }
            else
            {
                token = args[0];
            }



            var keepGoing = true;
            while (keepGoing)
            {
                Console.Clear();
                Console.Write("Get (A)ll to-do, (O)ne to-do, (C)reate new item, or (Q)uit: ");
                var choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "Q":
                        keepGoing = false;
                        break;

                    case "A":
                        //Note the need to include "await" keyword since we need user to tell us what list to use. 
                        await ShowAllItems(token);

                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;


                    //This allows user to fetch a specific item IN the todo list.
                    case "O":
                        Console.Write("Enter the ID of the item to show: ");
                        var id = int.Parse(Console.ReadLine());

                        //token, id are the things it is taking in
                        await GetOneItem(token, id);
                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;

                    case "C":
                        Console.Write("Enter the description of your new todo: ");
                        var text = Console.ReadLine();

                        var newItem = new Item
                        {
                            Text = text
                        };

                        await AddOneItem(token, newItem);

                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;

                    case "U":
                        Console.Write("Enter the ID of the item to update: ");
                        var existingId = int.Parse(Console.ReadLine());

                        Console.Write("Enter the new description: ");
                        var newText = Console.ReadLine();

                        Console.Write("Enter yes or no to indicate if the item is comoplete: ");
                        var newComplete = Console.ReadLine().ToLower() == "yes";

                        var updateItem = new Item
                        {
                            Text = newText,
                            Complete = newComplete
                        };

                        await UpdateOneItem(token, existingId, updateItem);

                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;

                    case "D":
                        Console.Write("Enter the ID of the item to delete: ");
                        var idToDelete = int.Parse(Console.ReadLine());

                        await DeleteOneITem(token, idToDelete);

                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;

                    //set default value
                    default:
                        break;
                }
            }
        }
    }
}
