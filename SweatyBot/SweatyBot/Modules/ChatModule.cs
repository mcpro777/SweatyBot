using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using SweatyBot.Services;

namespace SweatyBot.Modules
{
    [Name("Chat")]
    [Summary("Chat module to interact with text chat.")]
    public class ChatModule : CustomModule
    {
        private readonly ChatService _service;

        public ChatModule(ChatService service)
        {
            _service = service;
            _service.SetParentModule(this);
        }

        [Command("botStatus")]
        [Alias("botstatus")]
        [Remarks("botstatus [status]")]
        [Summary("Allows admins to set the bot's current game to [status]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task SetBotStatus([Remainder] string botStatus)
        {
            _service.SetStatus(botStatus);
            await Task.Delay(0);
        }

        [Command("say")]
        [Alias("say")]
        [Remarks("say [msg]")]
        [Summary("The bot will respond in the same channel with the message said.")]
        public async Task Say([Remainder] string usr_msg = "")
        {
            _service.SayMessage(usr_msg);
            await Task.Delay(0);
        }

        [Command("Clear")]
        [Remarks("clear [num]")]
        [Summary("Allows admins to clear [num] amount of messages from current channel")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task ClearMessages([Remainder] int num = 0)
        {
            await _service.ClearMessagesAsync(Context.Guild, Context.Channel, Context.User, num);
        }
    }
}
