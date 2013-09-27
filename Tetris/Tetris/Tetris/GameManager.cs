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

        static World gameWorld;
        static GameTime gameTime;
        #endregion

        #region Methods
        public static void Update(GameTime newGameTime)
        {
            gameTime = newGameTime;

            switch (currentGameState)
            {
                case GameState.Playing:
                    gameWorld.Update();
                    break;
                case GameState.Menu:
                    break;
                case GameState.Paused:
                    break;
                case GameState.StartScreen:
                    //There is no startscreen yet, so set gamestate to playing
                    currentGameState = GameState.Playing;
                    break;
                case GameState.GameOver:
                    break;
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState)
            {
                case GameState.Playing:
                    spriteBatch.Begin();
                    gameWorld.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Menu:
                    break;
                case GameState.Paused:
                    break;
                case GameState.StartScreen:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
        #endregion

        #region Properties
        public static World GameWorld { get { return gameWorld; } set { gameWorld = value; } }
        public static GameTime GameTime { get { return GameTime; } }
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
