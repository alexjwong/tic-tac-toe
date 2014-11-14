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


            // If the computer can't win, look to block

            // If the computer can't do either, play an intelligent move

            // If opponent moves corner for first move, computer must choose center

            // If center taken, take corner

            // Special case if opponent has two opposite corners and player has center, need to play a side!

            // if corner NOT taken already, take random corner 

            // Set computermove as the last move
            // LastMove = new Point(i, j);
        }

        private void CanWin(CellSelection player)
        {
            // Can only win if there are more than 4 moves
            if (MoveCount >= 4)
            {

            }
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
            if (GameOver)
            {
                return true;
            }
            else return false;
        }

        public CellSelection[,] getBoard()
        {
            return board;
        }

        public bool cellIsEmpty(int i, int j)
        {
            if (board[i, j] == CellSelection.N)
            {
                return true;
            }
            else return false;
        }

        public void playerMove(int i, int j)
        {
            board[i, j] = CellSelection.X;
            // Set this as the last move
            LastMove = new Point(i, j);
        }
    }
}
