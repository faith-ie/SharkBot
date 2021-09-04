using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
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
    }
}