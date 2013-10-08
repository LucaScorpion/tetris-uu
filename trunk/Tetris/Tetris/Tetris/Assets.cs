using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    /// <summary>
    /// This static class contains several commonly used assets like sounds, textures and fonts.
    /// </summary>
    public static class Assets
    {
        #region Fields
        public struct Textures
        {
            public static Texture2D DummyTexture;
            public static Texture2D Block;
            public static Texture2D Particle;
            public static Texture2D WorldBG;
            public static Texture2D MenuBG;
        }
        public struct Fonts
        {
            public static SpriteFont BasicFont;
            public static SpriteFont SmallerFont;
        }
        public struct Audio
        {
            public static SoundEffect LockSound;
            public static SoundEffect Single;
            public static SoundEffect Double;
            public static SoundEffect Triple;
            public static SoundEffect Tetris;
            public static SoundEffect Intro;
            public static SoundEffect Loop;
        }
        #endregion

        #region Methods
        public static void init(GraphicsDevice g)
        {
            Textures.DummyTexture = new Texture2D(g, 1, 1);
            Textures.DummyTexture.SetData(new Color[] { Color.White });
        }
        #endregion
    }
}
