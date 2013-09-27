﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Tetris
{
    /// <summary>
    /// This static class contains several commonly used assets like sounds, textures and fonts.
    /// </summary>
    public static class Assets
    {
        #region Fields
        public static Texture2D DummyTexture;

        public struct Fonts
        {
            public static SpriteFont BasicFont;
        }
        #endregion

        #region Methods
        public static void init(GraphicsDevice g)
        {
            DummyTexture = new Texture2D(g, 1, 1);
            DummyTexture.SetData(new Color[] { Color.White });
        }
        #endregion
    }
}
