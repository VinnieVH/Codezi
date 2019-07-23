using System;
using System.Threading.Tasks;
using Codezi.Application.Commands;
using Codezi.Application.Services;
using Codezi.Domain.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Codezi.Bot
{
    internal class Program
    {
        private DiscordSocketClient _discordClient;
        private CommandService _commandService;
        private readonly string _token;

        public Program()
        {
            _token = "NjAzMTUxMzg4NDU3MDQxOTIw.XTbPnQ.ipe554OmiJaSe6QH70XblQNM_BA";
        }
        private static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            _discordClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            _commandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            _discordClient.MessageReceived += ClientMessageReceived;
            await _commandService.AddModulesAsync(typeof(CompanyInformationCommand).Assembly, null);
            _discordClient.Ready += ClientReady;
            _discordClient.Log += ClientLog;

            await _discordClient.LoginAsync(TokenType.Bot, _token);
            await _discordClient.StartAsync();

            await Task.Delay(-1);
        }

        private async Task ClientLog(LogMessage arg)
        {
            Console.WriteLine($"{DateTime.Now} at {arg.Source} {arg.Message}");
        }

        private async Task ClientReady()
        {
            await _discordClient.SetGameAsync("Watching the market");
        }

        private async Task ClientMessageReceived(SocketMessage param)
        {
            var message = (SocketUserMessage) param;
            var ctx = new SocketCommandContext(_discordClient, message);

            if (ctx.Message == null || ctx.Message.Content == "") return;
            if (ctx.User.IsBot) return;
            var position = 0;
            if (message.HasStringPrefix("!", ref position) ||
                message.HasMentionPrefix(_discordClient.CurrentUser, ref position))
            {
                var result = await _commandService.ExecuteAsync(ctx, position, null, MultiMatchHandling.Exception);
                if (!result.IsSuccess)
                {
                    Console.WriteLine($"{DateTime.Now} at command service. Something went wrong with executing the command. Text: {ctx.Message.Content} Error: {result.ErrorReason}");
                }
            }
        }
    }
}
