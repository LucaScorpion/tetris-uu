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
        bool muteShape = true;

        Emitter comboEmitter;
        Emitter epicComboEmitter;
        Emitter explosionEmitter;
        #endregion

        
        #region Methods
        public void Update()
        {
            //Update emitters
            comboEmitter.Update();
            epicComboEmitter.Update();
            explosionEmitter.Update();

            if (currentShape == null)
            {
                currentShape = new Shape(this, controlMode, muteShape);
            }

            if (isAlive)
            {
                //Update shape
                currentShape.Update(this);
            }

            //if Epic Combo
            //epicComboEmitter.Shoot();
            //else if Combo
            //comboEmitter.Shoot();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw bg
            spriteBatch.Draw(Assets.Textures.DummyTexture, rect, Color.White * 0.7f);
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
                currentShape.Draw(spriteBatch, this);
            }
            //Dark grey overlay when player is dead
            if (!isAlive)
                spriteBatch.Draw(Assets.Textures.DummyTexture, rect, Color.Black * .6f);


            explosionEmitter.Draw(GameManager.FGParticleSB);
            comboEmitter.Draw(GameManager.BGParticleSB);
            epicComboEmitter.Draw(GameManager.BGParticleSB);
        }
        List<int> GetFullRows()
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
        public void DestroyFullRows()
        {
            //Get all full rows
            List<int> fullRows = GetFullRows();

            //Calculate score in Stats
            Stats.CalculateScore(fullRows.Count());

            //Destroy them and move down all blocks above them
            for (int i = 0; i < fullRows.Count(); i++)
            {
                //move down grid
                for (int y = fullRows[i] + i; y > 0; y--)
                {
                    for (int x = 0; x < columns; x++)
                        grid[x, y] = grid[x, y - 1];
                }
                //Explode graphic
                explosionEmitter.ForcePosition(new Vector2(rect.Center.X, CalculateBlockRectangle(new Point(0, i)).Center.Y));
                explosionEmitter.Shoot();
            }

            //Remove row 0
            for (int x = 0; x < columns; x++)
                grid[x, 0] = null;
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
            this.muteShape = muteShape;

            this.currentShape = new Shape(this, controlMode, muteShape);


            List<ParticleModifier> p = new List<ParticleModifier>();
            p.Add(new GravityModifier(new Vector2(0,-0.5f)));
            p.Add(new RandomSpeedModifier(new Vector2(0.1f, 0.1f)));
            this.comboEmitter = new Emitter(rect.Width/100f, 0f, Color.Orange * 0.6f, Color.Red, 20, 1, new RandomSpawnSpeed(Vector2.Zero,Vector2.Zero), Assets.Textures.Particle, new RectangleSpawnShape(rect.Width,rect.Height), p);
            this.comboEmitter.Position = new Vector2(rect.Center.X, rect.Center.Y);

            this.epicComboEmitter = new Emitter(rect.Width / 90f, 0f, Color.Orange * 0.5f, Color.Blue, 20, 1.5f, new RandomSpawnSpeed(Vector2.Zero), Assets.Textures.Particle, new RectangleSpawnShape(rect.Width, rect.Height), p);
            this.epicComboEmitter.Position = new Vector2(rect.Center.X, rect.Center.Y);

            List<ParticleModifier> ep = new List<ParticleModifier>();
            p.Add(new RandomSpeedModifier(new Vector2(2, 1)));
            explosionEmitter = new Emitter(
                0f,
                7f,
                Color.Orange * 0.8f,
                Color.Red * 0.1f,
                80,
                0.5f,
                new RandomSpawnSpeed(new Vector2(12, 2)),
                Assets.Textures.Particle,
                new RectangleSpawnShape(80, 0),
                ep
            );
        }
        #endregion

        #region Properties
        public Shape CurrentShape { get { return currentShape; } set { currentShape = value; } }
        public int Rows { get { return rows; } set { if (value > 0) { rows = value; } } }
        public int Columns { get { return columns; } set { if (value > 0) { columns = value; } } }
        public Block[,] Grid { get { return grid; } }
        #endregion
    }
}