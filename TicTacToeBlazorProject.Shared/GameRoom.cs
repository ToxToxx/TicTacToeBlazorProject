using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeBlazorProject.Shared
{
    public class GameRoom(string roomId, string roomName)
    {
        //room data
        public string RoomId { get; set; } = roomId;
        public string RoomName { get; set; } = roomName;
        //list of current players in room
        public List<Player> Players { get; set; } = new();

        //game ref
        public TicTacToeGame Game { get; set; } = new();

        //Adding a player
        public bool TryAddPlayer(Player newPlayer) 
        {
            if(Players.Count < 2 && 
                !Players.Any(p => 
                p.ConnectionId == newPlayer.ConnectionId))
            {
                Players.Add(newPlayer);
                if(Players.Count == 1)
                {
                    Game.PlayerXId = newPlayer.ConnectionId;
                }
                else if(Players.Count == 2)
                {
                    Game.PlayerOId = newPlayer.ConnectionId;
                }
                return true;
            }
            return false;
        }
    }
}
