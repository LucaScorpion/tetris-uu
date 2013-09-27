using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Block
    {
        #region Fields
        Texture2D texture = Assets.DummyTexture;
        Color color;
        #endregion

        #region Methods
        public void Update()
        {

        }
        #endregion

        #region Constructors
        public Block()
        {
            texture = Assets.DummyTexture;
            color = Color.Red;
        }
        #endregion

        #region Properties
        public Texture2D Texture { get { return texture; } }
        #endregion
    }
}
