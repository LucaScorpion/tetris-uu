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
        public static SpriteBatch BGParticleSB;
        public static SpriteBatch FGParticleSB;
        #endregion

        #region Methods
        public static void Init(Action quit)
        {
            mainMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(350, 200, 120, 50), Color.White, Color.LightBlue, "Singleplayer", Assets.Fonts.BasicFont, Color.Black, StartSP),
            new Button(new Rectangle(350, 270, 120, 50), Color.White, Color.LightBlue, "Multiplayer", Assets.Fonts.BasicFont, Color.Black, StartMP),
            new Button(new Rectangle(350, 340, 120, 50), Color.White, Color.LightBlue, "Exit game", Assets.Fonts.BasicFont, Color.Black, quit)
            //Exit game button is added in Game1
        });
            pausedMenu = new Menu(new List<Button>() {
            new Button(new Rectangle(60, 80, 180, 50), Color.White, Color.LightBlue, "Continue", Assets.Fonts.BasicFont, Color.Black, Continue),
            new Button(new Rectangle(60, 150, 180, 50), Color.White, Color.LightBlue, "Back to main menu", Assets.Fonts.BasicFont, Color.Black, ToMenu)
        });
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
                        w.Update();
                    //Pause game if esc is pressed
                    if (InputState.isKeyPressed(pauseKey))
                        currentGameState = GameState.Paused;
                    break;
                case GameState.Menu:
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
                    break;
                case GameState.Menu:
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
                    break;
            }
            BGParticleSB.End();
            s.End();
            FGParticleSB.End();
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
            //Load test worlds
            GameWorld.Add(new World(new Rectangle(50, 70, 216, 360), 0, ControlMode.Player, false));
            GameWorld.Add(new World(new Rectangle(400, 25, 110, 190), 0, ControlMode.AI));
            GameWorld.Add(new World(new Rectangle(400, 245, 110, 190), 0, ControlMode.AI));
            GameWorld.Add(new World(new Rectangle(550, 245, 110, 190), 0, ControlMode.AI));
            GameWorld.Add(new World(new Rectangle(550, 25, 110, 190), 0, ControlMode.AI));
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
