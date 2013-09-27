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
        #endregion

        #region Constructors
        public Button(SpriteBatch s, int width, int height, Vector2 pos, String text, Color color)
        {
            s.Draw(Assets.DummyTexture, new Rectangle((int)pos.X, (int)pos.Y, width, height), color);
        }
        #endregion

        #region Properties
        #endregion
    }
}
