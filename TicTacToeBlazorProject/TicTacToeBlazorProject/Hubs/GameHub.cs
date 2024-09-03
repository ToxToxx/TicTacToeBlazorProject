using Microsoft.AspNetCore.SignalR;
using TicTacToeBlazorProject.Shared;

namespace TicTacToeBlazorProject.Hubs
{
    // Hub class for managing game rooms, players, and gameplay logic in real-time
    public class GameHub : Hub
    {
        // Static list to keep track of all active game rooms
        private static readonly List<GameRoom> _rooms = new();

        // Called when a new client connects to the hub
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Player Id: '{Context.ConnectionId}' connected.");

            // Sends the list of rooms to the newly connected client
            await Clients.Caller.SendAsync("Rooms", _rooms.OrderBy(r => r.RoomName));
        }

        // Method to create a new game room and add the first player
        public async Task<GameRoom> CreateRoom(string name, string playerName)
        {
            var roomId = Guid.NewGuid().ToString(); // Generate a unique ID for the room
            var room = new GameRoom(roomId, name); // Create the game room object
            _rooms.Add(room); // Add the room to the list of rooms

            var newPlayer = new Player(Context.ConnectionId, playerName); // Create the player object

            room.TryAddPlayer(newPlayer); // Add the player to the room

            // Add the player to a SignalR group corresponding to the room
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            // Notify all clients about the updated list of rooms
            await Clients.All.SendAsync("Rooms", _rooms.OrderBy(r => r.RoomName));

            return room; // Return the created room
        }

        // Method for a player to join an existing room
        public async Task<GameRoom?> JoinRoom(string roomId, string playerName)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == roomId); // Find the room by ID

            if (room is not null)
            {
                var newPlayer = new Player(Context.ConnectionId, playerName); // Create the player object

                if (room.TryAddPlayer(newPlayer)) // Add the player to the room if possible
                {
                    // Add the player to the SignalR group for the room
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                    // Notify all players in the room that a new player has joined
                    await Clients.Group(roomId).SendAsync("PlayerJoined", newPlayer);

                    return room; // Return the room that the player joined
                }
            }

            return null; // Return null if the room was not found or the player couldn't join
        }

        // Method to start the game in a specific room
        public async Task StartGame(string roomId)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == roomId); // Find the room by ID

            if (room is not null)
            {
                room.Game.StartGame(); // Start the game

                // Notify all players in the room that the game has started
                await Clients.Group(roomId).SendAsync("UpdateGame", room);
            }
        }

        // Method to make a move in the game
        public async Task MakeMove(string roomId, int row, int col, string playerId)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomId == roomId); // Find the room by ID

            if (room != null && room.Game.MakeMove(row, col, playerId)) // Make the move if valid
            {
                room.Game.Winner = room.Game.CheckWinner(); // Check if there's a winner
                room.Game.IsDraw = room.Game.CheckDraw() && string.IsNullOrEmpty(room.Game.Winner); // Check if it's a draw

                if (!string.IsNullOrEmpty(room.Game.Winner) || room.Game.IsDraw)
                {
                    room.Game.GameOver = true; // End the game if there's a winner or a draw
                }

                // Notify all players in the room about the updated game state
                await Clients.Group(roomId).SendAsync("UpdateGame", room);
            }
        }
    }
}
