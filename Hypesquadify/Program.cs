using Hypesquadify.Enums;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Hypesquadify
{
    public class Program
    {
        private const string Version = "1.0";

        private static void Intro()
        {
            Console.WriteLine($"Hypesquadify v{Version}");
            Console.WriteLine("Allows you to get the HypeSquad badges.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }

        private static void WriteError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static async Task<int> Main(string[] args)
        {
            Console.Title = "Hypesquadify";

            Intro();
            Console.Clear();

            Console.WriteLine("You need to provide your Discord token in order to authenticate with Discord's API properly.");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Don't worry, I won't share it with anyone.");
            Console.ResetColor();

            Console.WriteLine();

            Console.Write("Discord Token: ");
            var token = Console.ReadLine();

            if (string.IsNullOrEmpty(token))
            {
                WriteError("Please don't leave it empty!");
                return 1;
            }

            Console.Clear();
            var values = Enum.GetValues<House>();

            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {values[i]}");
            }

            Console.Write("House: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var index))
            {
                WriteError("That's not a number.");
                return 1;
            }

            if (!Enum.IsDefined(typeof(House), index))
            {
                WriteError("That's not a house.");
                return 1;
            }

            var requestUri = "https://discord.com/api/v9/hypesquad/online";
            var content = new
            {
                house_id = index
            };

            var json = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            using var http = new HttpClient();

            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("Authorization", token);

            var res = await http.PostAsync(requestUri, json);

            if (res.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Welcome to the house!");
                Console.ResetColor();

                return 0;
            }
            else
            {
                var responseContent = await res.Content.ReadAsStringAsync();

                WriteError(responseContent);

                return 1;
            }
        }
    }
}
