using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Interactions;

namespace Erra_Discord_Sharp_V1
{
    class Program
    {
        private readonly DiscordSocketClient _client;
        static void Main(string[] args)
            => new Program()
                .MainAsync()
                .GetAwaiter()
                .GetResult();
        public Program() 
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            _client = new DiscordSocketClient(config);
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
            _client.InteractionCreated += InteractionCreatedAsync;
        }
        public async Task MainAsync()
        {
            var token = "";

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
        private Task LogAsync(LogMessage log) 
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");
            return Task.CompletedTask;
        }
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.Id == _client.CurrentUser.Id)
            {
                return;
            }

            if (message.Content == "e!ping")
            {
                var cb = new ComponentBuilder().WithButton("Click me!", "btnPing", ButtonStyle.Primary);

                await message.Channel.SendMessageAsync("pong, I am alive !", components: cb.Build());
            }
        }
        private async Task InteractionCreatedAsync(SocketInteraction interaction)
        {
            if (interaction is SocketMessageComponent component)
            {
                if (component.Data.CustomId == "btnPing")
                {
                    await interaction.RespondAsync("Yay ! You clicked meee !!");
                }
                else
                {
                    Console.WriteLine("An ID has been received that as no handler...");
                }
            }
        }
    }
}