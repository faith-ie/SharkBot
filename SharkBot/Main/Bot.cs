using DSharpPlus;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharkBot.Main
{
    public static class Program
    {
        public static async Task<int> MainAsync()
        {
            Config conf = JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync("./Config.json"));
            if (conf != null)
            {
                DiscordClient discordClient = new(new DiscordConfiguration()
                {
                    Token = conf.Token,
                    TokenType = TokenType.Bot
                });

                await discordClient.ConnectAsync();
            }

            await Task.Delay(-1);
            await Task.CompletedTask;
            return 0;
        }

        public class Config
        {
            public string Token
            {
                get;
                set;
            }

            public string Prefix
            {
                get;
                set;
            }
        }
    }
}