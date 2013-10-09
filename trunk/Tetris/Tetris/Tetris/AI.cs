﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public static class AI
    {
        #region Fields
        static int xMoves;
        static int rotations;
        static int lastScore;
        static int bestScore = 0;
        static GhostShape ghostShape;
        #endregion

        #region Methods
        public static Tuple<int, int> Think(World world)
        {
            GhostShape ghostShape = new GhostShape(world);
            for (int xCheck = 0; xCheck <= gridWidth; xCheck++)
            {
                for (int rotCheck = 0; rotCheck <= 3; rotCheck++)
                {
                    lastScore = ghostShape.CalculateScore(xCheck, rotCheck);
                    if (lastScore > bestScore)
                    {
                        bestScore = lastScore;
                        xMoves = xCheck;
                        rotations = rotCheck;
                    }
                }
            }
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
