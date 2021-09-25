using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SweatyBot.Modules
{
    public class CustomModule : ModuleBase
    {
        public async Task ServiceReplyAsync(string s)
        {
            await ReplyAsync(s);
        }

        public async Task ServiceReplyAsync(string title, EmbedBuilder emb)
        {
            await ReplyAsync(title, false, emb.Build());
        }

        public async Task ServicePlayingAsync(string s)
        {
            try
            {
                await (Context.Client as DiscordSocketClient).SetGameAsync(s);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
