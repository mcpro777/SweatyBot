using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using SweatyBot.Services;

namespace SweatyBot.Modules
{
    [Name("Blacklist")]
    [Summary("Tracks assholes that you don't want to forget")]
    public class BlacklistModule : CustomModule
    {
        private readonly BlacklistService _service;

        public BlacklistModule(BlacklistService service)
        {
            _service = service;
            _service.SetParentModule(this);
        }

        [Command("blacklist")]
        [Alias("bl")]
        [Remarks("blacklist")]
        [Summary("View the current blacklist")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task GetBlacklist()
        {
            await _service.GetBlacklist();
            await Task.Delay(0);
        }

        [Command("blacklistadd")]
        [Alias("bladd")]
        [Remarks("blacklistadd name rid [reason]")]
        [Summary("Add to the blacklist")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task AddBlacklist(string name, string rid, [Remainder] string reason)
        {
            await _service.AddToBlacklist(name, rid, reason);
            await Task.Delay(0);
        }

        [Command("blacklistdel")]
        [Alias("bldel")]
        [Remarks("blacklistdel name")]
        [Summary("Delete from the blacklist")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task DeleteFromBlacklist(string name)
        {
            await _service.RemoveFromBlacklist(name);
            await Task.Delay(0);
        }

        [Command("blacklistpurge")]
        [Alias("blpurge")]
        [Remarks("blacklistpurge")]
        [Summary("Purge the blacklist")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task PurgeBlacklist()
        {
            await _service.RemoveAll();
            await Task.Delay(0);
        }

        [Command("blacklistupdate")]
        [Alias("blupdate")]
        [Remarks("blacklistupdate name rid [reason]")]
        [Summary("Update the blacklist")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task UpdateBlacklist(string name, string rid, [Remainder] string reason)
        {
            await _service.UpdateBlacklist(name, rid, reason);
            await Task.Delay(0);
        }
    }
}
