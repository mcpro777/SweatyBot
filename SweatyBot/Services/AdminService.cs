using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;
using SweatyBot.Infrastructure;

namespace SweatyBot.Services
{
    public class AdminService : CustomService
    {
        public async Task MuteUser(IGuild guild, IUser user)
        {
            try
            {
                await (user as IGuildUser).ModifyAsync(x => x.Mute = true);
                Log($"{user.Mention} has been muted.", (int)LogType.Text);
            }
            catch
            {
                Log($"Error while trying to mute {user}.");
            }
        }

        public async Task UnmuteUser(IGuild guild, IUser user)
        {
            try
            {
                await (user as IGuildUser).ModifyAsync(x => x.Mute = false);
                Log($"{user.Mention} has been unmuted.", (int)LogType.Text);
            }
            catch
            {
                Log($"Error while trying to unmute {user}.");
            }
        }

        public async Task KickUser(IGuild guild, IUser user, string reason = null)
        {
            try
            {
                await (user as IGuildUser).KickAsync(reason);
            }
            catch
            {
                Log($"Error while trying to kick {user}.");
            }
        }
    }
}
