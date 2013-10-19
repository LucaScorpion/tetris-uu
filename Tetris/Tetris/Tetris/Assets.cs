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
            public static Texture2D FreddieMercury;
            public static Texture2D CloseEnough;
            public static Texture2D ItsSomething;
            public static Texture2D Wow;
            public static Texture2D ROFLcopter;
            public static Texture2D IELogo;
            public static Texture2D RockHand;
            public static Texture2D Focused;
            public static Texture2D PukingRainbows;
            public static Texture2D Lock;
            public static Texture2D ArrowLeft;
            public static Texture2D ArrowRight;
            public static Texture2D ComboBreaker;
            public static Texture2D AwwYea;
            public static Texture2D Heart;
            public static Texture2D AchievementWh0re;
            public static Texture2D CountVonCount;
            public static Texture2D MaxLevel;
        }
        public struct Fonts
        {
            public static SpriteFont BasicFont;
            public static SpriteFont SmallerFont;
            public static SpriteFont GiantFont;
        }
        public struct Audio
        {
            public static SoundEffect LockSound;
            public static SoundEffect Single;
            public static SoundEffect Double;
            public static SoundEffect Triple;
            public static SoundEffect Tetris;
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
