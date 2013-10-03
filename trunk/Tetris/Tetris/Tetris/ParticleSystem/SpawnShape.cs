using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris
{
    /// <summary>
    /// Abstract class SpawnShapes
    /// </summary>
    public abstract class SpawnShape
    {
        #region Methods
        public abstract Vector2 GetPosition();
        #endregion
    }
    /// <summary>
    /// Spawn on a random point on the circle (oval)
    /// </summary>
    public class CircleSpawnShape : SpawnShape
    {
        #region Fields
        Random random = new Random(153451);
        float height, width;
        #endregion

        #region Methods
        public override Vector2 GetPosition()
        {
            //Generate random point on circle
            double angle = MathHelper.Lerp(0, MathHelper.Pi * 2f, (float)random.NextDouble());
            return new Vector2((float)(width * Math.Sin(angle) / 2f), (float)(height * Math.Cos(angle) / 2f));
        }
        #endregion

        #region Constructors
        public CircleSpawnShape(float width, float height)
        {
            this.height = height;
            this.width = width;
        }
        #endregion

    }
    /// <summary>
    /// Spawn on a random point in a circle (oval)
    /// </summary>
    public class RoundSpawnShape : SpawnShape
    {
        #region Fields
        Random random = new Random(5131543);
        float height, width;
        #endregion

        #region Methods
        public override Vector2 GetPosition()
        {
            double angle = random.NextDouble() * Math.PI * 2;
            double radius = Math.Sqrt(random.NextDouble()); //Random point inside circle with radius 1

            //Generate random point in circle
            return new Vector2((float)(radius * Math.Sin(angle) * width / 2f), (float)(radius * Math.Cos(angle) * height / 2f));
        }
        #endregion

        #region Constructors
        public RoundSpawnShape(float width, float height)
        {
            this.height = height;
            this.width = width;
        }
        #endregion

    }
    /// <summary>
    /// Spawn on a random point inside a rectangle
    /// </summary>
    public class RectangleSpawnShape : SpawnShape
    {
        #region Fields
        Random random = new Random(97864);
        float height, width;
        #endregion

        #region Methods
        public override Vector2 GetPosition()
        {
            //Generate random point in rectangle
            return new Vector2((float)((random.NextDouble() - 0.5) * width), (float)((random.NextDouble() - 0.5) * height));
        }
        #endregion

        #region Constructors
        public RectangleSpawnShape(float width, float height)
        {
            this.height = height;
            this.width = width;
        }
        #endregion

    }
}
