using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris
{
    public abstract class ParticleModifier
    {
        #region Fields
        #endregion

        #region Methods
        public abstract void Update(Particle particle);
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion
    }
    public class GravityModifier : ParticleModifier
    {
        #region Fields
        Vector2 gravity;
        #endregion

        #region Methods
        public override void Update(Particle particle)
        {
            particle.Speed += gravity;
        }
        #endregion

        #region Constructors
        public GravityModifier(Vector2 gravity)
        {
            this.gravity = gravity;
        }
        #endregion

        #region Properties
        #endregion
    }
    public class RandomSpeedModifier : ParticleModifier
    {
        Vector2 minSpeed;
        Vector2 maxSpeed;

        public override void Update(Particle particle)
        {
            particle.Speed += new Vector2((maxSpeed.X - minSpeed.X) * (float)GameManager.Random.NextDouble() + minSpeed.X, (maxSpeed.Y - minSpeed.Y) * (float)GameManager.Random.NextDouble() + minSpeed.Y);
        }
        public RandomSpeedModifier(Vector2 minSpeed, Vector2 maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
        }
        public RandomSpeedModifier(Vector2 speed)
        {
            this.minSpeed = -speed;
            this.maxSpeed = speed;
        }
    }
}
