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
        Dictionary<Point,Block> blocks = new Dictionary<Point,Block>();

        #endregion

        #region Methods
        public void AddBlock(Block block, int x, int y)
        {
            blocks.Add(new Point(x, y), block);
        }
        public void AddBlock(Block block, Vector2 pos)
        {
            blocks.Add(new Point((int)pos.X, (int)pos.Y), block);
        }
        public void AddBlock(Block block, Point point)
        {
            blocks.Add(point, block);
        }
        public void Update()
        {
        }
        public Boolean MoveDown(World world)
        {
            Dictionary<Point, Block> newBlocks = new Dictionary<Point, Block>();

            foreach (KeyValuePair<Point, Block> pair in blocks)
            {
                Point nextPoint = new Point(pair.Key.X,pair.Key.Y + 1);
                newBlocks.Add(nextPoint, pair.Value);
                if(world.Blocks.ContainsKey(nextPoint) || nextPoint.Y >= world.Rows)
                {
                    //Can't move. exit loop
                    return false;
                }
            }

            //Replace the blocks
            blocks = newBlocks;

            return true;
        }
        #endregion

        #region Constructors

        public Shape(Dictionary<Point, Block> points)
        {
            blocks = points;
        }
        public Shape(World world)
        {
            Dictionary<Point, Block> newBlocks = new Dictionary<Point, Block>();
            //Random int for what kind of shape
            switch (GameManager.Random.Next(0, 5))
            {
                case 0:
                    newBlocks = Long.blocks;
                    break;
                case 1:
                    newBlocks = ZShape.blocks;
                    break;
                case 2:
                    newBlocks = SShape.blocks;
                    break;
                case 3:
                    newBlocks = Square.blocks;
                    break;
                case 4:
                    newBlocks = TShape.blocks;
                    break;
            }

            //Move to center and add blocks
            foreach (KeyValuePair<Point, Block> pair in newBlocks)
            {
                blocks.Add(new Point(pair.Key.X + world.Columns / 2, pair.Key.Y), pair.Value);
            }

            //Random Color
            this.Color = new Color(255 * GameManager.Random.Next(0, 2), 255 * GameManager.Random.Next(0, 2), 255 * GameManager.Random.Next(0, 2));
        }
        #endregion

        #region Properties
        //Default shapes
        public static Shape Long
        {
            get
            {
                return new Shape(new Dictionary<Point, Block> {
            { new Point(0,4),new Block() },
            { new Point(0,3),new Block() },
            { new Point(0,2),new Block() },
            { new Point(0,1),new Block() },
            { new Point(0,0),new Block() }
        });
            }
        }
        public static Shape ZShape
        {
            get
            {
                return new Shape(new Dictionary<Point, Block> {
            { new Point(-1,0),new Block() },
            { new Point(0,0),new Block() },
            { new Point(0,1),new Block() },
            { new Point(1,1),new Block() }
        });
            }
        }
        public static Shape SShape
        {
            get
            {
                return new Shape(new Dictionary<Point, Block> {
            { new Point(-1,1),new Block() },
            { new Point(0,1),new Block() },
            { new Point(0,0),new Block() },
            { new Point(1,0),new Block() }
        });
            }
        }
        public static Shape Square
        {
            get
            {
                return new Shape(new Dictionary<Point, Block> {
            { new Point(0,0),new Block() },
            { new Point(1,0),new Block() },
            { new Point(1,1),new Block() },
            { new Point(0,1),new Block() }
        });
            }
        }
        public static Shape TShape
        {
            get
            {
                return new Shape(new Dictionary<Point, Block> {
            { new Point(-1,0),new Block() },
            { new Point(0,0),new Block() },
            { new Point(1,0),new Block() },
            { new Point(0,1),new Block() }
        });
            }
        }
        public Dictionary<Point, Block> Blocks { get { return blocks; } }
        public Color Color
        {
            get { return Color.Black; }
            set
            {
                foreach (KeyValuePair<Point, Block> pair in blocks)
                {
                    pair.Value.Color = value;
                }
            }
        }
        #endregion
    }
}
