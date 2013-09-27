using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Shape
    {
        #region Fields
        Dictionary<Point,Block> blocks = new Dictionary<Point,Block>();

        //Default shapes
        static Shape longDefaultShape = new Shape(new Dictionary<Point, Block> {
            { new Point(0,4),new Block() },
            { new Point(0,3),new Block() },
            { new Point(0,2),new Block() },
            { new Point(0,1),new Block() },
            { new Point(0,0),new Block() }
        });
        static Shape tDefaultShape = new Shape(new Dictionary<Point, Block> {
            { new Point(0,0),new Block() },
            { new Point(1,0),new Block() },
            { new Point(2,0),new Block() },
            { new Point(1,1),new Block() }
        });
        static Shape zDefaultShape = new Shape(new Dictionary<Point, Block> {
            { new Point(0,1),new Block() },
            { new Point(1,1),new Block() },
            { new Point(1,0),new Block() },
            { new Point(2,0),new Block() }
        });
        static Shape sDefaultShape = new Shape(new Dictionary<Point, Block> {
            { new Point(0,0),new Block() },
            { new Point(1,0),new Block() },
            { new Point(1,1),new Block() },
            { new Point(2,1),new Block() }
        });
        static Shape squareDefaultShape = new Shape(new Dictionary<Point, Block> {
            { new Point(0,0),new Block() },
            { new Point(1,0),new Block() },
            { new Point(1,1),new Block() },
            { new Point(0,1),new Block() }
        });
        #endregion

        #region Methods
        public Shape(Dictionary<Point,Block> points)
        {
            blocks = points;
        }
        public Shape()
        {
        }
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
            foreach (KeyValuePair<Point, Block> pair in blocks)
            {
                pair.Value.Update();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<Point, Block> pair in blocks)
            {
                pair.Value.Draw(spriteBatch);
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Properties
        public static Shape Long
        {
            get { return longDefaultShape; }
        }
        #endregion
    }
}
