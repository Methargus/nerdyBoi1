using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Welcome;

namespace _nerdyBoi
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotASync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotASync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = "NDY3NzY5NzE3NzM0NzY4NjYw.DivqIQ.lEri5tR9yDqXs3MoNLpVid-jUM0";

            _client.Log += Log;
            _client.UserJoined += AnnounceJoinedUser;
            await RegisterCommandASync();
            await _client.LoginAsync(TokenType.Bot, botToken);
            await _client.StartAsync();
            await Task.Delay(-1);

        }

        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            var guild = user.Guild;
            var _inputJpeg2 = ($"https://cdn.discordapp.com/avatars/{user.Id}/{user.AvatarId}.jpeg");
            WelcomeGenerating util = new WelcomeGenerating();
            String folder = @"C:\Users\pysiak\Downloads\Discord_Img\";
            Bitmap bitMapImage = new System.Drawing.Bitmap(folder + "hellothere.jpg");
            byte[] imageBytes = util.imageToByteArray(bitMapImage);
            Bitmap b = util.ByteArraytoBitmap(imageBytes);
            byte[] output = util.createImage(user.Username, b, _inputJpeg2);
            Bitmap bm = util.ByteArraytoBitmap(output);
            bm.Save(folder + "output.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            var channel = guild.GetChannel(313433828460855296) as SocketTextChannel;
            await channel.SendFileAsync(folder + "output.jpeg", "Hello There! " + user.Mention);

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandASync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message == null || message.Author.IsBot) return;

            int argPos = 0;

            if (message.HasStringPrefix("_!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }

    }
}
