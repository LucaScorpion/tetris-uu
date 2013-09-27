using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Shape
    {
        #region Fields
        Block[,] grid;
        Point location;
        Vector2 gridCenter;
        #endregion

        #region Methods
        public void Draw(SpriteBatch spriteBatch, World world)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] != null)
                    {
                        spriteBatch.Draw(grid[y, x].Texture, world.CalculateBlockRectangle(GetWorldLocation(x, y)), grid[y, x].Color);
                    }
                }
            }
        }
        Point GetWorldLocation(int x, int y)
        {
            return new Point((int)(x - (int)gridCenter.X + location.X), (int)(y - (int)gridCenter.Y + location.Y));
        }
        #endregion

        #region Constructors
        public Shape(World world)
        {
            //Select random shape
            switch (GameManager.Random.Next(0, 7))
            {
                case 0:
                    grid = IShape;
                    break;
                case 1:
                    grid = LShape;
                    break;
                case 2:
                    grid = JShape;
                    break;
                case 3:
                    grid = ZShape;
                    break;
                case 4:
                    grid = SShape;
                    break;
                case 5:
                    grid = TShape;
                    break;
                case 6:
                    grid = OShape;
                    break;
            }

            
            gridCenter = new Vector2(grid.GetLength(1), grid.GetLength(0)) / 2;
            location = new Point(world.Columns / 2 - 1, (int)gridCenter.Y);

            //Random Color
            Color = new Color(255 * GameManager.Random.Next(0, 2), 255 * GameManager.Random.Next(0, 2), 255 * GameManager.Random.Next(0, 2));
        }
        #endregion

        #region Properties
        //Default shapes
        public static Block[,] IShape
        {
            get { return new Block[4, 4] { { null, null, null, null }, { new Block(), new Block(), new Block(), new Block() }, { null, null, null, null }, { null, null, null, null } }; }
        }
        public static Block[,] LShape
        {
            get { return new Block[3, 3] { { null, null, new Block() }, { new Block(), new Block(), new Block() }, { null, null, null } }; }
        }
        public static Block[,] JShape
        {
            get { return new Block[3, 3] { { new Block(), null, null }, { new Block(), new Block(), new Block() }, { null, null, null } }; }
        }
        public static Block[,] ZShape
        {
            get { return new Block[3, 3] { { new Block(), new Block(), null }, { null, new Block(), new Block() }, { null, null, null } }; }
        }
        public static Block[,] SShape
        {
            get { return new Block[3, 3] { { null, new Block(), new Block() }, { new Block(), new Block(), null }, { null, null, null } }; }
        }
        public static Block[,] TShape
        {
            get { return new Block[3, 3] { { null, new Block(), null }, { new Block(), new Block(), new Block() }, { null, null, null } }; }
        }
        public static Block[,] OShape
        {
            get { return new Block[2, 2] { { new Block(), new Block() }, { new Block(), new Block() } }; }
        }

        //Properties
        public Color Color
        {
            get { return Color.White; }
            set
            {
                foreach (Block b in grid)
                {
                    if (b != null)
                    {
                        b.Color = value;
                    }
                }
            }
        }
        #endregion
    }
}
