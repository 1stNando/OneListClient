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
        static async Task GetOneItem(string token, int id)
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
                Console.Write("Get (A)ll to-do, (O)ne to-do, or (Q)uit: ");
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

                    //set default value
                    default:
                        break;
                }
            }
        }
    }
}
