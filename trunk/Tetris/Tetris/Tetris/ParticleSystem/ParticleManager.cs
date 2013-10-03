using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    /// <summary>
    /// Manager of particle emitters.
    /// </summary>
    public static class ParticleManager
    {
        #region Fields
        static List<Emitter> emitters = new List<Emitter>(); //All emitter managed by this one manager
        #endregion

        #region Methods
        public static void update()
        {
            //Update all emitters
            foreach (Emitter emitter in emitters)
            {
                emitter.Update();
            }
        }
        /// <summary>
        /// Draw the full particle manager
        /// </summary>
        /// <param name="s">SpriteBatch. WARNING! DO NOT send in an open spriteBatch. Make sure it has been ended</param>
        public static void draw(SpriteBatch s)
        {
            //Draw all emitters
            foreach (Emitter emitter in emitters)
            {
                emitter.Draw(s);
            }
        }
        public static void addEmitter(Emitter emitter)
        {
            emitters.Add(emitter);
        }
        public static void removeEmitter(Emitter emitter)
        {
            emitters.Remove(emitter);
        }
        #endregion

        #region Properties
        #endregion
    }
}
