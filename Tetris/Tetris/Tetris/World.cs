using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class World
    {
        #region Fields
        static int rows = 20, columns = 12;
        Block[,] grid = new Block[columns, rows];

        Shape currentShape;
        ControlMode controlMode;

        Rectangle rect = new Rectangle();
        int borderOffset = 10;

        bool isAlive = true;
        bool mute = true;

        Emitter comboEmitter;
        Emitter epicComboEmitter;
        Emitter explosionEmitter;
        Emitter tetrisEmitter;

        Stats stats;
        #endregion
        
        #region Methods
        public void Update()
        {
            //Update emitters
            comboEmitter.Update();
            epicComboEmitter.Update();
            explosionEmitter.Update();
            tetrisEmitter.Update();

            if (currentShape == null)
            {
                currentShape = new Shape(this, controlMode);
                stats.Combo = 1;
            }

            if (isAlive)
            {
                //Update shape
                currentShape.Update(this);

                if (stats.Combo >= 1)
                {
                    comboEmitter.Shoot();
                }
                if (stats.Combo >= 2)
                {
                    epicComboEmitter.Shoot();
                }
            }

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw bg
            spriteBatch.Draw(Assets.Textures.WorldBG, rect, Color.White);
            //If the game is paused, only draw the background and not the blocks
            if (GameManager.CurrentGameState != GameState.Paused)
            {
                //Draw all blocks
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        if (grid[x, y] != null)
                        {
                            grid[x, y].Draw(spriteBatch, CalculateBlockRectangle(x, y));
                        }
                    }
                }
                //Draw shape
                if (currentShape != null)
                {
                    currentShape.Draw(spriteBatch, this);
                }
            }
            //Dark grey overlay when player is dead
            if (!isAlive)
                spriteBatch.Draw(Assets.Textures.DummyTexture, rect, Color.Black * .6f);

            //Draw stats
            stats.Draw(spriteBatch);

            explosionEmitter.Draw(spriteBatch);
            comboEmitter.Draw(GameManager.BGParticleSB);
            epicComboEmitter.Draw(GameManager.BGParticleSB);
            tetrisEmitter.Draw(GameManager.BGParticleSB);
        }
        public List<int> GetFullRows()
        {
            List<int> fullRows = new List<int>();
            for (int y = rows - 1; y >= 0; y--)
            {
                bool fullRow = true;
                for (int x = columns - 1; x >= 0 && fullRow; x--)
                {
                    if (grid[x, y] == null)
                        fullRow = false;
                }
                if (fullRow)
                    fullRows.Add(y);
            }
            return fullRows;
        }
        //Get empty rows, used by AI
        public int GetEmptyRows()
        {
            int emptyRows = 0;
            //Check each row
            for (int y = 0; y <= rows - 1; y++)
            {
                bool emptyRow = true;
                //Check each column for a block
                for (int x = columns - 1; x >= 0 && emptyRow; x--)
                {
                    if (grid[x, y] != null)
                        emptyRow = false;
                }
                if (emptyRow)
                    emptyRows++;
            }
            return emptyRows;
        }
        //Get gaps, used by AI
        public int GetGaps()
        {
            int gaps = 0;
            //Check each column
            for (int x = 0; x <= columns - 1; x++)
            {
                bool emptyBlock = false;
                //Check each row for an empty space with a block above it
                for (int y = rows - 1; y >= 0; y--)
                {
                    if (grid[x, y] == null)
                        emptyBlock = true;
                    if (grid[x, y] != null && emptyBlock)
                        gaps++;
                }
            }
            return gaps;
        }
        //Get holes, used by AI
        public int GetHoles()
        {
            int holes = 0;
            //Check each row, starting at the first empty row
            for (int y = GetEmptyRows(); y <= rows - 1; y++)
            {
                for (int x = 0; x <= columns - 1; x++)
                {
                    //If there is no block, check for blocks around it
                    if (grid[x, y] == null)
                    {
                        bool isHole = true;
                        //Check right
                        if (x + 1 <= columns - 1)
                            if (grid[x + 1, y] == null)
                                isHole = false;
                        //Check left
                        if (x - 1 >= 0)
                            if (grid[x - 1, y] == null)
                                isHole = false;
                        //Check top
                        if (y + 1 <= rows - 1)
                            if (grid[x, y + 1] == null)
                                isHole = false;
                        //Check bottom
                        if (y - 1 >= 0)
                            if (grid[x, y - 1] == null)
                                isHole = false;
                        if (isHole)
                            holes++;
                    }
                }
            }
            return holes;
        }
        //Get wells (holes 3 blocks or deeper), used by AI
        public int GetWells()
        {
            int wells = 0;
            for (int x = 0; x <= columns - 1; x++)
            {
                int holeDepth = 0;
                for (int y = rows - 1; y >= 0; y--)
                {
                    //Check if there is no block
                    if (grid[x, y] == null)
                    {
                        bool blockLeft = true;
                        bool blockRight = true;
                        //Check if there is a block to the left
                        if (x - 1 >= 0)
                            if (grid[x - 1, y] == null)
                                blockLeft = false;
                        //Check if there is a block to the right
                        if (x + 1 <= columns - 1)
                            if (grid[x + 1, y] == null)
                                blockRight = false;
                        //If there are blocks to the left and right, add 1 to holeDepth
                        if (blockLeft && blockRight)
                            holeDepth++;
                        //If the holeDepth is 3, add a well
                        if (holeDepth == 3)
                            wells++;
                    }
                    else
                        holeDepth = 0;
                }
            }
            return wells;
        }
        //Get delta height (bumpiness of the grid), used by AI
        public int GetDeltaHeight()
        {
            int totalDHeight = 0;
            int dHeight;
            int hLeft = 0;
            int hCurrent = 0;
            //Check each column
            for (int x = 0; x <= columns - 1; x++)
            {
                bool block = false;
                for (int y = 0; y <= rows - 1; y++)
                {
                    //If no block was found yet, and there is a block, set the current height
                    if (grid[x, y] != null && !block)
                    {
                        block = true;
                        hCurrent = rows - y;
                    }
                    //If no block was found and it is the bottom row, hCurrent is 0
                    if (y == rows - 1 && !block && grid[x, y] == null)
                        hCurrent = 0;
                }
                //For the most left column of the grid
                if (x == 0)
                    hLeft = hCurrent;
                dHeight = Math.Abs(hCurrent - hLeft);
                totalDHeight += dHeight;
                hLeft = hCurrent;
            }
            return totalDHeight;
        }
        public void DestroyFullRows()
        {

            //Get all full rows
            List<int> fullRows = GetFullRows();
            //Destroy them and move down all blocks above them
            for (int i = 0; i < fullRows.Count(); i++)
            {
                //Explode graphic
                for (int x = 0; x < columns; x++)
                {
                    Rectangle blockRect = CalculateBlockRectangle(new Point(x, fullRows[i]));
                    explosionEmitter.ForcePosition(new Vector2(blockRect.Center.X, blockRect.Center.Y));
                    explosionEmitter.SetColor(grid[x, fullRows[i] + i].Color);
                    explosionEmitter.Shoot();
                }

                //move down grid
                for (int y = fullRows[i] + i; y > 0; y--)
                {
                    for (int x = 0; x < columns; x++)
                        grid[x, y] = grid[x, y - 1];
                }
            }
            if (fullRows.Count() > 0)
            {
                //Remove row 0
                for (int x = 0; x < columns; x++)
                    grid[x, 0] = null;


                //Calculate the score
                stats.CalculateScore(fullRows.Count());
                //create new shape
                this.currentShape = new Shape(this, controlMode);

            }

            
            if (!mute)
            {
                //Play sound
                switch (fullRows.Count)
                {
                    case 0:
                        //Assets.Audio.LockSound.Play();
                        break;
                    case 1:
                        Assets.Audio.Single.Play();
                        break;
                    case 2:
                        Assets.Audio.Double.Play();
                        break;
                    case 3:
                        Assets.Audio.Triple.Play();
                        break;
                    case 4:
                        Assets.Audio.Tetris.Play();
                        break;
                }
            }
        }
        public Rectangle CalculateBlockRectangle(int x, int y)
        {
            return new Rectangle(rect.X + borderOffset + x * (rect.Width - 2 * borderOffset) / columns, rect.Y + borderOffset + y * (rect.Height - 2 * borderOffset) / rows, (rect.Width - 2 * borderOffset) / columns, (rect.Height - 2 * borderOffset) / rows);
        }
        public Rectangle CalculateBlockRectangle(Point p)
        {
            return CalculateBlockRectangle(p.X, p.Y);
        }
        public void AddBlock(Block block, Point location)
        {
            grid[location.X, location.Y] = block;
        }
        public void RemoveBlock(Block block, Point location)
        {
            grid[location.X, location.Y] = null;
        }
        public void Kill()
        {
            isAlive = false;
            comboEmitter.Pause();
            epicComboEmitter.Pause();
        }
        #endregion

        #region Constructors
        public World(Rectangle rect, int offset, ControlMode controlMode, bool muteShape = true)
        {
            this.rect = rect;
            this.borderOffset = offset;
            this.controlMode = controlMode;
            this.mute = muteShape;

            this.currentShape = new Shape(this, controlMode);

            //Combo emitters
            List<ParticleModifier> p = new List<ParticleModifier>();
            p.Add(new GravityModifier(new Vector2(0, -0.5f)));
            p.Add(new RandomSpeedModifier(new Vector2(0.1f, 0.1f)));
            this.comboEmitter = new Emitter(rect.Width / 100f, 0f, Color.Orange * 0.6f, Color.Red, 20, 1, new RandomSpawnSpeed(Vector2.Zero, Vector2.Zero), Assets.Textures.Particle, new RectangleSpawnShape(rect.Width, rect.Height), p);
            this.comboEmitter.Position = new Vector2(rect.Center.X, rect.Center.Y);

            List<ParticleModifier> cp = new List<ParticleModifier>();
            cp.Add(new GravityModifier(new Vector2(0, -0.5f)));
            cp.Add(new RandomSpeedModifier(new Vector2(1f, 1f)));
            this.epicComboEmitter = new Emitter(rect.Width / 90f, 0f, Color.Orange * 0.5f, Color.Blue, 20, 1.5f, new RandomSpawnSpeed(Vector2.Zero), Assets.Textures.Particle, new RectangleSpawnShape(rect.Width, rect.Height), cp);
            this.epicComboEmitter.Position = new Vector2(rect.Center.X, rect.Center.Y);


            //Get rect of one block for size
            Rectangle blockRect = CalculateBlockRectangle(0,0);
            List<ParticleModifier> ep = new List<ParticleModifier>();
            ep.Add(new GravityModifier(new Vector2(0, 0.3f)));
            explosionEmitter = new Emitter(
                (float)blockRect.Width / (float)Assets.Textures.Block.Width,
                (float)blockRect.Width / (float)Assets.Textures.Block.Width,
                Color.Red,
                Color.Red,
                1,
                2f,
                new RandomSpawnSpeed(new Vector2(12, -rect.Width / 30), new Vector2(-12, -rect.Width / 50)),
                Assets.Textures.Block,
                new RectangleSpawnShape(0, 0),
                ep
            );
            stats = new Stats(Assets.Fonts.BasicFont, Color.White, rect);

            //Tetris emitter
            tetrisEmitter = new Emitter(rect.Width / 100f, 0f, Color.Red * 0.4f, Color.Blue, 100, 0.5f, new RandomSpawnSpeed(new Vector2(-30,-9), new Vector2(30,-4)), Assets.Textures.Particle, new RectangleSpawnShape(rect.Width, 0), new List<ParticleModifier>());
            tetrisEmitter.Position = new Vector2(rect.Center.X, rect.Bottom);
        }
        #endregion

        #region Properties
        public Shape CurrentShape { get { return currentShape; } set { currentShape = value; } }
        public int Rows { get { return rows; } set { if (value > 0) { rows = value; } } }
        public int Columns { get { return columns; } set { if (value > 0) { columns = value; } } }
        public Block[,] Grid { get { return grid; } }
        public bool IsAlive { get { return isAlive; } }
        public ControlMode CurrentControlMode { get { return controlMode; } }
        public Stats Stats { get { return stats; } }
        #endregion
    }
}