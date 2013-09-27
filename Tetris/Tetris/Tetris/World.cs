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
        int borderOffset = 10;

        int timeSinceMove = 0;
        int movementSpeed = 1; //in blocks per seccond
        #endregion

        #region Methods
        public void Update()
        {
            timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceMove >= 1000 * movementSpeed)
            {
                //currentShape.MoveDown();
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw world bg
            spriteBatch.Draw(Assets.DummyTexture, rect, Color.Black);

            //Draw blocks
            foreach (KeyValuePair<Point, Block> pair in blocks)
            {
                spriteBatch.Draw(pair.Value.Texture, CalculateBlockRectangle(pair.Key), Color.White);
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
        public void AddShape(Shape shape, Point point)
        {
            foreach(KeyValuePair<Point,Block> b in shape.Blocks)
            {
                blocks.Add(new Point(b.Key.X + point.X,b.Key.Y + point.Y), b.Value);
            }
        }
        public Rectangle CalculateBlockRectangle(Point point)
        {
            return new Rectangle((int)(point.X * (rect.Width - 2 * borderOffset) / columns) + rect.X + borderOffset, (int)(point.Y * (rect.Height - 2 * borderOffset) / rows) + rect.Y + borderOffset, (int)((rect.Width - 2 * borderOffset) / columns), (int)((rect.Height - 2 * borderOffset) / rows));
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
