using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Menu
    {
        #region Fields
        List<Button> buttonList;
        #endregion

        #region Methods
        public void AddButton(Button button)
        {
            buttonList.Add(button);
        }
        public void Update()
        {
            foreach (Button b in buttonList)
            {
                b.Update();
            }
        }
        public void Draw(SpriteBatch s)
        {
            foreach (Button b in buttonList)
            {
                b.Draw(s);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new menu with a list of buttons.
        /// </summary>
        public Menu(List<Button> buttonList)
        {
            this.buttonList = buttonList;
        }
        #endregion

        #region Properties
        #endregion
    }
}
