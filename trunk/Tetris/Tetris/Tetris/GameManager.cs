using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    static public class GameManager
    {
        #region Fields
        static GameState currentGameState = GameState.StartScreen; //Default gamestate is startscreen
        static GameMode currentGameMode;
        static List<World> gameWorld = new List<World>();
        static GameTime gameTime;
        static Keys pauseKey = Keys.Escape;
        static Random random = new Random(1337);
        static Menu mainMenu = new Menu(new List<Button>());
        static Menu pausedMenu = new Menu(new List<Button>());
        static Menu gameOverMenu = new Menu(new List<Button>());
        public static SpriteBatch BGParticleSB;
        public static SpriteBatch FGParticleSB;
        static Emitter menuEmitter;
        //Achievements
        public static Achievement tetris, triple, doublec, single, roflcopter, slow;
        static List<Achievement> achievementList = new List<Achievement>();
        //The files to save stats and achievements to
        static string scoreFile = "stats.mesave";
        static string achievesFile = "achievements.mesave";

        public static World AIWorld;
        #endregion

        #region Methods
        //Initialise all the buttons and achievements
        public static void Init(Action quit)
        {
            mainMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(180, 330, 140, 50), Color.Transparent, Color.White * 0.3f, "Endless", Assets.Fonts.BasicFont, Color.White, StartSP),
            new Button(new Rectangle(330, 330, 140, 50), Color.Transparent, Color.White * 0.3f, "Battle mode", Assets.Fonts.BasicFont, Color.White, StartMP),
            new Button(new Rectangle(490, 330, 140, 50), Color.Transparent, Color.White * 0.3f, "Exit game", Assets.Fonts.BasicFont, Color.White, quit)
        });
            pausedMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(60, 80, 195, 50), Color.Black * 0.5f, Color.White * 0.3f, "Continue", Assets.Fonts.BasicFont, Color.White, Continue),
            new Button(new Rectangle(60, 150, 195, 50), Color.Black * 0.5f, Color.White * 0.3f, "Back to main menu", Assets.Fonts.BasicFont, Color.White, ToMenu)
        });
            gameOverMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(60, 150, 195, 50), Color.Black * 0.5f, Color.White * 0.3f, "Back to main menu", Assets.Fonts.BasicFont, Color.White, ToMenu)
        });
            //Create the menu emitter
            List<ParticleModifier> p = new List<ParticleModifier>();
            p.Add(new GravityModifier(new Vector2(0, -0.07f)));
            p.Add(new RandomSpeedModifier(new Vector2(0.1f, 0.1f)));
            menuEmitter = new Emitter(2, 0.5f, Color.Orange * 0.6f, Color.Red * 0.7f, 20, 1, new RandomSpawnSpeed(Vector2.Zero, Vector2.Zero), Assets.Textures.Particle, new RectangleSpawnShape(800, 0), p);
            menuEmitter.ForcePosition(new Vector2(400, 500));
            menuEmitter.Start();

            //Create achievements
            tetris = new Achievement("TETRIS!", "Cleared 4 rows", "at once", Assets.Textures.Wow, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            triple = new Achievement("Triple!", "Cleared 3 rows", "at once", Assets.Textures.CloseEnough, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            doublec = new Achievement("Double", "Cleared 2 rows", "at once", Assets.Textures.FreddieMercury, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            single = new Achievement("Single...", "Cleared 1 row", Assets.Textures.ItsSomething, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            roflcopter = new Achievement("ROFLCOPTER", "roflroflroflrofl", "roflroflroflrofl", "roflroflroflrofl", Assets.Textures.ROFLcopter, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            slow = new Achievement("So slow...", Assets.Textures.IELogo, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            //Add ALL of the achievements to achievementList
            achievementList.Add(tetris);
            achievementList.Add(triple);
            achievementList.Add(doublec);
            achievementList.Add(single);
            achievementList.Add(roflcopter);
            achievementList.Add(slow);
        }
        public static void Update(GameTime newGameTime)
        {
            //Update input
            InputState.update();

            gameTime = newGameTime;

            switch (currentGameState)
            {
                case GameState.Playing:
                    //Update world
                    foreach (World w in gameWorld)
                    {
                        w.Update();
                        //For singleplayer: if player is dead, game over
                        if (currentGameMode == GameMode.Singleplayer)
                            if (!w.IsAlive)
                            {
                                SaveStats();
                                currentGameState = GameState.GameOver;
                            }
                        //For multiplayer: if all human players are dead, game over
                        if (currentGameMode == GameMode.Multiplayer)
                        {
                            if (w.CurrentControlMode == ControlMode.Player && !w.IsAlive)
                            {
                                SaveStats();
                                currentGameState = GameState.GameOver;
                            }
                        }
                    }

                    //Update achievements
                    foreach (Achievement a in achievementList)
                        a.Update();

                    //Pause game if esc is pressed
                    if (InputState.isKeyPressed(pauseKey))
                        currentGameState = GameState.Paused;
                    break;
                case GameState.Menu:
                    menuEmitter.Update();
                    mainMenu.Update();
                    break;
                case GameState.Paused:
                    //Unpause game if esc is pressed
                    if (InputState.isKeyPressed(pauseKey))
                        currentGameState = GameState.Playing;
                    pausedMenu.Update();
                    break;
                case GameState.StartScreen:
                    currentGameState = GameState.Menu;
                    break;
                case GameState.GameOver:
                    foreach (World w in gameWorld)
                        w.Update();
                    gameOverMenu.Update();
                    break;
            }
        }
        public static void Draw(SpriteBatch s)
        {
            s.Begin();
            BGParticleSB.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            FGParticleSB.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            switch (currentGameState)
            {
                case GameState.Playing:
                    //Draw world
                    foreach (World w in gameWorld)
                        w.Draw(s);
                    //Draw achievements
                    foreach (Achievement a in achievementList)
                        a.Draw(s);
                    break;
                case GameState.Menu:
                    s.Draw(Assets.Textures.MenuBG, new Rectangle(0, 0, Assets.Textures.MenuBG.Width, Assets.Textures.MenuBG.Height), Color.White);
                    menuEmitter.Draw(FGParticleSB);
                    mainMenu.Draw(s);
                    break;
                case GameState.Paused:
                    //Draw world
                    foreach (World w in gameWorld)
                        w.Draw(s);
                    //Draw menu
                    pausedMenu.Draw(s);
                    break;
                case GameState.StartScreen:
                    break;
                case GameState.GameOver:
                    foreach (World w in gameWorld)
                        w.Draw(s);
                    gameOverMenu.Draw(s);
                    break;
            }
            BGParticleSB.End();
            s.End();
            FGParticleSB.End();
        }
        static void StartSP()
        {
            //Load the achievements
            LoadAchieves();

            gameWorld = new List<World>();

            //Set gamemode
            currentGameMode = GameMode.Singleplayer;
            //Load world
            GameWorld.Add(new World(new Rectangle(50, 70, 216, 360), 0, ControlMode.Player, false));
            //Change gamestate
            currentGameState = GameState.Playing;
        }
        static void StartMP()
        {
            LoadAchieves(); 

            gameWorld = new List<World>();

            //Set gamemode
            currentGameMode = GameMode.Multiplayer;
            //Load test worlds
            GameWorld.Add(new World(new Rectangle(50, 70, 216, 360), 0, ControlMode.Player, false));
            AIWorld = new World(new Rectangle(400, 70, 216, 360), 0, ControlMode.AI);
            GameWorld.Add(AIWorld);
            /*GameWorld.Add(new World(new Rectangle(400, 245, 110, 190), 0, ControlMode.AI));
            GameWorld.Add(new World(new Rectangle(550, 245, 110, 190), 0, ControlMode.AI));
            GameWorld.Add(new World(new Rectangle(550, 25, 110, 190), 0, ControlMode.AI));*/
            //Change gamestate
            currentGameState = GameState.Playing;
        }
        static void Continue()
        {
            //Continue the game
            currentGameState = GameState.Playing;
        }
        static void ToMenu()
        {
            //Go to the main menu
            currentGameState = GameState.Menu;
            //Save the stats and achievements
            SaveStats();
        }
        static void SaveStats()
        {
            string text = String.Empty;

            //Save score of every player
            foreach (World w in gameWorld)
            {
                if (w.CurrentControlMode == ControlMode.Player)
                {
                    //Add line: Score,<score>,<lines>
                    text += "Score," + w.Stats.Score + "," + w.Stats.LinesCleared + ";";
                }
            }

            //Append new stats to the file
            System.IO.File.AppendAllText(scoreFile, text);

            //Save achievements
            string achieves = String.Empty;
            foreach (Achievement a in achievementList)
            {
                if (a.Achieved)
                {
                    achieves += a.Name + ";";
                }
            }

            //Write Achievements to file
            System.IO.File.WriteAllText(achievesFile, achieves);
        }
        static void LoadAchieves()
        {
            //If the achievesFile exists...
            if (System.IO.File.Exists(achievesFile))
            {
                //Write the achievements, split by semicolons
                string achieves = System.IO.File.ReadAllText(achievesFile);
                string[] lines = achieves.Split(';');
                foreach (String l in lines)
                {
                    //Write every achievement name
                    foreach (Achievement a in achievementList)
                    {
                        if (a.Name == l)
                            a.Achieved = true;
                    }
                }
            }
        }
        #endregion

        #region Properties
        public static List<World> GameWorld { get { return gameWorld; } set { gameWorld = value; } }
        public static GameTime GameTime { get { return gameTime; } }
        public static Random Random { get { return random; } }
        public static Menu MainMenu { get { return mainMenu; } }
        public static GameState CurrentGameState { get { return currentGameState; } }
        public static GameMode CurrentGameMode { get { return currentGameMode; } }
        #endregion
    }
    public enum GameState
    {
        StartScreen, Playing, Menu, GameOver, Paused
    }
    public enum GameMode
    {
        Singleplayer, Multiplayer
    }
}
