using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TetrisGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SoundEffectInstance loop;
        SoundEffectInstance intro;

        public TetrisGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Assets
            Assets.init(GraphicsDevice);
            Assets.Fonts.BasicFont = Content.Load<SpriteFont>("Fonts/basicFont");
            Assets.Fonts.SmallerFont = Content.Load<SpriteFont>("Fonts/smallerFont");
            Assets.Fonts.GiantFont = Content.Load<SpriteFont>("Fonts/giantFont");

            Assets.Textures.Block = Content.Load<Texture2D>("Textures/block");
            Assets.Textures.Particle = Content.Load<Texture2D>("Textures/Particle");
            Assets.Textures.WorldBG = Content.Load<Texture2D>("Textures/WorldBG");
            Assets.Textures.MenuBG = Content.Load<Texture2D>("Textures/Background");
            Assets.Textures.Wow = Content.Load<Texture2D>("Textures/Wow");
            Assets.Textures.CloseEnough = Content.Load<Texture2D>("Textures/CloseEnough");
            Assets.Textures.FreddieMercury = Content.Load<Texture2D>("Textures/FreddieMercury");
            Assets.Textures.ItsSomething = Content.Load<Texture2D>("Textures/ItsSomething");
            Assets.Textures.ROFLcopter = Content.Load<Texture2D>("Textures/ROFLcopter");
            Assets.Textures.IELogo = Content.Load<Texture2D>("Textures/IELogo");
            Assets.Textures.RockHand = Content.Load<Texture2D>("Textures/RockHand");
            Assets.Textures.Focused = Content.Load<Texture2D>("Textures/Focused");
            Assets.Textures.PukingRainbows = Content.Load<Texture2D>("Textures/PukingRainbows");
            Assets.Textures.Lock = Content.Load<Texture2D>("Textures/Lock");
            Assets.Textures.ArrowLeft = Content.Load<Texture2D>("Textures/ArrowLeft");
            Assets.Textures.ArrowRight = Content.Load<Texture2D>("Textures/ArrowRight");
            Assets.Textures.ComboBreaker = Content.Load<Texture2D>("Textures/ComboBreaker");

            Assets.Audio.LockSound = Content.Load<SoundEffect>("Audio/LockSound");
            Assets.Audio.Single = Content.Load<SoundEffect>("Audio/Single");
            Assets.Audio.Double = Content.Load<SoundEffect>("Audio/Double");
            Assets.Audio.Triple = Content.Load<SoundEffect>("Audio/Triple");
            Assets.Audio.Tetris = Content.Load<SoundEffect>("Audio/Tetris");
            Assets.Audio.Loop = Content.Load<SoundEffect>("Audio/Loop");

            GameManager.BGParticleSB = new SpriteBatch(graphics.GraphicsDevice);
            GameManager.FGParticleSB = new SpriteBatch(graphics.GraphicsDevice);

            GameManager.Init(this.Exit);

            //Initialise the achiement class
            Achievement.Initialise(GraphicsDevice);
            //intro.Play();

            loop = Assets.Audio.Loop.CreateInstance();
            loop.IsLooped = true;
            loop.Play();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //update game
            GameManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Draw game
            GameManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
