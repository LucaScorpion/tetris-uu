﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    /// <summary>
    /// Single particle managed by an emitter
    /// </summary>
    public class Particle
    {
        #region Fields
        Vector2 position = Vector2.Zero;
        float beginSize = 1f;
        float endSize = 0f;
        Vector2 speed = Vector2.Zero;
        Texture2D texture;
        Color beginColor; //Color on spawn
        Color endColor; //Color on death. (Fades from begin to end color)
        float ttl = 30; //Amount of ms to live
        float lifeTime = 0; //The age of the particle in ms
        Vector2 gravity;
        #endregion

        #region Methods
        public void Update()
        {
            //Change Lifetime
            lifeTime += GameManager.GameTime.ElapsedGameTime.Milliseconds;

            //Gravity
            speed += gravity;

            //Random speed change
            float speedChange = 0.3f;
            speed += new Vector2((float)(GameManager.Random.NextDouble() - 0.5) * speedChange, (float)(GameManager.Random.NextDouble() - 0.5) * speedChange);

            //Move particle
            position += speed;
        }
        public void Draw(SpriteBatch s)
        {
            if (isAlive)
            {
                //Draw particle
                float size = MathHelper.Lerp(beginSize, endSize, lifeTime / ttl);
                s.Draw(texture, new Vector2(position.X - size * texture.Width / 2, position.Y - size * texture.Height / 2), null, Color.Lerp(beginColor, endColor, lifeTime / ttl), 0f, Vector2.Zero, size, SpriteEffects.None, 0);
            }
        }
        public void SetSpeed(Vector2 speed)
        {
            this.speed = speed;
        }
        #endregion

        #region Constructors
        public Particle(float ttl, Vector2 position, float beginSize, float endSize, Color beginColor, Color endColor, Texture2D texture, Vector2 gravity)
        {
            this.position = position;
            this.beginSize = beginSize;
            this.endSize = endSize;
            this.speed = Vector2.Zero;
            this.beginColor = beginColor;
            this.endColor = endColor;
            this.texture = texture;
            this.ttl = ttl * 1000;
            this.gravity = gravity;
        }
        #endregion

        #region Properties
        public bool isAlive { get { if (ttl > lifeTime) { return true; } else { return false; } } }
        public Vector2 Position { get { return position; } set { position = value; } }
        #endregion
    }
}
