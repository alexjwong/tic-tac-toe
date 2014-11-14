using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace tic_tac_toe
{
    class GameEngine
    {
        private bool GameOver = false;
        private bool Draw = false;
        private int MoveCount = 0;
        private Point LastMove;
        
        public GameEngine()
        {

        }

        public void Update(Form1.CellSelection[,] board)   // Updates the state of the game
        {
            // Update is called after a move has been made by the user

            // Increment the move counter
            this.MoveCount++;

            // Check the board
            this.CheckBoard(board);

            Console.WriteLine(LastMove);
            Console.WriteLine(GameOver);

            if (GameOver)
            {

            }
            else // If not, the computer moves
            {
                this.ComputerMove(board);
            }
        }

        private void CheckBoard(Form1.CellSelection[,] board)
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

            // Check tie
            if (this.MoveCount == 9)
            {
                Draw = true;
            }

            // Else the game is not over!
        }

        private void ComputerMove(Form1.CellSelection[,] board)
        {
            // If the computer can win

            // If the computer can't win, look to block

            // If the computer can't do either, play an intelligent move

            // If opponent moves corner for first move, computer must choose center

            // If center taken, take corner

            // Special case if opponent has two opposite corners and player has center, need to play a side!

            // if corner NOT taken already, take random corner 
        }

        private void CanWin(Form1.CellSelection[,] board, char player)
        {

        }

        public void getLastMove(int i, int j)
        {
            LastMove = new Point(i, j);
        }

        public void Reset()
        {
            // Reset all state variables to prepare for new game
            GameOver = false;
            Draw = false;
            MoveCount = 0;
        }

        public bool isOver()
        {
            if (GameOver)
            {
                return true;
            }
            else return false;
        }
    }
}
