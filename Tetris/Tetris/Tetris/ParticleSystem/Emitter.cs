using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    /// <summary>
    /// Emitter of particles
    /// </summary>
    public class Emitter
    {
        #region Fields
        List<Particle> particles = new List<Particle>();
        Vector2 position = Vector2.Zero;
        float particlesPerLoop; //Amount of particles per loop
        float waitList = 0; //Amount of particles waiting to be spawned
        Random random = new Random();
        bool paused = true;
        float beginSize;
        float endSize;
        Color beginColor;
        Color endColor;
        Texture2D texture;
        SpawnSpeed spawnSpeed;
        float ttl;
        SpawnShape spawnShape;
        Vector2 gravity;
        #endregion

        #region Methods
        /// <summary>
        /// Update Emitter. (Updates all particles sent out by the emitter)
        /// </summary>
        public void Update()
        {
            //If the emitter isn't paused, it will call the Shoot method.
            if (!paused)
            {
                Shoot();
            }

            //Loop through all particles. Kill if the particle isn't alive, otherwise Update the particle.
            for (int i = particles.Count() - 1; i >= 0; i--)
            {
                //Update all particles that are alive.
                if (particles[i].isAlive)
                {
                    particles[i].Update();
                }
                else
                {
                    //Remove the dead particle
                    particles.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Draw all particles on the screen.
        /// </summary>
        /// <param name="s">Opened Spritebatch. (Commonly opened by the ParticleManager)</param>
        public void Draw(SpriteBatch s)
        {
            //Draw EVERY particle
            foreach (Particle particle in particles)
            {
                particle.Draw(s);
            }
        }
        /// <summary>
        /// Start spawning particles every Update.
        /// </summary>
        public void Start()
        {
            paused = false;
        }
        /// <summary>
        /// Pause spawning particles.
        /// </summary>
        public void Pause()
        {
            paused = true;
        }
        /// <summary>
        /// Spawn a single batch of particles. (if paused == false then Shoot is called every Update)
        /// </summary>
        public void Shoot()
        {
            waitList += particlesPerLoop; //Add new particles to waitlist

            //Spawn particles and empty waitlist
            for (; waitList >= 1; waitList--)
            {
                //Spawn a particle
                Particle newParticle = new Particle(ttl, position + spawnShape.GetPosition(), beginSize, endSize, beginColor, endColor, texture, gravity);
                //Set particle speed
                newParticle.SetSpeed(spawnSpeed.GetSpeed(this, newParticle));

                //Add particle to list
                particles.Add(newParticle);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Emitter of particles
        /// </summary>
        /// <param name="beginSize">Size of particle on spawn</param>
        /// <param name="endSize">Size of particle on dreath (lerp)</param>
        /// <param name="beginColor">Color of particle on spawn</param>
        /// <param name="endColor">Color of particle on death (fade)</param>
        /// <param name="particlesPerLoop">Amount of particles spawned per loop. (Every time Shoot() is called)</param>
        /// <param name="spawnSpeed">Maximum speed of the particle on spawn. (MAXIMUM)</param>
        public Emitter(float beginSize, float endSize, Color beginColor, Color endColor, float particlesPerLoop, float ttl, SpawnSpeed spawnSpeed, Texture2D texture, SpawnShape spawnShape, Vector2 gravity)
        {
            this.beginColor = beginColor;
            this.endColor = endColor;
            this.beginSize = beginSize;
            this.endSize = endSize;
            this.spawnSpeed = spawnSpeed;
            this.particlesPerLoop = particlesPerLoop;
            this.texture = texture;
            this.spawnShape = spawnShape;
            this.ttl = ttl;
            this.gravity = gravity;

            ParticleManager.addEmitter(this);
        }
        #endregion

        #region Properties
        public Vector2 Position { get { return position; } set { position = value; } }
        #endregion
    }
}
