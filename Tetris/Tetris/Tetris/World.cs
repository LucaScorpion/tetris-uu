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
        Shape currentShape;

        int rows, columns;
        Rectangle rect = new Rectangle();
        int borderOffset = 10;

        int timeSinceMove = 0;
        int movementSpeed = 5; //in blocks per seccond
        #endregion

        #region Methods
        public void Update()
        {
            timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceMove >= 1000 / movementSpeed)
            {
                if (!currentShape.MoveDown(this))
                {
                    //Impossible to move down, lock blocks
                    AddShape(currentShape);

                    //Create new shape
                    currentShape = new Shape(this);
                }
                timeSinceMove = 0;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw world bg
            spriteBatch.Draw(Assets.DummyTexture, rect, Color.LightBlue);

            //Draw shape
            foreach (KeyValuePair<Point, Block> pair in currentShape.Blocks)
            {
                spriteBatch.Draw(pair.Value.Texture, CalculateBlockRectangle(pair.Key), pair.Value.Color);
            }

            //Draw blocks
            foreach (KeyValuePair<Point, Block> pair in blocks)
            {
                spriteBatch.Draw(pair.Value.Texture, CalculateBlockRectangle(pair.Key), pair.Value.Color);
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
        public void AddShape(Shape shape)
        {
            foreach (KeyValuePair<Point, Block> b in shape.Blocks)
            {
                blocks.Add(b.Key, b.Value);
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
        public Shape CurrentShape { get { return currentShape; } set { currentShape = value; } }
        public Dictionary<Point, Block> Blocks { get { return blocks; } }
        public int Rows { get { return rows; } set { if (value > 0) { rows = value; } } }
        public int Columns { get { return columns; } set { if (value > 0) { columns = value; } } }
        #endregion
    }
}