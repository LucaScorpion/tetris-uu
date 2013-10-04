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
        int maxMultiplier = 3;
        Vector2 positionSP = new Vector2(340, 70);
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
                if (multiplier <= maxMultiplier)
                    multiplier++;
            }
            if (fullRows == 3)
            {
                score += 60;
                fullRows -= 3;
                multiplier = 1;
            }
            if (fullRows == 2)
            {
                score += 30;
                fullRows -= 2;
                multiplier = 1;
            }
            if (fullRows == 1)
            {
                score += 10;
                fullRows -= 1;
                multiplier = 1;
            }
        }
        public void Draw(SpriteBatch s)
        {
            if (GameManager.CurrentGameMode == GameMode.Singleplayer)
            {
                //Draw the text
                s.DrawString(font, "Lines cleared:", positionSP, textColor);
                s.DrawString(font, "Total score:", positionSP + new Vector2(0, Assets.Fonts.BasicFont.MeasureString("Lines cleared").Y * 2), textColor);
                //Draw the numbers
                s.DrawString(font, linesCleared.ToString(), positionSP + new Vector2(200, 0), textColor);
                s.DrawString(font, score.ToString(), positionSP + new Vector2(200, Assets.Fonts.BasicFont.MeasureString("Lines cleared").Y * 2), textColor);
            }
        }
        #endregion

        #region Constructors
        public Stats(SpriteFont font, Color textColor)
        {
            this.font = font;
            this.textColor = textColor;
        }
        #endregion

        #region Properties
        public int Combo
        {
            get { return multiplier; }
        }
        #endregion
    }
}
