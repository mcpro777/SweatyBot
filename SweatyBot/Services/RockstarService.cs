using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;
using SweatyBot.Infrastructure;

namespace SweatyBot.Services
{
    public class RockstarService : CustomService
    {
        public void GetRockstarId(String username)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                Log("You need to specify a username | !rockstarid username", (int)LogType.Text);
                return;
            }

            var emb = new EmbedBuilder();
            emb.WithTitle("Here is the rockstar information:");
            emb.AddField("scopped", "123");
            DiscordReply("", emb);
        }
    }
}
