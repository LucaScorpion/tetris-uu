using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class World
    {
        #region Fields
        Dictionary<Point, Block> blocks = new Dictionary<Point, Block>();
        Shape currentShape = new Shape();

        int rows, columns;
        Rectangle rect = new Rectangle();

        #endregion

        #region Methods
        public void Update()
        {
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw world bg
            spriteBatch.Draw(Assets.DummyTexture, rect, Color.Black);

            //Draw blocks
            foreach (KeyValuePair<Point, Block> pair in blocks)
            {
                pair.Value.Draw(spriteBatch);
            }
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
        #endregion

        #region Constructors
        public World(int rows, int columns, Rectangle rect)
        {
            this.rect = rect;
            this.rows = rows;
            this.columns = columns;
        }
        #endregion

        #region Properties
        #endregion
    }
}
