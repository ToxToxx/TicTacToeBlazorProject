using Microsoft.AspNetCore.SignalR;
using TicTacToeBlazorProject.Shared;

namespace TicTacToeBlazorProject.Hubs
{
    public class GameHub : Hub
    {
        private static readonly List<GameRoom> _rooms = new();


        public override async Task OnConnectedAsync()
        {
            Console.WriteLine
                ($"Player Id: '{Context.ConnectionId}' connected.");
            await Clients.Caller.SendAsync("Rooms", _rooms);
            
            return base.OnConnectedAsync();
        }


    }
}
