using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // For CoordinateSpace
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tic_tac_toe
{
    public partial class Form1 : Form
    {
        private const float clientSize = 100;
        private const float lineLength = 80;
        private const float block = lineLength / 3;
        private const float offset = 10;
        private const float delta = 5;
        private float scale;            // Current scale factor
        
        // Create a game engine
        private GameEngine game = new GameEngine();

        public Form1()
        {
            InitializeComponent();
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ApplyTransform(g);

            // Draw board
            g.DrawLine(Pens.Black, block, 0, block, lineLength);
            g.DrawLine(Pens.Black, 2 * block, 0, 2 * block, lineLength);
            g.DrawLine(Pens.Black, 0, block, lineLength, block);
            g.DrawLine(Pens.Black, 0, 2 * block, lineLength, 2 * block);

            GameEngine.CellSelection[,] grid = game.getBoard();

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (grid[i, j] == GameEngine.CellSelection.O)
                    {
                        DrawO(i, j, g);
                    }
                    else if (grid[i, j] == GameEngine.CellSelection.X)
                    {
                        DrawX(i, j, g);
                    }
                }
            }
        }

        private void ApplyTransform(Graphics g)
        {
                scale = Math.Min(ClientRectangle.Width / clientSize, ClientRectangle.Height / clientSize);
                
                if (scale == 0f) return;
                g.ScaleTransform(scale, scale);
                g.TranslateTransform(offset, offset);
        }

        private void DrawX(int i, int j, Graphics g)
        {
            g.DrawLine(Pens.Black, (i * block) + delta, (j * block) + delta, (i * block) + block - delta, (j * block) + block - delta);
            g.DrawLine(Pens.Black, (i * block) + block - delta, (j * block) + delta, (i * block) + delta, (j * block) + block - delta);
        }

        private void DrawO(int i, int j, Graphics g)
        {
            g.DrawEllipse(Pens.Black, (i * block) + delta, (j * block) + delta, block - (2 * delta), block - (2 * delta));
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            ApplyTransform(g);
            PointF[] p = { new Point(e.X, e.Y) };
            g.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, p);
            if (p[0].X < 0 || p[0].Y < 0) return;
            int i = (int)(p[0].X / block);
            int j = (int)(p[0].Y / block);
            if (i > 2 || j > 2) return;

            // Only allow setting empty cells
            // Only play if the game is not over
            if (game.cellIsEmpty(i,j) && game.isOver() == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    game.playerMove(i, j);
                    // Update the game after a move has been made.
                    game.Update();
                    if (!game.isOver())
                    {
                        // Recheck to see if the game is over after player move
                        game.computerMove();
                    }
                }
                
                // Invalidate so that the board updates
                this.Invalidate();
            }
            else if (game.isOver() == true)
            {
                // Do nothing
            }
            else if (!game.cellIsEmpty(i, j))
            {
                MessageBox.Show("Move not allowed.");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Reset the state of the game
            game.Reset();

            this.Invalidate();
        }

        private void computerStartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The GameEngine handles what move the computer takes
            // Computer first is disabled after the first move
            if (game.getMoveCount() < 1)
            {
                game.computerMove();
            }

            if (game.isOver())
            {
                game.Reset();
                game.computerMove();
            }

            this.Invalidate();
        }
    }
}
