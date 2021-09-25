using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using SweatyBot.Infrastructure;
using SweatyBot.Modules;

namespace SweatyBot.Services
{
    public class CustomService
    {
        private CustomModule _parentModule = null;

        public void SetParentModule(CustomModule parent) { _parentModule = parent; }

        protected async void DiscordReply(string s, EmbedBuilder emb = null)
        {
            if (_parentModule == null) return;
            if (emb != null)
                await _parentModule.ServiceReplyAsync(s, emb);
            else
                await _parentModule.ServiceReplyAsync(s);
        }

        protected async void DiscordPlaying(string s)
        {
            if (_parentModule == null) return;
            await _parentModule.ServicePlayingAsync(s);
        }

        public void Log(string s, int output = (int)LogType.Console)
        {
            string withDate = $"{DateTime.Now.ToString("hh:mm:ss")} DiscordBot {s}";

            if (output == 0)
            {
                Console.WriteLine(withDate);
            }
            else if (output == 1) DiscordReply($"`{s}`");
            else if (output == 2)
            {
                DiscordPlaying(s);
            }
        }
    }
}
