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
            if (fullRows == 4)
            {
                score += 100 * multiplier;
                fullRows -= 4;
                multiplier++;
                GameManager.tetris.GetAchievement();
            }
            if (fullRows == 3)
            {
                score += 60 * multiplier;
                fullRows -= 3;
                multiplier++;
                GameManager.triple.GetAchievement();
            }
            if (fullRows == 2)
            {
                score += 30 * multiplier;
                fullRows -= 2;
                multiplier++;
                GameManager.doublec.GetAchievement();
            }
            if (fullRows == 1)
            {
                score += 10;
                fullRows -= 1;
                multiplier++;
                GameManager.single.GetAchievement();
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
                Vector2 stringSize = font.MeasureString("Score");
                //Draw bg
                s.Draw(Assets.Textures.DummyTexture, new Rectangle(worldRect.Left, worldRect.Top - (int)stringSize.Y * 2, worldRect.Width, (int)stringSize.Y * 2), Color.Black);
                //Draw the score
                s.DrawString(font, "Score", new Vector2(worldRect.X + worldRect.Width / 2 - stringSize.X / 2, worldRect.Y - stringSize.Y * 2), textColor);
                s.DrawString(font, score.ToString(), new Vector2(worldRect.X + 5, worldRect.Y - stringSize.Y), textColor);
                s.DrawString(font, multiplier.ToString(), new Vector2(worldRect.Right  - font.MeasureString("xX").X - 5, worldRect.Y - stringSize.Y), textColor);
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
        public int Score { get { return score; } }
        public int LinesCleared { get { return linesCleared; } }
        #endregion
    }
}
