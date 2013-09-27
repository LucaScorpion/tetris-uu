using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Block
    {
        #region Fields
        Texture2D texture = Assets.Textures.DummyTexture;
        Color color;
        #endregion

        #region Methods
        public void Draw(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(texture, rect, color);
        }
        #endregion

        #region Constructors
        public Block()
        {
            texture = Assets.Textures.Block;
            color = Color.White;
        }
        #endregion

        #region Properties
        public Texture2D Texture { get { return texture; } }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        #endregion
    }
}
