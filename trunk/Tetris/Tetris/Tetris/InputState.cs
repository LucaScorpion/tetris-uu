using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// Keeps the up-to-date input data
    /// </summary>
    public static class InputState
    {
        #region Fields
        static KeyboardState currentKeyboard;
        static KeyboardState previousKeyboard;
        public static MouseState currentMouse;
        static MouseState previousMouse;
        #endregion

        #region Methods
        public static void update()
        {
            //Update Kayboard
            previousKeyboard = currentKeyboard;
            currentKeyboard = Keyboard.GetState();

            //Update Mouse
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
        }
        public static Boolean isKeyDown(Keys k)
        {
            if (currentKeyboard.IsKeyDown(k))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean isKeyUp(Keys k)
        {
            if (currentKeyboard.IsKeyUp(k))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean isKeyPressed(Keys k)
        {
            if (currentKeyboard.IsKeyDown(k) && previousKeyboard.IsKeyUp(k))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Boolean leftClick()
        {
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }
        public static Boolean rightClick()
        {
            if (currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Properties
        #endregion
    }
}