using DSharpPlus;
using DSharpPlus.CommandsNext;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SharkBot.Main
{
    public static class Program
    {
        public static async Task BotAsync()
        {
            Config conf = JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync("./Config.json"));

            DiscordClient discordClient = new(new DiscordConfiguration()
            {
                Token = conf.Token,
                TokenType = TokenType.Bot
            });

            var services = new ServiceCollection()
                .AddSingleton<IdkModule>()
                .BuildServiceProvider();

            var commands = discordClient.UseCommandsNext(new CommandsNextConfiguration()
            {
                Services = services,
                StringPrefixes = new[] { conf.Prefix },
                CaseSensitive = false,
                EnableMentionPrefix = true
            });
            commands.RegisterCommands<IdkModule>();
            CreateDatabase("./data.db");
            await discordClient.ConnectAsync();
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

            public ulong MemberId
            {
                get; set;
            }

            public uint Points
            {
                get; set;
            }

            public int Id { get; set; }
        }

        public class Config
        {
            public string Token { get; set; }

            public string Prefix { get; set; }
        }
    }
}