﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GhostShape
    {
        #region Fields
        World world;
        Block[,] grid;
        Point location;
        Vector2 gridCenter;
        int timeSinceMove = 0;
        int moveSpeedDown = 4; //In blocks per sec
        int emptyRows;
        int emptyRowsWeight = 1;
        int filledRows;
        int filledRowsWeight = 2;
        int score;
        #endregion

        #region Methods
        public void Update(World world)
        {
            this.world = world;
            
            //Add time
            timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds;

            //Move down
            if (timeSinceMove >= 1000 / moveSpeedDown)
            {
                if (CanMove(new Point(0, 1), world, grid))
                {
                    location.Y += 1;
                }
                else
                {
                    //Can't move down
                    MoveToWorld(world);
                }
                timeSinceMove = 0;
            }
        }
        
        //Calculate score (called by AI.cs)
        public int CalculateScore(int xMoves, int rotations)
        {
            Rotate(rotations);
            MoveRight(xMoves);
            MoveToWorld(world);
            filledRows = world.GetFilledRows();
            emptyRows = world.GetEmptyRows();
            score = filledRowsWeight * filledRows + emptyRowsWeight * emptyRows;
            return score;
        }

        //Movements used by CalculateScore
        //Move right
        void MoveRight(int xMoves)
        {
            while (CanMove(new Point(xMoves, 0), world, grid))
            {
                location.X += 1;
                xMoves--;
            }
        }
        //Rotate
        void Rotate(int rotations)
        {
            RotateLeft(world);
            //Infinity lock
            timeSinceMove = 0;
        }
        //Hard drop
        void HardDrop()
        {
            while (CanMove(new Point(0, 1), world, grid))
            {
                location.Y += 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] != null)
                    {
                        Rectangle blockRect = world.CalculateBlockRectangle(GetWorldLocation(x, y));

                        spriteBatch.Draw(grid[x, y].Texture, blockRect, grid[x, y].Color);
                    }
                }
            }
        }
        Point GetWorldLocation(int x, int y)
        {
            return new Point((int)(x - (int)gridCenter.X + location.X), (int)(y - (int)gridCenter.Y + location.Y));
        }
        void MoveToWorld(World world)
        {
            //Move shape to world
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[x, y] != null)
                    {
                        world.AddBlock(grid[x, y], GetWorldLocation(x, y));
                    }
                }
            }
        }
        void RemoveFromWorld(World world)
        {
            //Remove the shape from the world
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[x, y] != null)
                    {
                        //world.(grid[x, y], GetWorldLocation(x, y));
                    }
                }
            }
        }

        bool CanMove(Point direction, World world, Block[,] grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] != null)
                    {
                        //This check if this block from the grid can move down on the world grid.
                        Point worldLocation = GetWorldLocation(x, y);

                        //Check for outside of screen
                        if (worldLocation.Y + direction.Y >= world.Rows)
                        {
                            //Went out the bottom
                            return false;
                        }
                        if (worldLocation.X + direction.X >= world.Columns)
                        {
                            //Went out the right
                            return false;
                        }
                        if (worldLocation.X + direction.X < 0)
                        {
                            //Went out the left
                            return false;
                        }

                        //Check collision with world blocks
                        if (world.Grid[worldLocation.X + direction.X, worldLocation.Y + direction.Y] != null)
                        {
                            //There is a block here!
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        void RotateLeft(World world)
        {
            Block[,] newGrid = new Block[grid.GetLength(0), grid.GetLength(1)];

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    newGrid[(int)(gridCenter.Y - y + gridCenter.X), (int)(x - gridCenter.X + gridCenter.Y)] = grid[x, y];
                }
            }
            //Check if rotation is possible
            if (CanMove(Point.Zero, world, newGrid))
            {
                grid = newGrid;
            }
        }
        #endregion

        #region Constructors
        public GhostShape(World world)
        {
            gridCenter = new Vector2(grid.GetLength(0) - 1, grid.GetLength(1) - 1) / 2;
            location = new Point(world.Columns / 2 - 1, (int)gridCenter.Y);

            //If can't spawn. kill world
            if (!CanMove(new Point(0, 0), world, grid))
            {
                world.Kill();
            }
        }
        #endregion

        #region Properties
        //Default shapes
        public static Block[,] IShape
        {
            get { return new Block[4, 4] { { null, new Block(), null, null }, { null, new Block(), null, null }, { null, new Block(), null, null }, { null, new Block(), null, null } }; }
        }
        public static Block[,] LShape
        {
            get { return new Block[3, 3] { { null, new Block(), new Block() }, { null, new Block(), null }, { null, new Block(), null } }; }
        }
        public static Block[,] JShape
        {
            get { return new Block[3, 3] { { null, new Block(), null }, { null, new Block(), null }, { null, new Block(), new Block() } }; }
        }
        public static Block[,] ZShape
        {
            get { return new Block[3, 3] { { null, new Block(), null }, { null, new Block(), new Block() }, { null, null, new Block() } }; }
        }
        public static Block[,] SShape
        {
            get { return new Block[3, 3] { { null, null, new Block() }, { null, new Block(), new Block() }, { null, new Block(), null } }; }
        }
        public static Block[,] TShape
        {
            get { return new Block[3, 3] { { null, new Block(), null }, { null, new Block(), new Block() }, { null, new Block(), null } }; }
        }
        public static Block[,] OShape
        {
            get { return new Block[2, 2] { { new Block(), new Block() }, { new Block(), new Block() } }; }
        }

        //Properties
        public Color Color
        {
            get { return Color.White; }
            set
            {
                foreach (Block b in grid)
                {
                    if (b != null)
                    {
                        b.Color = value;
                    }
                }
            }
        }
        #endregion
    }
}
