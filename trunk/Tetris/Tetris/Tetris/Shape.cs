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
        protected Point location;
        protected Vector2 gridCenter;
        int timeSinceMove = 0;
        double startMoveSpeedDown = 4; //In blocks per sec
        double moveSpeedDown;
        int moveSpeedBoost = 7; //Factor of speed boost when boosting down
        ControlMode controlMode = ControlMode.AI;
        Tuple<int, int> moves;
        int xMoves, rotations;
        int helicopterMoves = 0;
        bool AIthought = false;
        bool slow = true;

        //Controls
        Keys down = Keys.Down;
        Keys left = Keys.Left;
        Keys right = Keys.Right;
        Keys rotateLeft = Keys.X;
        Keys drop = Keys.Up;

        Shadow shadow;
        World world;
        Color color;
        #endregion

        #region Methods
        public virtual void Update()
        {
            //Add time
            timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds;

            //Set moveSpeedDown based on level
            moveSpeedDown = startMoveSpeedDown + 0.5 * GameManager.Level;

            //Controlled by player
            if (controlMode == ControlMode.Player)
            {
                //Add time
                if (InputState.isKeyDown(down))
                {
                    //Complete boost for keypress
                    timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds * (moveSpeedBoost - 1);
                    slow = false;
                }

                //Move left
                if (InputState.isKeyPressed(left))
                {
                    if (CanMove(new Point(-1, 0), grid))
                    {
                        location.X -= 1;
                    }
                }

                //Move right
                if (InputState.isKeyPressed(right))
                {
                    if (CanMove(new Point(1, 0), grid))
                    {
                        location.X += 1;
                    }
                }

                //Rotate
                if (InputState.isKeyPressed(rotateLeft))
                {
                    RotateLeft();
                    //Check for helicopter rotation (aka infinity lock) withing 250 ms
                    if (timeSinceMove < 250)
                        helicopterMoves++;
                    else
                        helicopterMoves = 0;
                    //Do 10 rotations for the achievement
                    if (helicopterMoves >= 10)
                        GameManager.roflcopter.GetAchievement();
                    //Infinity lock
                    timeSinceMove = 0;
                }
                //Hard drop
                if (InputState.isKeyPressed(drop))
                {
                    Harddrop();
                }
            }

            //Controlled by AI
            if (controlMode == ControlMode.AI)
            {
                //If the AI hasn't thought yet, think
                if (!AIthought)
                {
                    moves = AI.Think(world, this);
                    xMoves = moves.Item1;
                    rotations = moves.Item2;
                    AIthought = true;
                }
                //Move horizontally
                if (xMoves > 0)
                {
                    //Move right
                    if (CanMove(new Point(1, 0), grid))
                    {
                        location.X++;
                    }
                    xMoves--;
                }
                if (xMoves < 0)
                {
                    //Move left
                    if (CanMove(new Point(-1, 0), grid))
                    {
                        location.X--;
                    }
                    xMoves++;
                }
                //Rotate
                if (rotations > 0)
                {
                    RotateLeft();
                    rotations--;
                }
                //Move down
                if (rotations == 0 && xMoves == 0)
                {
                    //Speed boost
                    timeSinceMove += GameManager.GameTime.ElapsedGameTime.Milliseconds * (moveSpeedBoost - 1);
                }
            }

            //Move down
            if (timeSinceMove >= 1000 / moveSpeedDown)
            {
                if (CanMove(new Point(0, 1), grid))
                {
                    location.Y += 1;
                }
                else
                {
                    //Can't move down
                    MoveToWorld();
                    //Reset AI
                    if (controlMode == ControlMode.AI)
                        AIthought = false;
                    //If hard drop and boost down weren't used, get the slow achievement (SP only)
                    if (slow && GameManager.CurrentGameMode == GameMode.Singleplayer)
                        GameManager.slow.GetAchievement();
                }
                timeSinceMove = 0;
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
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

            if (controlMode == ControlMode.Player)
            {
                shadow.Draw(spriteBatch);
            }
        }
        public void Harddrop()
        {
            //Keep moving down
            while (CanMove(new Point(0, 1), grid))
            {
                location.Y += 1;
            }
            //Move shape to world 
            if(shadow != null)
                MoveToWorld();
            slow = false;
        }
        Point GetWorldLocation(int x, int y)
        {
            return new Point((int)(x - (int)gridCenter.X + location.X), (int)(y - (int)gridCenter.Y + location.Y));
        }
        void MoveToWorld()
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
            world.CurrentShape = null;
            //destroy all full rows
            world.DestroyFullRows();
        }
        bool CanMove(Point direction, Block[,] grid)
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
        void RotateLeft()
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
            if (CanMove(Point.Zero, newGrid))
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
                if (CanMove(Point.Zero, newGrid))
                {
                    grid = newGrid;
                }
            }
        }
        #endregion

        #region Constructors
        public Shape(World world, ControlMode controlMode)
        {
            //Select random shape
            switch (GameManager.Random.Next(0, 7))
            {
                case 0:
                    grid = IShape;
                    Color = Color.Red;
                    break;
                case 1:
                    grid = LShape;
                    Color = Color.Blue;
                    break;
                case 2:
                    grid = JShape;
                    Color = Color.Yellow;
                    break;
                case 3:
                    grid = ZShape;
                    Color = Color.Green;
                    break;
                case 4:
                    grid = SShape;
                    Color = Color.Purple;
                    break;
                case 5:
                    grid = TShape;
                    Color = Color.Orange;
                    break;
                case 6:
                    grid = OShape;
                    Color = Color.White;
                    break;
            }

            
            gridCenter = new Vector2(grid.GetLength(0) - 1, grid.GetLength(1) - 1) / 2;
            location = new Point(world.Columns / 2 - 1, (int)gridCenter.Y);
            this.controlMode = controlMode;
            this.world = world;

            //If can't spawn. kill world
            if (!CanMove(new Point(0, 0), grid))
            {
                world.Kill();
            }

            if (controlMode == ControlMode.Player)
                shadow = new Shadow(world, this);
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
            get { return color; }
            set
            {
                foreach (Block b in grid)
                {
                    if (b != null)
                    {
                        b.Color = value;
                    }
                }
                color = value;
            }
        }
        public ControlMode ControlMode { get { return controlMode; } set { controlMode = value; } }
        public Block[,] Grid { get { return grid; } set { grid = value; } }
        public Point Location { get { return location; } }
        #endregion
    }
    public enum ControlMode
    {
        Player, AI, None
    }
    public class Shadow : Shape
    {
        Shape shape;
        public Shadow(World world, Shape shape)
            : base(world, ControlMode.None)
        {
            this.shape = shape;
        }
        public override void Update()
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            this.Grid = new Block[shape.Grid.GetLength(0), shape.Grid.GetLength(1)];
            //Clone the grid
            for (int y = 0; y < shape.Grid.GetLength(1); y++)
            {
                for (int x = 0; x < shape.Grid.GetLength(0); x++)
                {
                    if (shape.Grid[x, y] != null)
                    {
                        this.Grid[x, y] = new Block();
                    }
                }
            }
            this.Color = shape.Color * 0.5f;
            this.location = shape.Location;
            gridCenter = new Vector2(Grid.GetLength(0) - 1, Grid.GetLength(1) - 1) / 2;
            Harddrop();

            base.Draw(spriteBatch);
        }
    }
}
