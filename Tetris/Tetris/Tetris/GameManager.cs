using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    static public class GameManager
    {
        #region Fields
        static GameState currentGameState = GameState.StartScreen; //Default gamestate is startscreen
        static GameMode currentGameMode;
        static List<World> gameWorld = new List<World>();
        static GameTime gameTime;
        static Random random = new Random(1337);
        static Menu mainMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(350, 200, 100, 50), Color.White, Color.LightBlue, "Singleplayer", Assets.Fonts.BasicFont, Color.Black, StartSP),
            new Button(new Rectangle(350, 270, 100, 50), Color.White, Color.LightBlue, "Multiplayer", Assets.Fonts.BasicFont, Color.Black, StartMP)
            //Exit game button is added in Game1
        });
        #endregion

        #region Methods
        public static void Update(GameTime newGameTime)
        {
            //Update input
            InputState.update();

            gameTime = newGameTime;

            switch (currentGameState)
            {
                case GameState.Playing:
                    foreach (World w in gameWorld)
                    {
                        w.Update();
                    }
                    break;
                case GameState.Menu:
                    mainMenu.Update();
                    break;
                case GameState.Paused:
                    break;
                case GameState.StartScreen:
                    currentGameState = GameState.Menu;
                    break;
                case GameState.GameOver:
                    break;
            }
        }
        public static void Draw(SpriteBatch s)
        {
            s.Begin();
            switch (currentGameState)
            {
                case GameState.Playing:
                    foreach (World w in gameWorld)
                    {
                        w.Draw(s);
                    }
                    if (currentGameMode == GameMode.Singleplayer)
                    {
                        Stats.Draw(s);
                    }
                    break;
                case GameState.Menu:
                    mainMenu.Draw(s);
                    break;
                case GameState.Paused:
                    break;
                case GameState.StartScreen:
                    break;
                case GameState.GameOver:
                    break;
            }
            s.End();
        }
        static void StartSP()
        {
            //Set gamemode
            currentGameMode = GameMode.Singleplayer;
            //Clear stats
            Stats.ClearStats();
            //Load world
            GameManager.GameWorld.Add(new World(new Rectangle(20, 30, 260, 420), 10, ControlMode.Player, false));
            //Change gamestate
            currentGameState = GameState.Playing;
        }
        static void StartMP()
        {
            //Set gamemode
            currentGameMode = GameMode.Multiplayer;
            //Load test worlds
            GameManager.GameWorld.Add(new World(new Rectangle(20, 30, 260, 420), 10, ControlMode.Player, false));
            GameManager.GameWorld.Add(new World(new Rectangle(300, 25, 130, 210), 5, ControlMode.AI));
            GameManager.GameWorld.Add(new World(new Rectangle(300, 245, 130, 210), 5, ControlMode.AI));
            GameManager.GameWorld.Add(new World(new Rectangle(450, 245, 130, 210), 5, ControlMode.AI));
            GameManager.GameWorld.Add(new World(new Rectangle(450, 25, 130, 210), 5, ControlMode.AI));
            //Change gamestate
            currentGameState = GameState.Playing;
        }
        #endregion

        #region Properties
        public static List<World> GameWorld { get { return gameWorld; } set { gameWorld = value; } }
        public static GameTime GameTime { get { return gameTime; } }
        public static Random Random { get { return random; } }
        public static Menu MainMenu { get { return mainMenu; } }
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
