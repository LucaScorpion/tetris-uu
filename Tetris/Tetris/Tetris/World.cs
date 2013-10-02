using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class World
    {
        #region Fields
        static int rows = 20, columns = 12;
        Block[,] grid = new Block[columns, rows];

        Shape currentShape;
        ControlMode controlMode;

        Rectangle rect = new Rectangle();
        int borderOffset = 10;

        bool isAlive = true;
        bool muteShape = true;
        #endregion

        
        #region Methods
        public void Update()
        {
            if (currentShape == null)
            {
                currentShape = new Shape(this, controlMode, muteShape);
            }

            if (isAlive)
            {
                //Update shape
                currentShape.Update(this);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw bg
            spriteBatch.Draw(Assets.Textures.DummyTexture, rect, Color.LightBlue);

            //Draw all blocks
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (grid[x, y] != null)
                    {
                        grid[x, y].Draw(spriteBatch, CalculateBlockRectangle(x, y));
                    }
                }
            }

            //Dark grey overlay
            if (!isAlive)
            {
                spriteBatch.Draw(Assets.Textures.DummyTexture, rect, Color.Black * .6f);
            }
            //Draw shape
            currentShape.Draw(spriteBatch, this);


        }
        List<int> GetFullRows()
        {
            List<int> fullRows = new List<int>();
            for (int y = rows - 1; y >= 0; y--)
            {
                bool fullRow = true;
                for (int x = columns - 1; x >= 0 && fullRow; x--)
                {
                    if (grid[x, y] == null)
                    {
                        fullRow = false;
                    }
                }
                if (fullRow)
                {
                    fullRows.Add(y);
                }
            }

            return fullRows;
        }
        public void DestroyFullRows()
        {
            //Get all full rows
            List<int> fullRows = GetFullRows();

            //Calculate score in Stats
            Stats.CalculateScore(fullRows.Count());

            //Destroy them and move down all blocks above them
            for (int i = 0; i < fullRows.Count(); i++)
            {
                for (int y = fullRows[i] + i; y > 0; y--)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        grid[x, y] = grid[x, y - 1];
                    }
                }
            }

            //Remove row 0
            for (int x = 0; x < columns; x++)
            {
                grid[x, 0] = null;
            }
        }
        public Rectangle CalculateBlockRectangle(int x, int y)
        {
            return new Rectangle(rect.X + borderOffset + x * (rect.Width - 2 * borderOffset) / columns, rect.Y + borderOffset + y * (rect.Height - 2 * borderOffset) / rows, (rect.Width - 2 * borderOffset) / columns, (rect.Height - 2 * borderOffset) / rows);
        }
        public Rectangle CalculateBlockRectangle(Point p)
        {
            return CalculateBlockRectangle(p.X, p.Y);
        }
        public void AddBlock(Block block, Point location)
        {
            grid[location.X, location.Y] = block;
        }
        public void Kill()
        {
            isAlive = false;
        }
        #endregion

        #region Constructors
        public World(Rectangle rect, int offset, ControlMode controlMode, bool muteShape = true)
        {
            this.rect = rect;
            this.borderOffset = offset;
            this.controlMode = controlMode;
            this.muteShape = muteShape;

            this.currentShape = new Shape(this, controlMode, muteShape);
        }
        #endregion

        #region Properties
        public Shape CurrentShape { get { return currentShape; } set { currentShape = value; } }
        public int Rows { get { return rows; } set { if (value > 0) { rows = value; } } }
        public int Columns { get { return columns; } set { if (value > 0) { columns = value; } } }
        public Block[,] Grid { get { return grid; } }
        #endregion
    }
}