using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    class Block
    {
        #region Fields
        Rectangle rect;
        Texture2D texture;
        Color color;
        #endregion

        #region Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, color);
        }
        public void Update()
        {

        }
        #endregion

        #region Constructors
        public Block()
        {

        }
        #endregion

        #region Properties
        #endregion
    }
}
