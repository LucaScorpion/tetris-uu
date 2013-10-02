using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Button
    {
        #region Fields
        Rectangle rect;
        Color buttonColor;
        Color hoverColor;
        String text;
        SpriteFont font;
        Color textColor;
        Texture2D texture;
        Action method;
        bool hover = false;
        #endregion

        #region Methods
        //Handle mouseclicks on the button
        public void Update()
        {
            if (InputState.currentMouse.X > rect.Left && InputState.currentMouse.X < rect.Right && InputState.currentMouse.Y > rect.Top && InputState.currentMouse.Y < rect.Bottom)
            {
                //Hover
                hover = true;
                if (InputState.leftClick())
                {
                    method.Invoke();
                }
            }
            else
                hover = false;
        }
        public void Draw(SpriteBatch s)
        {
            //Draw the button
            if (texture == null)
            {
                if (hover)
                    s.Draw(Assets.Textures.DummyTexture, rect, hoverColor);
                else
                    s.Draw(Assets.Textures.DummyTexture, rect, buttonColor);
            }
            else
            {
                s.Draw(texture, rect, Color.White);
            }
            //Draw the text on the button
            s.DrawString(font, text, new Vector2((int)(rect.Width / 2 - font.MeasureString(text).X / 2) + rect.Left, (int)(rect.Height / 2 - font.MeasureString(text).Y / 2) + rect.Top), textColor);
        }
        #endregion

        #region Constructors
        public Button(Rectangle rect, Color buttonColor, Color hoverColor, String text, SpriteFont font, Color textColor, Action method)
        {
            this.rect = rect;
            this.buttonColor = buttonColor;
            this.hoverColor = hoverColor;
            this.text = text;
            this.font = font;
            this.textColor = textColor;
            this.method = method;
        }
        public Button(Rectangle rect, Texture2D texture, String text, SpriteFont font, Color textColor, Action method)
        {
            this.rect = rect;
            this.texture = texture;
            this.text = text;
            this.font = font;
            this.textColor = textColor;
            this.method = method;
        }
        #endregion

        #region Properties
        #endregion
    }
}
