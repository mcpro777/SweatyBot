using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SweatyBot
{
    class Program
    {
		private static SweatyBot sweatyBot = new SweatyBot();

		public static void Main(string[] args)
			=> sweatyBot.MainAsync().GetAwaiter().GetResult();
	}
}
