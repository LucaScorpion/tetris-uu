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
        Block[,] grid = new Block[rows, columns];

        Shape currentShape;
        ControlMode controlMode;

        Rectangle rect = new Rectangle();
        int borderOffset = 10;

        bool isAlive = true;
        #endregion

        
        #region Methods
        public void Update()
        {
            if (currentShape == null)
            {
                currentShape = new Shape(this, controlMode);
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
                    if (grid[y, x] != null)
                    {
                        grid[y, x].Draw(spriteBatch, CalculateBlockRectangle(x, y));
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
            grid[location.Y, location.X] = block;
        }
        public void Kill()
        {
            isAlive = false;
        }
        #endregion

        #region Constructors
        public World(Rectangle rect, int offset, ControlMode controlMode)
        {
            this.rect = rect;
            this.borderOffset = offset;
            this.controlMode = controlMode;
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