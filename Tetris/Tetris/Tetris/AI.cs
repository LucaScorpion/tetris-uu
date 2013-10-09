﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public static class AI
    {
        #region Fields
        static int gridWidth;
        static int gridHeight;
        static int xMoves;
        static int rotations;
        static int lastScore;
        static int bestScore = 0;
        #endregion

        #region Methods
        public static Tuple<int, int> Think(World world, Shape shape)
        {
            GhostShape ghostShape = new GhostShape(world, shape);
            gridWidth = world.Columns;
            gridHeight = world.Rows;
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
