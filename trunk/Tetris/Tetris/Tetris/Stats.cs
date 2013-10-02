using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    class Stats
    {
        #region Fields
        static int linesCleared;
        static int score;
        #endregion

        #region Methods
        public static void ClearStats()
        {
            //Clears all stats
            linesCleared = 0;
            score = 0;
        }
        public static void CalculateScore(int fullRows)
        {
            linesCleared += fullRows;
            while (fullRows >= 4)
            {
                score += 100;
                fullRows -= 4;
            }
            if (fullRows == 3)
            {
                score += 60;
                fullRows -= 3;
            }
            if (fullRows == 2)
            {
                score += 30;
                fullRows -= 2;
            }
            if (fullRows == 1)
            {
                score += 10;
                fullRows -= 1;
            }
        }
        public static void Draw(SpriteBatch s)
        {
            //Draw the text
            s.DrawString(Assets.Fonts.BasicFont, "Lines cleared:", new Vector2(300, 30), Color.White);
            s.DrawString(Assets.Fonts.BasicFont, "Total score:", new Vector2(300, 30 + Assets.Fonts.BasicFont.MeasureString("Lines cleared").Y * 2), Color.White);
            //Draw the numbers
            s.DrawString(Assets.Fonts.BasicFont, linesCleared.ToString(), new Vector2(500, 30), Color.White);
            s.DrawString(Assets.Fonts.BasicFont, score.ToString(), new Vector2(500, 30 + Assets.Fonts.BasicFont.MeasureString("Lines cleared").Y * 2), Color.White);
        }
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion
    }
}
