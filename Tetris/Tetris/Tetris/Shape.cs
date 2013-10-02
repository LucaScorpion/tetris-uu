using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Shape
    {
        #region Fields
        Block[,] grid;
        Point location;
        Vector2 gridCenter;
        int timeSinceMove = 0;
        int moveSpeedDown = 4; //In blocks per sec
        int moveSpeedBoost = 7; //Factor of speed boost when boosting down
        ControlMode controlMode = ControlMode.AI;
        bool mute = true;

        //Controls
        Keys down = Keys.Down;
        Keys left = Keys.Left;
        Keys right = Keys.Right;
        Keys rotate = Keys.Up;
        #endregion

        #region Methods
        public void Update(World world)
        {
            //add time
            timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds;

            if (controlMode == ControlMode.Player)
            {
                //add time
                if (InputState.isKeyDown(down))
                {
                    //Complete boost for keypress
                    timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds * (moveSpeedBoost - 1);
                }

                //Move left
                if (InputState.isKeyPressed(left))
                {
                    if (CanMove(new Point(-1, 0), world, grid))
                    {
                        location.X -= 1;
                    }
                }

                //Move right
                if (InputState.isKeyPressed(right))
                {
                    if (CanMove(new Point(1, 0), world, grid))
                    {
                        location.X += 1;
                    }
                }

                //Rotate
                if (InputState.isKeyPressed(rotate))
                {
                    RotateLeft(world);
                    //Infinity lock
                    timeSinceMove = 0;
                }
            }

            //Move down
            if (timeSinceMove >= 1000 / moveSpeedDown)
            {
                if (CanMove(new Point(0, 1), world, grid))
                {
                    location.Y += 1;
                }
                else
                {
                    //Can't move down. 
                    MoveToWorld(world);
                    world.CurrentShape = new Shape(world, controlMode, mute);
                    
                    //Play lock sound
                    if(!mute)
                        Assets.Audio.LockSound.Play();
                }
                timeSinceMove = 0;
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

            //destroy all full rows
            world.DestroyFullRows();
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
                    newGrid[(int)(gridCenter.Y - y + gridCenter.X), (int)(x - gridCenter.X + gridCenter.Y)] = grid[x,y];
                }
            }
            //Check if rotation is possible
            if (CanMove(Point.Zero, world, newGrid))
            {
                grid = newGrid;
            }
            else
            {
                //Check for wallkick
                if (location.X - gridCenter.X <= 0)
                {
                    //Is kicking the left wall
                    location.X = (int)gridCenter.X;
                }
                else if (location.X + gridCenter.X >= world.Columns)
                {
                    //Is kicking the right wall
                    location.X = world.Columns - (int)gridCenter.X - 1;
                }

                //Check again
                if (CanMove(Point.Zero, world, newGrid))
                {
                    grid = newGrid;
                }
            }
        }
        #endregion

        #region Constructors
        public Shape(World world, ControlMode controlMode, bool mute)
        {
            //Select random shape
            switch (GameManager.Random.Next(0, 7))
            {
                case 0:
                    grid = IShape;
                    break;
                case 1:
                    grid = LShape;
                    break;
                case 2:
                    grid = JShape;
                    break;
                case 3:
                    grid = ZShape;
                    break;
                case 4:
                    grid = SShape;
                    break;
                case 5:
                    grid = TShape;
                    break;
                case 6:
                    grid = OShape;
                    break;
            }

            
            gridCenter = new Vector2(grid.GetLength(0) - 1, grid.GetLength(1) - 1) / 2;
            location = new Point(world.Columns / 2 - 1, (int)gridCenter.Y);
            this.controlMode = controlMode;

            //Random Color
            Color = new Color(255 * GameManager.Random.Next(0, 2), 255 * GameManager.Random.Next(0, 2), 255 * GameManager.Random.Next(0, 2));

            this.mute = mute;

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
        public ControlMode ControlMode { get { return controlMode; } set { controlMode = value; } }
        #endregion
    }
    public enum ControlMode
    {
        Player, AI
    }
}
