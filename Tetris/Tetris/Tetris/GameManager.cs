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
        static Menu mainMenu, pausedMenu, gameOverMenu, mpGameOverMenu, achievementsMenu;
        public static SpriteBatch BGParticleSB, FGParticleSB;
        static Emitter menuEmitter;
        //Levels
        static int levelTime = 60; //Seconds per level
        static int level = 1;
        static int maxLevel = 10;
        static int lastLevelUp;
        //Achievements
        public static Achievement tetris, triple, doublec, single, roflcopter, slow, mpWon, cleared1, cleared2, combo, allInOne, doubleTetris, love, achievementWh0re, maxLevelReached;
        static List<Achievement> achievementList = new List<Achievement>();
        static int gotAchievements, lockedAchievements;
        static int scroll = 0;
        static int secondsPlayed = 0;
        static int prevTime;
        //The files to save stats and achievements to
        static string scoreFile = "stats.mesave";
        static string achievesFile = "achievements.mesave";

        static string winText = "You rock! \\m/";
        static string loseText = "You fail...";

        public static World AIWorld;
        #endregion

        #region Methods
        //Initialise all the buttons, menus and achievements
        public static void Init(Action quit)
        {
            BuildMenu(quit);

            menuEmitter.Start();

            //Create achievements
            tetris = new Achievement("TETRIS!", "Clear 4 rows", "at once.", Assets.Textures.Wow, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            triple = new Achievement("Triple!", "Clear 3 rows", "at once.", Assets.Textures.CloseEnough, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            doublec = new Achievement("Double", "Clear 2 rows", "at once.", Assets.Textures.FreddieMercury, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            single = new Achievement("Single...", "Clear 1 row.", Assets.Textures.ItsSomething, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            roflcopter = new Achievement("ROFLCOPTER", "roflroflroflrofl", "roflroflroflrofl", "roflroflroflrofl", Assets.Textures.ROFLcopter, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            slow = new Achievement("So slow...", "Don't use hard drop", "or boost down.", Assets.Textures.IELogo, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            mpWon = new Achievement("You rock!", "Beat the AI.", Assets.Textures.RockHand, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            cleared1 = new Achievement("Focused", "Clear 50 lines", "in 1 game.", Assets.Textures.Focused, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            cleared2 = new Achievement("In the zone", "Clear 100 lines", "in 1 game.", Assets.Textures.PukingRainbows, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            combo = new Achievement("Combobreaker", "Get a multiplier", "of 3.", Assets.Textures.ComboBreaker, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            allInOne = new Achievement("All in one", "Clear a single,", "double, triple and", "tetris in 1 game.", Assets.Textures.CountVonCount, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            doubleTetris = new Achievement("Back-to-back", "Clear 2 tetrises", "back to back.", Assets.Textures.AwwYea, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            love = new Achievement("I love this game!", "Play singleplayer", "for more then 10", "minutes in 1 session.", Assets.Textures.Heart, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            achievementWh0re = new Achievement("Chievies!", "Unlock all", "achievements.", Assets.Textures.AchievementWh0re, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            maxLevelReached = new Achievement("Hell yeah", "Reach the max", "level in singleplayer.", Assets.Textures.MaxLevel, Color.Gray, Color.White, Assets.Fonts.BasicFont, Assets.Fonts.SmallerFont);
            //Add ALL of the achievements to achievementList
            achievementList.Add(single);
            achievementList.Add(doublec);
            achievementList.Add(triple);
            achievementList.Add(tetris);
            achievementList.Add(roflcopter);
            achievementList.Add(slow);
            achievementList.Add(mpWon);
            achievementList.Add(combo);
            achievementList.Add(cleared1);
            achievementList.Add(cleared2);
            achievementList.Add(allInOne);
            achievementList.Add(doubleTetris);
            achievementList.Add(maxLevelReached);
            achievementList.Add(love);
            achievementList.Add(achievementWh0re);

            //Load all achievements
            LoadAchieves();
        }
        public static void Update(GameTime newGameTime)
        {
            //Update input
            InputState.update();
            //Update gametime
            gameTime = newGameTime;

            switch (currentGameState)
            {
                case GameState.Playing:
                    UpdateGame();
                    if (currentGameMode == GameMode.Singleplayer)
                    {
                        //Add time
                        if (prevTime != newGameTime.TotalGameTime.Seconds)
                        {
                            secondsPlayed++;
                            prevTime = newGameTime.TotalGameTime.Seconds;
                        }
                        //Get achievement
                        if (secondsPlayed == 600)
                            love.GetAchievement();
                        //Set level
                        if (secondsPlayed % levelTime == 0 && level <= maxLevel && lastLevelUp != secondsPlayed)
                        {
                            lastLevelUp = secondsPlayed;
                            level++;
                            if (level == maxLevel)
                                maxLevelReached.GetAchievement();
                        }
                    }
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
                case GameState.MPWon:
                    //Get the mpWon achievement
                    mpWon.GetAchievement();
                    //Update achievements
                    foreach (Achievement a in achievementList)
                        a.Update();
                    //Update particles
                    menuEmitter.Update();
                    //Update the menu
                    mpGameOverMenu.Update();
                    break;
                case GameState.MPLost:
                    //Update the menu
                    mpGameOverMenu.Update();
                    break;
                case GameState.Achievements:
                    gotAchievements = 0;
                    //Check each achievement
                    for (int n = 0; n < achievementList.Count(); n++)
                    {
                        Achievement a = achievementList.ElementAt(n);
                        //If the achievement is achieved
                        if (a.Achieved)
                            //Add 1 to got achievements
                            gotAchievements++;
                    }
                    //Calculate locked achievements
                    lockedAchievements = achievementList.Count() - gotAchievements;
                    //If locked achievements = 1, get the last achievement
                    if (lockedAchievements == 1)
                        achievementWh0re.GetAchievement();
                    //Update achievements
                    foreach (Achievement a in achievementList)
                        a.Update();
                    //Update the menu
                    achievementsMenu.Update();
                    break;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            BGParticleSB.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            FGParticleSB.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            switch (currentGameState)
            {
                case GameState.Playing:
                    //Draw world
                    foreach (World w in gameWorld)
                        w.Draw(spriteBatch);
                    //Draw achievements
                    foreach (Achievement a in achievementList)
                        a.Draw(spriteBatch);
                    break;
                case GameState.Menu:
                    spriteBatch.Draw(Assets.Textures.MenuBG, new Rectangle(0, 0, Assets.Textures.MenuBG.Width, Assets.Textures.MenuBG.Height), Color.White);
                    menuEmitter.Draw(FGParticleSB);
                    mainMenu.Draw(spriteBatch);
                    break;
                case GameState.Paused:
                    //Draw world
                    foreach (World w in gameWorld)
                        w.Draw(spriteBatch);
                    //Draw menu
                    pausedMenu.Draw(spriteBatch);
                    break;
                case GameState.StartScreen:
                    break;
                case GameState.GameOver:
                    foreach (World w in gameWorld)
                        w.Draw(spriteBatch);
                    gameOverMenu.Draw(spriteBatch);
                    break;
                case GameState.MPWon:
                    DrawMPWinscreen(spriteBatch);
                    break;
                case GameState.MPLost:
                    //Draw the menu
                    mpGameOverMenu.Draw(spriteBatch);
                    //Draw the text
                    spriteBatch.DrawString(Assets.Fonts.GiantFont, loseText, new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2 - Assets.Fonts.GiantFont.MeasureString(loseText).X / 2, 100), Color.White);
                    break;
                case GameState.Achievements:
                    //View achievements
                    ViewAchievements(spriteBatch);
                    //Draw achievements
                    foreach (Achievement a in achievementList)
                        a.Draw(spriteBatch);
                    //Draw the menu
                    achievementsMenu.Draw(spriteBatch);
                    break;
            }
            BGParticleSB.End();
            spriteBatch.End();
            FGParticleSB.End();
        }
        static void UpdateGame()
        {

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
                //For multiplayer: go to the MPLost or MPWon game over screen, depending on if the player or the AI died
                if (currentGameMode == GameMode.Multiplayer)
                {
                    if (w.CurrentControlMode == ControlMode.Player && !w.IsAlive)
                        currentGameState = GameState.MPLost;
                    if (w.CurrentControlMode == ControlMode.AI && !w.IsAlive)
                        currentGameState = GameState.MPWon;
                }
            }

            //Update achievements
            foreach (Achievement a in achievementList)
                a.Update();

            //Pause game if esc is pressed
            if (InputState.isKeyPressed(pauseKey))
                currentGameState = GameState.Paused;
        }
        static void DrawMPWinscreen(SpriteBatch spriteBatch)
        {
            foreach (World w in gameWorld)
                w.Draw(spriteBatch);

            //Draw achievements
            foreach (Achievement a in achievementList)
                a.Draw(spriteBatch);
            //Draw particles
            menuEmitter.Draw(FGParticleSB);
            //Draw the menu
            mpGameOverMenu.Draw(spriteBatch);
            //Draw the text
            spriteBatch.DrawString(Assets.Fonts.GiantFont, winText, new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2 - Assets.Fonts.GiantFont.MeasureString(winText).X / 2, 100), Color.White);
        }
        static void ViewAchievements(SpriteBatch spriteBatch)
        {
            //Draw the text
            spriteBatch.DrawString(Assets.Fonts.BasicFont, "Achievements", new Vector2(10, 0), Color.White);
            spriteBatch.DrawString(Assets.Fonts.BasicFont, "Got achievements:", new Vector2(10, Assets.Fonts.BasicFont.MeasureString("Achievements").Y), Color.White);
            spriteBatch.DrawString(Assets.Fonts.BasicFont, "Locked achievements:", new Vector2(10, Assets.Fonts.BasicFont.MeasureString("Achievements").Y + Assets.Fonts.BasicFont.MeasureString("Got achievements:").Y), Color.White);
            //Draw the numbers
            spriteBatch.DrawString(Assets.Fonts.BasicFont, gotAchievements.ToString(), new Vector2(250, Assets.Fonts.BasicFont.MeasureString("Achievements").Y), Color.White);
            spriteBatch.DrawString(Assets.Fonts.BasicFont, lockedAchievements.ToString(), new Vector2(250, Assets.Fonts.BasicFont.MeasureString("Achievements").Y + Assets.Fonts.BasicFont.MeasureString("Got achievements:").Y), Color.White);
            //Check and draw each achievement
            int cols = 0;
            int rows = 1;
            Vector2 pos = new Vector2();
            for (int n = 0; n < achievementList.Count(); n++)
            {
                Achievement a = achievementList.ElementAt(n);
                //Calculate the position
                pos = new Vector2(10 + (cols - scroll) * (a.Size.X + 10), 110 + (rows - 1) * (a.Size.Y + 10));
                
                //If the achievement is achieved, show the achievement, else show a locked achievement.
                if (a.Achieved)
                {
                    //View the achievement
                    a.View(spriteBatch, pos);
                }
                else
                {
                    a.ViewLocked(spriteBatch, pos, Assets.Textures.Lock, "Locked", true);
                }

                //Add a row
                rows++;
                //3 rows per column
                if (rows > 3)
                {
                    rows = 1;
                    cols++;
                }
            }
        }
        static void StartSP()
        {
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
            gameWorld = new List<World>();

            //Set gamemode
            currentGameMode = GameMode.Multiplayer;
            //Load worlds
            GameWorld.Add(new World(new Rectangle(50, 70, 240, 400), 0, ControlMode.Player, false));
            AIWorld = new World(new Rectangle(510, 70, 240, 400), 0, ControlMode.AI);
            GameWorld.Add(AIWorld);
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
        static void ToAchievements()
        {
            //Set the gamestate to Achievements
            currentGameState = GameState.Achievements;
        }
        static void ScrollLeft()
        {
            if (scroll < (achievementList.Count + 2) / 3 - 2)
                scroll++;
        }
        static void ScrollRight()
        {
            if (scroll > 0)
                scroll--;
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
        static void BuildMenu(Action quit)
        {
            mainMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(180, 330, 140, 50), Color.Transparent, Color.White * 0.3f, "Endless", Assets.Fonts.BasicFont, Color.White, StartSP),
            new Button(new Rectangle(330, 330, 140, 50), Color.Transparent, Color.White * 0.3f, "Battle mode", Assets.Fonts.BasicFont, Color.White, StartMP),
            new Button(new Rectangle(490, 330, 140, 50), Color.Transparent, Color.White * 0.3f, "Exit game", Assets.Fonts.BasicFont, Color.White, quit),
            new Button(new Rectangle(320, 400, 160, 50), Color.Transparent, Color.White * 0.3f, "Achievements", Assets.Fonts.BasicFont, Color.White, ToAchievements)
        });
            pausedMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(60, 80, 195, 50), Color.Black * 0.5f, Color.White * 0.3f, "Continue", Assets.Fonts.BasicFont, Color.White, Continue),
            new Button(new Rectangle(60, 150, 195, 50), Color.Black * 0.5f, Color.White * 0.3f, "Back to main menu", Assets.Fonts.BasicFont, Color.White, ToMenu)
        });
            gameOverMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(60, 150, 195, 50), Color.Black * 0.5f, Color.White * 0.3f, "Back to main menu", Assets.Fonts.BasicFont, Color.White, ToMenu)
        });
            mpGameOverMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(300, 330, 200, 50), Color.Transparent, Color.White * 0.3f, "Back to main menu", Assets.Fonts.BasicFont, Color.White, ToMenu)
        });
            achievementsMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(590, 10, 200, 50), Color.Transparent, Color.White * 0.3f, "Back to main menu", Assets.Fonts.BasicFont, Color.White, ToMenu),
            new Button(new Rectangle(0, 440, 80, 40), Assets.Textures.ArrowLeft, ScrollRight),
            new Button(new Rectangle(720, 440, 80, 40), Assets.Textures.ArrowRight, ScrollLeft)
        });
            //Create the menu emitter
            List<ParticleModifier> p = new List<ParticleModifier>();
            p.Add(new GravityModifier(new Vector2(0, -0.07f)));
            p.Add(new RandomSpeedModifier(new Vector2(0.1f, 0.1f)));
            menuEmitter = new Emitter(2, 0.5f, Color.Orange * 0.6f, Color.Red * 0.7f, 20, 1, new RandomSpawnSpeed(Vector2.Zero, Vector2.Zero), Assets.Textures.Particle, new RectangleSpawnShape(800, 0), p);
            menuEmitter.ForcePosition(new Vector2(400, 500));
        }
        #endregion

        #region Properties
        public static List<World> GameWorld { get { return gameWorld; } set { gameWorld = value; } }
        public static GameTime GameTime { get { return gameTime; } }
        public static Random Random { get { return random; } }
        public static Menu MainMenu { get { return mainMenu; } }
        public static GameState CurrentGameState { get { return currentGameState; } }
        public static GameMode CurrentGameMode { get { return currentGameMode; } }
        public static int Level { get { return level; } }
        #endregion
    }
    public enum GameState
    {
        StartScreen, Playing, Menu, GameOver, Paused, MPWon, MPLost, Achievements
    }
    public enum GameMode
    {
        Singleplayer, Multiplayer
    }
}
