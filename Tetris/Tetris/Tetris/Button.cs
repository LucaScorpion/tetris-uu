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
        public Button(SpriteBatch s, int width, int height, Vector2 position, Color buttonColor, String text, SpriteFont font, Color textColor)
        {
            //Draw the button
            s.Draw(Assets.DummyTexture, new Rectangle((int)position.X, (int)position.Y, width, height), buttonColor);
            //Draw the text on the button
            s.DrawString(font, text, new Vector2((int)(width / 2 - font.MeasureString(text).X / 2) + position.X, (int)(height / 2 - font.MeasureString(text).Y / 2) + position.Y), textColor);
        }
        #endregion

        #region Properties
        #endregion
    }
}
