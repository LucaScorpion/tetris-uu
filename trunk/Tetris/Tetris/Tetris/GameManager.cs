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
        GameMode currentGameMode;


        #endregion

        #region Methods
        #endregion

        #region Constructors
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
