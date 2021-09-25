using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using SweatyBot.Services;

namespace SweatyBot.Modules
{
    [Name("Rockstar")]
    [Summary("Provides rockstar services")]
    public class RockstarModule : CustomModule
    {
        private readonly RockstarService _service;

        public RockstarModule(RockstarService service)
        {
            _service = service;
            _service.SetParentModule(this);
        }

        [Command("rockstarid")]
        [Alias("rid")]
        [Remarks("rockstarid username")]
        [Summary("Get rockstar id by username")]
        public async Task GetRockstarId(String username)
        {
            _service.GetRockstarId(username);
            await Task.Delay(0);
        }
    }
}
