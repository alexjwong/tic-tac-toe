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

        // 3x3 array holds the board
        private CellSelection[,] board = new CellSelection[3, 3];
        
        public GameEngine()
        {

        }

        public void Update()   // Updates the state of the game
        {
            // Update is called after a move has been made by the user

            // Increment the move counter
            this.MoveCount++;

            // Check the board
            this.CheckBoard();

            Console.WriteLine(LastMove);
            Console.WriteLine(GameOver);
            Console.WriteLine(CanWin(CellSelection.X));

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
            else // If not, the computer moves
            {
                this.ComputerMove();
            }
        }

        private void CheckBoard()
        {
            // Since we know the last move, we can use that to make our checking easier

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

            // Check Rows
            for (int i = 0; i < 3; i++)
            {
                if (board[i, LastMove.Y] != board[LastMove.X, LastMove.Y])
                    break;
                if (i == 2)
                {
                    GameOver = true;
                }
            }

            // Check Diagonals
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

            // Check tie
            if (this.MoveCount == 9)
            {
                Draw = true;
                MessageBox.Show("It's a draw.");
            }

            // Else the game is not over!
        }

        private void ComputerMove()
        {
            // If the computer can win
            if (CanWin(CellSelection.O).Item1 == true)
            {
                board[CanWin(CellSelection.O).Item2.X, CanWin(CellSelection.O).Item2.Y] = CellSelection.O;
                LastMove = new Point(CanWin(CellSelection.O).Item2.X, CanWin(CellSelection.O).Item2.Y);
            }

            // If the computer can't win, look to block
            else if (CanWin(CellSelection.X).Item1 == true)
            {
                board[CanWin(CellSelection.X).Item2.X, CanWin(CellSelection.X).Item2.Y] = CellSelection.O;
                LastMove = new Point(CanWin(CellSelection.X).Item2.X, CanWin(CellSelection.X).Item2.Y);
            }

            // If the computer can't do either, play an intelligent move

            // If opponent moves corner for first move, computer must choose center

            // If center taken, take corner

            // Special case if opponent has two opposite corners and player has center, need to play a side!

            // if corner NOT taken already, take random corner

            // Set computermove as the last move
            // LastMove = new Point(i, j);
        }

        private Tuple<bool, Point> CanWin(CellSelection player)
        {
            // Returns if the player can win, and the spot if so.
            Point spot = new Point();
            // Can only win if there are more than 4 moves
            // Horizontals
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
            // Verticals
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
        }
    }
}
