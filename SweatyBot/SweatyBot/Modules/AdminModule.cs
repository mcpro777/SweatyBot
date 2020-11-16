using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using SweatyBot.Services;

namespace SweatyBot.Modules
{
    [Name("Admin")]
    [Summary("Admin module to manage this discord server.")]
    public class AdminModule : CustomModule
    {
        private readonly AdminService _service;

        public AdminModule(AdminService service)
        {
            _service = service;
            _service.SetParentModule(this);
        }

        [Command("mute")]
        [Remarks("mute [user]")]
        [Summary("This allows admins to mute users.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task MuteUser([Remainder] IGuildUser user)
        {
            await _service.MuteUser(Context.Guild, user);
        }

        [Command("unmute")]
        [Remarks("unmute [user]")]
        [Summary("This allows admins to unmute users.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task UnmuteUser([Remainder] IGuildUser user)
        {
            await _service.UnmuteUser(Context.Guild, user);
        }

        [Command("kick")]
        [Remarks("kick [user] [reason]")]
        [Summary("This allows admins to kick users.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickUser(IGuildUser user, [Remainder] string reason = null)
        {
            await _service.KickUser(Context.Guild, user, reason);
        }
    }
}
