using DSharpPlus;
using LiteDB;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SharkBot.Main
{
    public static class Program
    {
        public static async Task BotAsync()
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

            CreateDatabase("./data.db");
            await Task.Delay(-1);
            await Task.CompletedTask;
        }

        private static void CreateDatabase(string location)
        {
            var db = new LiteDatabase(location);
        }

        public class Warns
        {
            public string Reason { get; set; }

            public uint Id { get; set; }
        }

        public class Config
        {
            public string Token { get; set; }

            public string Prefix { get; set; }
        }
    }
}