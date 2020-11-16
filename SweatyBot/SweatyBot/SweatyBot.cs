using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SweatyBot.Services;

namespace SweatyBot
{
	public class SweatyBot
    {
		const ulong TextChannelGeneral = 712730423893032973;
		//const ulong TextChannelGeneral = 686232562636554259;    //mcpro
		const ulong TextChannelWardroom = 722996559641444402;
		const ulong TextChannelBotChat = 729451587570761738;
		const ulong VoiceChannelAlphaRoom = 712730424316788877;
		const ulong RoleCadet = 717751935066963988;
		//const ulong RoleCadet = 777313543908622367;  //mcpro
		public static char PREFIX = '!';

		private DiscordSocketClient _client;
		private CommandService _commands;
		private IServiceProvider _services;

		public async Task MainAsync()
		{
			String token = Environment.GetEnvironmentVariable("SweatyBotToken");

			var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
			_client = new DiscordSocketClient(_config);
			_commands = new CommandService();
			_services = ConfigureServices();

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			_client.Ready += async () =>
			{
				Console.WriteLine("Bot is connected!");
				await ConfigureCommands();
				await _client.SetActivityAsync(new Game("for input (!help)", ActivityType.Watching, ActivityProperties.None, ""));
			};

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		private IServiceProvider ConfigureServices()
		{
			ServiceCollection services = new ServiceCollection();

			services.AddSingleton<AdminService>();
			services.AddSingleton<AudioService>();
			services.AddSingleton<BlacklistService>();
			services.AddSingleton<ChatService>();
			services.AddSingleton<RockstarService>();

			return services.BuildServiceProvider();
		}

		private async Task ConfigureCommands()
		{
			_client.Log += Log;
			_client.UserJoined += UserJoined;
			_client.UserLeft += UserLeft;
			_client.MessageReceived += HandleCommandAsync;

			// Discover all of the commands in this assembly and load them.
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			// Don't process the command if it was a System Message
			var message = messageParam as SocketUserMessage;
			if (message == null) return;

			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;

			// Determine if the message is a command, based on if it starts with the prefix char or a mention prefix
			if (!(message.HasCharPrefix(SweatyBot.PREFIX, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
			{
				// If it isn't a command, decide what to do with it here. 
				// TODO: Add any special handlers here.
				return;
			}

			//Route commands to handlers
			var context = new SocketCommandContext(_client, message);
			var result = await _commands.ExecuteAsync(context, argPos, _services);

			if (!result.IsSuccess)
            {
				await context.Channel.SendMessageAsync(result.ErrorReason);
			}
		}

		private async Task UserJoined(SocketGuildUser user)
		{
			if (user.IsBot || user.IsWebhook) return;

			var role = user.Guild.Roles.FirstOrDefault(x => x.Id == RoleCadet);
			if (role != null) await user.AddRoleAsync(role);

			//Welcome user
			var channel = _client.GetChannel(TextChannelGeneral) as SocketTextChannel;
			if (channel != null)
			{
				await channel.SendMessageAsync($"Welcome {user.Mention} to {channel.Guild.Name}. As a cadet, you have limited visibility to see channels.");
			}
		}

		private async Task UserLeft(SocketGuildUser user)
		{
			if (user.IsBot || user.IsWebhook) return;

			var channel = _client.GetChannel(TextChannelGeneral) as SocketTextChannel;
			if (channel != null)
			{
				await channel.SendMessageAsync($"{user.Mention} left {channel.Guild.Name}.  Don't let the door hit ya in the ass, noob.");
			}
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
