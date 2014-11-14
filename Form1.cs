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


        public enum CellSelection { N, O, X };
        public CellSelection[,] grid = new CellSelection[3, 3];
        

        // Create a game engine
        private GameEngine game = new GameEngine();

        // Perhaps figure out a way to pass the grid as a reference..


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

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (grid[i, j] == CellSelection.O)
                    {
                        DrawO(i, j, g);
                    }
                    else if (grid[i, j] == CellSelection.X)
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

            if (e.Button == MouseButtons.Middle)
            {
                grid[i, j] = CellSelection.N;
            }

            // Only allow setting empty cells
            if (grid[i, j] == CellSelection.N)
            {
                if (e.Button == MouseButtons.Left)
                {
                    grid[i, j] = CellSelection.X;
                }
                if (e.Button == MouseButtons.Right)
                {
                    grid[i, j] = CellSelection.O;
                }
                // Get the last move made for easier checking of board
                game.getLastMove(i, j);
                // Update the game after a move has been made.
                game.Update(grid);
                this.Invalidate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Reset grid
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    grid[i, j] = CellSelection.N;
                }
            }

            // Reset the state of the game
            game.Reset();

            this.Invalidate();
        }

        private void computerStartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The GameEngine handles what move the computer takes
            game.Update(grid);

            this.Invalidate();
        }
    }
}
