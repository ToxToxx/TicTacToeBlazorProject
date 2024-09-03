using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeBlazorProject.Shared
{
    // Core logic and state management for a Tic Tac Toe game
    public class TicTacToeGame
    {
        // Player IDs and current player tracking
        public string? PlayerXId { get; set; }
        public string? PlayerOId { get; set; }
        public string? CurrentPlayerId { get; set; }
        public string CurrentPlayerSymbol => CurrentPlayerId == PlayerXId ? "X" : "O";

        // Game state indicators
        public bool GameStarted { get; set; } = false;
        public bool GameOver { get; set; } = false;
        public bool IsDraw { get; set; } = false;
        public string Winner { get; set; } = string.Empty;

        // 2D board representation
        public List<List<string>> Board { get; set; } = new List<List<string>>(3);

        // Constructor: Initializes the board
        public TicTacToeGame()
        {
            InitializeBoard();
        }

        // Starts the game, sets initial player and resets the board
        public void StartGame()
        {
            CurrentPlayerId = PlayerXId;
            GameStarted = true;
            GameOver = false;
            Winner = string.Empty;
            IsDraw = false;
            InitializeBoard();
        }

        // Resets the board to empty cells
        private void InitializeBoard()
        {
            Board.Clear();
            for (int i = 0; i < 3; i++)
            {
                var row = new List<string>(3);
                for (int j = 0; j < 3; j++)
                {
                    row.Add(string.Empty);
                }
                Board.Add(row);
            }
        }

        // Switches the turn to the other player
        public void TogglePlayer()
        {
            CurrentPlayerId = CurrentPlayerId == PlayerXId ? PlayerOId : PlayerXId;
        }

        // Attempts to make a move on the board, returns success status
        public bool MakeMove(int row, int col, string playerId)
        {
            if (playerId != CurrentPlayerId || row < 0 || row >= 3 || col < 0 || col >= 3 || Board[row][col] != string.Empty)
            {
                return false; // Invalid move
            }

            Board[row][col] = CurrentPlayerSymbol; // Place symbol
            TogglePlayer(); // Change turn
            return true;
        }

        // Checks for a winner on the board (rows, columns, diagonals)
        public string CheckWinner()
        {
            // Rows and columns
            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(Board[i][0]) && Board[i][0] == Board[i][1] && Board[i][1] == Board[i][2])
                {
                    return Board[i][0];
                }

                if (!string.IsNullOrEmpty(Board[0][i]) && Board[0][i] == Board[1][i] && Board[1][i] == Board[2][i])
                {
                    return Board[0][i];
                }
            }

            // Diagonals
            if (!string.IsNullOrEmpty(Board[0][0]) && Board[0][0] == Board[1][1] && Board[1][1] == Board[2][2])
            {
                return Board[0][0];
            }
            if (!string.IsNullOrEmpty(Board[0][2]) && Board[0][2] == Board[1][1] && Board[1][1] == Board[2][0])
            {
                return Board[0][2];
            }

            return String.Empty; // No winner found
        }

        // Checks if the game ended in a draw (i.e., all cells filled)
        public bool CheckDraw()
        {
            return IsDraw = Board.All(row => row.All(cell => !string.IsNullOrEmpty(cell)));
        }
    }
}
