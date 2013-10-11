using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public static class AI
    {
        #region Fields
        static int gridWidth;
        static int gridHeight;
        static int lastScore;
        static int bestScore;
        static int xMoves;
        static int rotations;
        #endregion

        #region Methods
        public static Tuple<int, int> Think(World world, Shape shape)
        {
            gridWidth = world.Columns;
            gridHeight = world.Rows;
            lastScore = 0;
            bestScore = 0;
            xMoves = 0;
            rotations = 0;

            //Check each possible space and rotation
            for (int xCheck = -(gridWidth / 2); xCheck <= gridWidth / 2; xCheck++)
            {
                for (int rotCheck = 0; rotCheck <= 3; rotCheck++)
                {
                    //Create a new GhostShape
                    GhostShape ghostShape = new GhostShape(world, shape);
                    //Calculate the score
                    lastScore = ghostShape.CalculateScore(xCheck, rotCheck);
                    //Remember the highest score, and that movement and rotation
                    if (lastScore > bestScore)
                    {
                        bestScore = lastScore;
                        xMoves = xCheck;
                        rotations = rotCheck;
                    }
                    lastScore = 0;
                }
            }
            //Return the moves belonging to the best score
            var moves = Tuple.Create(xMoves, rotations);
            return moves;
        }
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion
    }
}
