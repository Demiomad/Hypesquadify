using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using Spectre.Console;
using Hypesquadify.Enums;
using Hypesquadify.Discord;

namespace Hypesquadify
{
    public class Program
    {
        public static async Task<int> Main()
        {
            try
            {
                Console.Title = $"Hypesquadify {Globals.AppVersion}";
                Globals.Http = new HttpClient();

                var values = Enum.GetValues<HypesquadHouse>();
                var prompt = new SelectionPrompt<HypesquadHouse>()
                    .AddChoices(values)
                    .Title("Select a house:");

                var choice = AnsiConsole.Prompt(prompt);

                Console.Clear();
                AnsiConsole.MarkupLine("[bold lime]This will not be shared with anyone.[/]");
                var token = AnsiConsole.Ask<string>("Enter your Discord token:");

                var response = await HypesquadBadge.SetAsync(token, choice);
                response.EnsureSuccessStatusCode();

                AnsiConsole.MarkupLine("[bold]The operation was successful![/]");
                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                return 1;
            }
            finally
            {
                Globals.Http.Dispose();
            }
        }
    }
}
