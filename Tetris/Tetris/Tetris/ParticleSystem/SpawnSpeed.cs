using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris
{
    /// <summary>
    /// Abstract class for spawnSpeed
    /// </summary>
    public abstract class SpawnSpeed
    {
        #region Methods
        public abstract Vector2 GetSpeed(Emitter emitter, Particle particle);
        #endregion
    }
    /// <summary>
    /// Spawn with a random speed
    /// </summary>
    public class RandomSpawnSpeed : SpawnSpeed
    {
        #region Fields
        Random random = new Random(65437);
        Vector2 minSpeed;
        Vector2 maxSpeed;
        #endregion

        #region Methods
        public override Vector2 GetSpeed(Emitter emitter, Particle particle)
        {
            return new Vector2(MathHelper.Lerp(minSpeed.X, maxSpeed.X, (float)random.NextDouble()), MathHelper.Lerp(minSpeed.Y, maxSpeed.Y, (float)random.NextDouble()));
        }
        #endregion

        #region Constructors
        public RandomSpawnSpeed(Vector2 minSpeed, Vector2 maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
        }
        public RandomSpawnSpeed(Vector2 maxSpeed)
        {
            this.maxSpeed = maxSpeed;
            this.minSpeed = -maxSpeed;
        }
        #endregion
    }
    /// <summary>
    /// Spawn and move towards the origin
    /// </summary>
    public class OriginSpawnSpeed : SpawnSpeed
    {
        #region Fields
        Random random = new Random(325467387);
        float minSpeed;
        float maxSpeed;
        #endregion

        #region Methods
        public override Vector2 GetSpeed(Emitter emitter, Particle particle)
        {
            //Get direction
            Vector2 direction = new Vector2(emitter.Position.X - particle.Position.X, emitter.Position.Y - particle.Position.Y);
            //Calculate speed
            float speed = MathHelper.Lerp(minSpeed, maxSpeed, (float)random.NextDouble());

            //Calculate final vector
            return direction / direction.Length() * speed;
        }
        #endregion

        #region Constructors
        public OriginSpawnSpeed(float minSpeed, float maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
        }
        public OriginSpawnSpeed(float speed)
        {
            this.maxSpeed = speed;
            this.minSpeed = speed;
        }
        #endregion
    }
}
