using Hypesquadify.Enums;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Hypesquadify
{
    public class Program
    {
        private const string Version = "1.1";

        private static void Intro()
        {
            var headerStr = $"Hypesquadify v{Version}";

            // this is a very odd method of doing this but fuck it we ball
            Console.WriteLine(new string('=', headerStr.Length * 2));
            Console.WriteLine(new string(' ', headerStr.Length / 2) + headerStr);
            Console.WriteLine(new string('=', headerStr.Length * 2));

            Console.WriteLine("Get any HypeSquad badge!");
            Console.WriteLine();
        }

        private static void WriteError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static async Task<int> Main(string[] args)
        {
            Console.Title = $"Hypesquadify {Version}";

            Intro();

            Console.WriteLine("You need to provide your Discord token in order to authenticate with Discord's API.");
            Console.WriteLine("Otherwise you'll just get a 403 Unauthorized error.");

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
                WriteError("That's not a valid house.");
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

            Console.WriteLine($"Performing POST request to {requestUri}...");
            var res = await http.PostAsync(requestUri, json);

            if (res.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Welcome to the squad! :)");
                Console.ResetColor();

                return 0;
            }
            else
            {
                switch (res.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        WriteError("401 Unauthorized - Probably invalid token");
                        break;

                    case HttpStatusCode.TooManyRequests:
                        WriteError("429 Too Many Requests - Rate limit, please wait a few moments");
                        break;

                    case HttpStatusCode.Forbidden:
                        WriteError("403 Forbidden - Discord hates you and didn't wanna fulfill the request");
                        break;

                    default:
                        var resContent = await res.Content.ReadAsStringAsync();
                        WriteError($"{(int)res.StatusCode} {res.ReasonPhrase}\n{resContent}");
                        break;
                }

                return 1;
            }
        }
    }
}
