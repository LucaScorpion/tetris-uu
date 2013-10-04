using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Stats
    {
        #region Fields
        int linesCleared;
        int score;
        int multiplier = 1;
        Rectangle worldRect;
        SpriteFont font;
        Color textColor;
        #endregion

        #region Methods
        public void ClearStats()
        {
            //Clears all stats
            linesCleared = 0;
            score = 0;
        }
        public void CalculateScore(int fullRows)
        {
            linesCleared += fullRows;
            while (fullRows >= 4)
            {
                score += 100 * multiplier;
                fullRows -= 4;
                multiplier++;
            }
            if (fullRows == 3)
            {
                score += 60 * multiplier;
                fullRows -= 3;
                multiplier++;
            }
            if (fullRows == 2)
            {
                score += 30 * multiplier;
                fullRows -= 2;
                multiplier++;
            }
            if (fullRows == 1)
            {
                score += 10;
                fullRows -= 1;
                multiplier++;
            }
        }
        public void Draw(SpriteBatch s)
        {
            //Singleplayer stats
            if (GameManager.CurrentGameMode == GameMode.Singleplayer)
            {
                //Draw the text
                s.DrawString(font, "Lines cleared:", new Vector2(worldRect.Right + 30, worldRect.Y), textColor);
                s.DrawString(font, "Multiplier:", new Vector2(worldRect.Right + 30, worldRect.Y + Assets.Fonts.BasicFont.MeasureString("Lines cleared:").Y * 2), textColor);
                s.DrawString(font, "Total score:", new Vector2(worldRect.Right + 30, worldRect.Y + Assets.Fonts.BasicFont.MeasureString("Lines cleared:").Y * 4), textColor);
                //Draw the numbers
                s.DrawString(font, linesCleared.ToString(), new Vector2(worldRect.Right + 230, worldRect.Y), textColor);
                s.DrawString(font, multiplier.ToString(), new Vector2(worldRect.Right + 230, worldRect.Y + Assets.Fonts.BasicFont.MeasureString("Lines cleared:").Y * 2), textColor);
                s.DrawString(font, score.ToString(), new Vector2(worldRect.Right + 230, worldRect.Y + Assets.Fonts.BasicFont.MeasureString("Lines cleared:").Y * 4), textColor);
            }
            if (GameManager.CurrentGameMode == GameMode.Multiplayer)
            {
                //Draw the text
                s.DrawString(font, "Score:", new Vector2(worldRect.X, worldRect.Y - font.MeasureString("Score:x").Y), textColor);
                s.DrawString(font, "x", new Vector2(worldRect.X + worldRect.Width / 2, worldRect.Y - font.MeasureString("Score:x").Y), textColor);
                //Draw the score
                s.DrawString(font, score.ToString(), new Vector2(worldRect.X + font.MeasureString("Score: ").X, worldRect.Y - font.MeasureString("Score:x").Y), textColor);
                s.DrawString(font, multiplier.ToString(), new Vector2(worldRect.X + worldRect.Width / 2  + font.MeasureString("x").X, worldRect.Y - font.MeasureString("Score:x").Y), textColor);
            }
        }
        #endregion

        #region Constructors
        public Stats(SpriteFont font, Color textColor, Rectangle worldRect)
        {
            this.font = font;
            this.textColor = textColor;
            this.worldRect = worldRect;
        }
        #endregion

        #region Properties
        public int Combo
        {
            get { return multiplier; }
            set { multiplier = value; }
        }
        #endregion
    }
}
