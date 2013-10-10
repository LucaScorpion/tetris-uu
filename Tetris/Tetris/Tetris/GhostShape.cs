using System;
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
        Shape shape;
        Block[,] grid;
        Point location;
        Vector2 gridCenter;
        int emptyRows;
        float emptyRowsWeight = 1;
        int filledRows;
        float filledRowsWeight = 2;
        int gaps;
        float gapWeight = -3;
        int score;
        #endregion

        #region Methods        
        //Calculate score (called by AI.cs)
        public int CalculateScore(int xMoves, int rotations)
        {
            Rotate(rotations);
            MoveHoriz(xMoves);
            HardDrop();
            MoveToWorld(world);
            filledRows = world.GetFullRows().Count();
            emptyRows = world.GetEmptyRows();
            gaps = world.GetGaps();
            RemoveFromWorld(world);
            score = (int)(filledRowsWeight * filledRows + emptyRowsWeight * (emptyRows + filledRows) + gaps * gapWeight );
            return score;
        }

        //Movements used by CalculateScore
        void MoveHoriz(int xMoves)
        {
            //Move left
            while (CanMove(new Point(-1, 0), world, grid) && xMoves < 0)
            {
                location.X -= 1;
                xMoves += 1;
            }
            //Move right
            while (CanMove(new Point(1, 0), world, grid) && xMoves > 0)
            {
                location.X += 1;
                xMoves -= 1;
            }
        }
        //Rotate
        void Rotate(int rotations)
        {
            if (rotations > 0)
            {
                RotateLeft(world);
                rotations -= 1;
                Rotate(rotations);
            }
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
                        world.RemoveBlock(grid[x, y], GetWorldLocation(x, y));
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
        public GhostShape(World world, Shape shape)
        {
            this.world = world;
            this.shape = shape;
            this.grid = shape.Grid;
            gridCenter = new Vector2(grid.GetLength(0) - 1, grid.GetLength(1) - 1) / 2;
            location = new Point(world.Columns / 2 - 1, (int)gridCenter.Y);
            //If can't spawn. kill world
            if (!CanMove(new Point(0, 0), world, shape.Grid))
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
