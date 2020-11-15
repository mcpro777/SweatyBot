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
		const ulong TextChannelGeneral = 712730423893032973;
		//const ulong TextChannelGeneral = 686232562636554259;    //mcpro
		const ulong TextChannelWardroom = 722996559641444402;
		const ulong TextChannelBotChat = 729451587570761738;
		const ulong VoiceChannelAlphaRoom = 712730424316788877;
		const ulong RoleCadet = 717751935066963988;
		//const ulong RoleCadet = 777313543908622367;  //mcpro

		private DiscordSocketClient _client;
		private readonly CommandService _commands;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			String token = Environment.GetEnvironmentVariable("SweatyBotToken");

			var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
			_client = new DiscordSocketClient(_config);
			_client.Log += Log;
            _client.UserJoined += _client_UserJoined;
            _client.MessageReceived += HandleCommandAsync;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

			_client.Ready += () =>
			{
				Console.WriteLine("Bot is connected!");
				return Task.CompletedTask;
			};

			// Block this task until the program is closed.
			await Task.Delay(-1);
        }

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			// Don't process the command if it was a system message
			var message = messageParam as SocketUserMessage;
			if (message == null) return;

			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;

			// Determine if the message is a command based on the prefix and make sure no bots trigger commands
			if (!(message.HasCharPrefix('!', ref argPos) ||
				message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
				message.Author.IsBot)
				return;

			// Create a WebSocket-based command context based on the message
			var context = new SocketCommandContext(_client, message);
			if (context.Message.Content.StartsWith("!sweatybot"))
            {
				var channel = _client.GetChannel(TextChannelGeneral) as SocketTextChannel;
				await channel.SendMessageAsync("fuck off");
			}

			// Execute the command with the command context we just
			// created, along with the service provider for precondition checks.

			// Keep in mind that result does not indicate a return value
			// rather an object stating if the command executed successfully.
			//////var result = await _commands.ExecuteAsync(
			//////	context: context,
			//////	argPos: argPos,
			//////	services: null);

			// Optionally, we may inform the user if the command fails
			// to be executed; however, this may not always be desired,
			// as it may clog up the request queue should a user spam a
			// command.
			// if (!result.IsSuccess)
			// await context.Channel.SendMessageAsync(result.ErrorReason);
		}

		private async Task _client_UserJoined(SocketGuildUser user)
        {
			try
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
			catch (Exception ex)
            {
				throw ex;
			}
		}

		private Task _client_MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
		{
			// If the message was not in the cache, downloading it will result in getting a copy of `after`.
			//var message = await before.GetOrDownloadAsync();
			//Console.WriteLine($"{message} -> {after}");
			return Task.CompletedTask;
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
