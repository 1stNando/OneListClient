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
        //This is part of the "Menu", but it is dependent on the specific "token" so we need it at the top of the program.
        static async Task ShowAllItems(string token)
        {
            var client = new HttpClient();

            //the await keyword is important part that lets us wait for the code. Fetching data from a server and receiving back a Stream!
            var responseBodyAsStream = await client.GetStreamAsync($"https://one-list-api.herokuapp.com/items?access_token={token}");

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
                Console.Write("Get (A)ll to-do, or (Q)uit: ");
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

                    //set default value
                    default:
                        break;
                }
            }
        }
    }
}
