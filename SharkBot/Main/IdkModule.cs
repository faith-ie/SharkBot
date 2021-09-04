using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using LiteDB;
using System;
using System.Threading.Tasks;
using static SharkBot.Main.Program;

namespace SharkBot.Main
{
    public class IdkModule : BaseCommandModule
    {
        [Command("ban")]
        [RequireUserPermissions(Permissions.BanMembers)]
        public async Task BanAsync(CommandContext ctx, DiscordMember member, [RemainingText] string reason)
        {
            if (ctx.Message.Author.Id != member.Id)
            {
                DiscordGuild dg = member.Guild;
                try
                {
                    await dg.BanMemberAsync(member, 1, reason);
                    await ctx.Channel.SendMessageAsync($"**{member.Username} has been banned.**");
                }
                catch (Exception e)
                {
                    DiscordEmbedBuilder b = new();
                    b.WithDescription(e.ToString());
                    b.WithColor(DiscordColor.Red);
                    await ctx.Channel.SendMessageAsync(embed: b.Build());
                }
            }
        }

        [Command("kick")]
        [RequireUserPermissions(Permissions.KickMembers)]
        public async Task KickAsync(CommandContext ctx, DiscordMember member, [RemainingText] string reason)
        {
            if (ctx.Message.Author.Id != member.Id)
            {
                try
                {
                    await member.RemoveAsync();
                    await ctx.Channel.SendMessageAsync($"**{member.Username} has been kicked.**");
                }
                catch (Exception e)
                {
                    DiscordEmbedBuilder b = new();
                    b.WithDescription(e.ToString());
                    b.WithColor(DiscordColor.Red);
                    await ctx.Channel.SendMessageAsync(embed: b.Build());
                }
            }
        }

        [Command("warn")]
        [RequireUserPermissions(Permissions.BanMembers)]
        public async Task WarnAsync(CommandContext ctx, DiscordMember member, uint points,
            [RemainingText] string reason)
        {
            int gen = await GenWarnId(1, 99999);
            LiteDatabase db = new("./data.db");
            try
            {
                var col = db.GetCollection<Warns>("warns");
                Warns warn = new()
                {
                    MemberId = member.Id,
                    Points = points,
                    Reason = reason,
                    Id = gen
                };
                col.Insert(warn);
                await ctx.Channel.SendMessageAsync($"Okay, I have warned {member.Username} with {points} points.");
            }
            catch (Exception e)
            {
                DiscordEmbedBuilder b = new();
                b.WithDescription(e.ToString());
                b.WithColor(DiscordColor.Red);
                await ctx.Channel.SendMessageAsync(embed: b.Build());
            }
        }

        private async Task<int> GenWarnId(int min, int max)
        {
            var r = new Random();
            int genId = r.Next(min, max);
            return genId;
        }
    }
}