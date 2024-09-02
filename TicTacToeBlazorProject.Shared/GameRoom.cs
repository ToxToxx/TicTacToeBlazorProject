using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeBlazorProject.Shared
{
    public class GameRoom(string roomId, string roomName)
    {
        public string RoomId { get; set; } = roomId;
        public string RoomName { get; set; } = roomName;
    }
}
