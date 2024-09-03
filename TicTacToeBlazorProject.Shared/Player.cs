using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeBlazorProject.Shared
{
    public class Player(string connectionId, string name)
    {
        //player data
        public string ConnectionId { get; set; } = connectionId;
        public string Name { get; set; } = name;
    }
}
