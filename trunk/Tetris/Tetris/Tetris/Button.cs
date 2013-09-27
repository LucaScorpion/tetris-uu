using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    class Button
    {
        #region Fields

        #endregion

        #region Methods
        public void DrawButton(SpriteBatch s, int width, int height, Vector2 pos, String text, Color color)
        {
            s.Draw(new Texture2D(s.GraphicsDevice, 1, 1), new Rectangle((int)pos.X, (int)pos.Y, width, height), color);
        }
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion
    }
}
