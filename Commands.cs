using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _nerdyBoi.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        public async Task PingAsync()
        {
            await ReplyAsync("Yo :b:");
        }
    }
}