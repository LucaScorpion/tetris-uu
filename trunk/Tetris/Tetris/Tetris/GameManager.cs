using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class GameManager
    {
        #region Fields
        GameState currentGameState = GameState.StartScreen; //Default gamestate is startscreen

        World gameWorld;
        #endregion

        #region Methods
        public void Update()
        {
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
        public void Draw(SpriteBatch spriteBatch)
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

        #region Constructors
        public GameManager()
        {
            gameWorld = new World(5, 5, new Rectangle(10, 10, 300, 300));
            gameWorld.AddBlock(new Block(), new Point(2, 2));
        }
        #endregion

        #region Properties
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
