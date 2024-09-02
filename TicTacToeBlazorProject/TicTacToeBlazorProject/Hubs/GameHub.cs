using Microsoft.AspNetCore.SignalR;

namespace TicTacToeBlazorProject.Hubs
{
    public class GameHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine
                ($"Player Id: '{Context.ConnectionId}' connected.");
            return base.OnConnectedAsync();
        }
    }
}
