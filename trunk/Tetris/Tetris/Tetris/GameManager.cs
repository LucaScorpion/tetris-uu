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
        static List<World> gameWorld = new List<World>();
        static GameTime gameTime;
        static Random random = new Random(7331);
        static Button button = new Button(new Rectangle(0, 0, 100, 50), Color.White, Color.LightBlue, "Start game", Assets.Fonts.BasicFont, Color.Black, Clicked);
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
                    break;
                case GameState.Paused:
                    break;
                case GameState.StartScreen:
                    button.Update();
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
                    break;
                case GameState.Menu:
                    break;
                case GameState.Paused:
                    break;
                case GameState.StartScreen:
                    button.Draw(s);
                    break;
                case GameState.GameOver:
                    break;
            }
            s.End();
        }
        static void Clicked()
        {
            currentGameState = GameState.Playing;
        }
        #endregion

        #region Properties
        public static List<World> GameWorld { get { return gameWorld; } set { gameWorld = value; } }
        public static GameTime GameTime { get { return gameTime; } }
        public static Random Random { get { return random; } }
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
