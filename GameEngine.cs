using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace tic_tac_toe
{
    class GameEngine
    {
        public enum CellSelection { N, O, X };

        private bool GameOver = false;
        private bool Draw = false;
        private int MoveCount = 0;
        private CellSelection WinningPlayer;
        private Point LastMove;
        private Random random = new Random();

        // 3x3 array holds the board
        private CellSelection[,] board = new CellSelection[3, 3];
        
        public GameEngine()
        {

        }

        public void Update()   // Updates the state of the game
        {
            // Update is called after a move has been made by the user

            // Check the board
            this.CheckBoard();

            Console.WriteLine("MoveCount: "+MoveCount);
            Console.WriteLine("LastMove: " +LastMove);
            Console.WriteLine("GameOver: " +GameOver);
        }

        private void CheckBoard()
        {
            // We use the last move made to make our checking easier

            // Check columns
            for (int i = 0; i < 3; i++)
            {
                if (board[LastMove.X, i] != board[LastMove.X, LastMove.Y])
                    break;
                if (i == 2)
                {
                    GameOver = true;
                }
            }

            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (board[i, LastMove.Y] != board[LastMove.X, LastMove.Y])
                    break;
                if (i == 2)
                {
                    GameOver = true;
                }
            }

            // Check diagonals
            for (int i = 0; i < 3; i++)
            {
                if (board[i, i] != board[LastMove.X, LastMove.Y])
                    break;
                if (i == 2)
                {
                    GameOver = true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 2 - i] != board[LastMove.X, LastMove.Y])
                    break;
                if (i == 2)
                {
                    GameOver = true;
                }
            }

            // Set winning player if the game was found to be over
            if (GameOver)
            {
                // If the last move caused the game to end, can find the player from the last move
                WinningPlayer = board[LastMove.X, LastMove.Y];
            }

            if (GameOver)
            {
                if (WinningPlayer == CellSelection.X)
                {
                    MessageBox.Show("You Won!");
                }
                else if (WinningPlayer == CellSelection.O)
                {
                    MessageBox.Show("You lose.");
                }
            }

            // Check tie
            if (!GameOver && this.MoveCount == 9)
            {
                Draw = true;
                MessageBox.Show("It's a draw.");
            }

            // Else the game is not over!
        }

        public void computerMove()
        {
            // If the computer can win
            if (CanWin(CellSelection.O).Item1 == true)
            {
                board[CanWin(CellSelection.O).Item2.X, CanWin(CellSelection.O).Item2.Y] = CellSelection.O;
                LastMove = new Point(CanWin(CellSelection.O).Item2.X, CanWin(CellSelection.O).Item2.Y);
            }

            // If the computer can't win, look to block if player can win
            else if (CanWin(CellSelection.X).Item1 == true)
            {
                board[CanWin(CellSelection.X).Item2.X, CanWin(CellSelection.X).Item2.Y] = CellSelection.O;
                LastMove = new Point(CanWin(CellSelection.X).Item2.X, CanWin(CellSelection.X).Item2.Y);
            }

            // If the computer can't do either, play an intelligent move

            // If computer is playing first, pick a random corner
            /*
            else if (MoveCount == 0)
            {
                int corner = random.Next(4);
                switch (corner)
                {
                    case 0: board[0, 0] = CellSelection.O; LastMove = new Point(0, 0); break;
                    case 1: board[0, 2] = CellSelection.O; LastMove = new Point(0, 2); break;
                    case 2: board[2, 0] = CellSelection.O; LastMove = new Point(2, 0); break;
                    case 3: board[2, 2] = CellSelection.O; LastMove = new Point(2, 0); break;
                }
            }
            */

            // If opponent moves corner for first move, computer must choose center
            else if (MoveCount == 1 && (board[0,0] == CellSelection.X || board[2,0] == CellSelection.X 
                || board[0,2] == CellSelection.X || board[2,2] == CellSelection.X))
            {
                board[1,1] = CellSelection.O;
                LastMove = new Point(1,1);
            }

            // If center taken as the player first move, take random corner
            else if (MoveCount == 1 && board[1,1] == CellSelection.X)
            {
                int corner = random.Next(4);
                switch (corner)
                {
                    case 0: board[0, 0] = CellSelection.O; LastMove = new Point(0, 0); break;
                    case 1: board[0, 2] = CellSelection.O; LastMove = new Point(0, 2); break;
                    case 2: board[2, 0] = CellSelection.O; LastMove = new Point(2, 0); break;
                    case 3: board[2, 2] = CellSelection.O; LastMove = new Point(2, 0); break;
                }
            }

            // Special case if opponent has two opposite corners and player has center, need to play a side!
            else if ((board[0, 0] == CellSelection.X && board[2, 2] == CellSelection.X)
                || (board[2, 0] == CellSelection.X && board[0, 2] == CellSelection.X) && board[1, 1] == CellSelection.O && MoveCount == 3)
            {
                int side = random.Next(4);
                switch (side)
                {
                    case 0: if (board[1, 0] == CellSelection.N) board[1, 0] = CellSelection.O; LastMove = new Point(1, 0); break;
                    case 1: if (board[0, 1] == CellSelection.N) board[0, 1] = CellSelection.O; LastMove = new Point(0, 1); break;
                    case 2: if (board[2, 1] == CellSelection.N) board[2, 1] = CellSelection.O; LastMove = new Point(2, 1); break;
                    case 3: if (board[1, 2] == CellSelection.N) board[1, 2] = CellSelection.O; LastMove = new Point(1, 2); break;
                }
            }

            // If no special moves, try to find a basic move
            else
            {
                bool MoveFound = false;
                while (MoveFound == false)
                {
                    // Corners
                    List<Point> corners = new List<Point> { new Point(0, 0), new Point(0, 2), new Point(2, 0), new Point(2, 2) };
                    bool CornerFound = false;
                    while (CornerFound == false && MoveFound == false && corners.Count != 0)
                    {
                        int corner = random.Next(corners.Count);
                        if (board[corners[corner].X, corners[corner].Y] == CellSelection.N)
                        {
                            board[corners[corner].X, corners[corner].Y] = CellSelection.O;
                            LastMove = corners[corner];
                            CornerFound = true;
                            MoveFound = true;
                        }
                        else // Corner taken
                        {
                            // Remove the corner that is taken and try again
                            corners.RemoveAt(corner);
                        }
                    }

                    // Center
                    if (board[1, 1] == CellSelection.N && MoveFound == false)
                    {
                        board[1, 1] = CellSelection.O;
                        LastMove = new Point(0, 0);
                        break;
                    }

                    // Sides
                    List<Point> sides = new List<Point> { new Point(1, 0), new Point(0, 1), new Point(2, 1), new Point(1, 2) };
                    bool SideFound = false;
                    while (SideFound == false && MoveFound == false && sides.Count != 0)
                    {
                        int side = random.Next(sides.Count);
                        if (board[sides[side].X, sides[side].Y] == CellSelection.N)
                        {
                            board[sides[side].X, corners[side].Y] = CellSelection.O;
                            LastMove = sides[side];
                            SideFound = true;
                            MoveFound = true;
                        }
                        else // Side taken
                        {
                            // Remove the side that is taken and try again
                            sides.RemoveAt(side);
                        }
                    }
                }
            }

            // Increment MoveCounter
            MoveCount++;
            this.CheckBoard();
        }

        private Tuple<bool, Point> CanWin(CellSelection player)
        {
            // Returns if the player can win, and the spot if so.
            
            // The spot to return
            Point spot = new Point();
            
            // Check verticals
            for (int i = 0; i < 3; i++)
            {
                int SpotCount = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == player)
                    {
                        SpotCount++;
                    }
                    else spot = new Point(i, j);
                }
                // If there are two spots taken
                if (SpotCount == 2 && board[spot.X, spot.Y] == CellSelection.N)
                {
                    return Tuple.Create(true, spot);
                }
            }
            // Check horizontals
            for (int j = 0; j < 3; j++)
            {
                int SpotCount = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (board[i, j] == player)
                    {
                        SpotCount++;
                    }
                    else spot = new Point(i, j);
                }
                // If there are two spots taken
                if (SpotCount == 2 && board[spot.X, spot.Y] == CellSelection.N)
                {
                    return Tuple.Create(true, spot);
                }
            }
            // Diagonals
            while (true)
            {
                // Regular diagonal
                int SpotCount = 0;

                for (int i = 0; i < 3; i++)
                {
                    if (board[i, i] == player)
                    {
                        SpotCount++;
                    }
                    else spot = new Point(i, i);
                }
                if (SpotCount == 2 && board[spot.X, spot.Y] == CellSelection.N)
                {
                    return Tuple.Create(true, spot);
                }
                else break;
            }

            while (true)
            {
                // Anti-diagonal
                int SpotCount = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (board[i, 2 - i] == player)
                    {
                        SpotCount++;
                    }
                    else spot = new Point(i, 2 - i);
                }
                if (SpotCount == 2 && board[spot.X, spot.Y] == CellSelection.N)
                {
                    return Tuple.Create(true, spot);
                }
                else break;
            }
            
            // If can't find any spot that a player can win, return false
            return Tuple.Create(false, spot);
        }

        public void Reset()
        {
            // Reset all state variables to prepare for new game
            GameOver = false;
            Draw = false;
            MoveCount = 0;
            WinningPlayer = CellSelection.N;
            LastMove = new Point();

            // Reset the board
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = CellSelection.N;
                }
            }
        }

        public bool isOver()
        {
            // Returns true if the game is over.
            if (GameOver)
            {
                return true;
            }
            else return false;
        }

        public CellSelection[,] getBoard()
        {
            // Returns the current state of the board (for drawing purposes)
            return board;
        }

        public bool cellIsEmpty(int i, int j)
        {
            // Returns true if a cell is empty
            if (board[i, j] == CellSelection.N)
            {
                return true;
            }
            else return false;
        }

        public void playerMove(int i, int j)
        {
            // Inputs the player's move to the board based on coordinates
            // received from the GUI
            board[i, j] = CellSelection.X;
            // Set this as the last move
            LastMove = new Point(i, j);
            // Increment MoveCounter
            MoveCount++;
        }
    }
}
