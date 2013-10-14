using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Achievement
    {
        #region Fields
        static Texture2D Texture;
        static Rectangle imageRect = new Rectangle(5, 5, 90, 90);
        static Vector2 size = new Vector2(270, 100);
        static Vector2 position, screen;
        static int speed = 10;
        static List<Achievement> queueList = new List<Achievement>();
        static bool drawing = false;
        String name;
        String line1, line2, line3;
        Texture2D image;
        Color backColor, textColor;
        SpriteFont titleFont, descFont;
        bool get = false;
        bool got = false;
        bool draw = false;
        int count = 0;
        int maxCount = 300;
        int move = 1;
        #endregion

        #region Methods
        public static void Initialise(GraphicsDevice g)
        {
            //Create a texture for the background
            Texture = new Texture2D(g, 1, 1);
            Texture.SetData(new Color[] { Color.White });
            //Set the position and screen size
            position = new Vector2(g.Viewport.Width - size.X, g.Viewport.Height);
            screen = new Vector2(g.Viewport.Width, g.Viewport.Height);
        }
        public void Update()
        {
            //Check if an achievement was get
            if (get)
            {
                //Set the startTime and stopTime, draw and got
                if (!got)
                {
                    draw = true;
                    got = true;
                    drawing = true;
                }
                //Move up
                if (move == 1)
                    if (position.Y > screen.Y - size.Y)
                        position.Y -= speed;
                    else
                        //Wait when completely inside screen
                        move = 0;
                //Wait
                if (move == 0)
                {
                    //Add 1 to count (60 per second)
                    if (count < maxCount)
                        count++;
                    //Check the time
                    else
                        move = -1;
                }
                //Move down
                if (move == -1)
                    if (position.Y < screen.Y)
                        position.Y += speed;
                    //Stop when outside screen
                    else
                        move = -2;
                //Stop
                if (move == -2)
                {
                    get = false;
                    draw = false;
                    drawing = false;
                }
            }
            //Get all achievements from the queue, starting with the first
            if (queueList.Count != 0 && !drawing)
            {
                Achievement a = queueList.First();
                a.GetAchievement();
                queueList.Remove(a);
            }
        }
        public void GetAchievement()
        {
            //If the player hasn't got the achievement yet and another achievement is NOT being drawn, get the achievement
            if (!got && !drawing)
                get = true;
            //If the player hasn't got the achievement yet and another achievement IS being drawn, add the achievement to the queue list
            else if (!got && drawing)
                queueList.Add(this);
        }
        public void Draw(SpriteBatch s)
        {
            if (draw)
            {
                //Background rectangle
                s.Draw(Texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), backColor);
                //Image
                s.Draw(image, new Rectangle((int)position.X + imageRect.Left, (int)position.Y + imageRect.Top, imageRect.Width, imageRect.Height), Color.White);
                //Achievement name
                s.DrawString(titleFont, name, new Vector2((int)position.X + imageRect.Left + imageRect.Right, (int)position.Y), textColor);
                //Description (3 lines)
                s.DrawString(descFont, line1, new Vector2((int)position.X + imageRect.Left + imageRect.Right, (int)position.Y + descFont.MeasureString(name).Y), textColor);
                s.DrawString(descFont, line2, new Vector2((int)position.X + imageRect.Left + imageRect.Right, (int)position.Y + descFont.MeasureString(name).Y + descFont.MeasureString(line1).Y), textColor);
                s.DrawString(descFont, line3, new Vector2((int)position.X + imageRect.Left + imageRect.Right, (int)position.Y + descFont.MeasureString(name).Y + descFont.MeasureString(line1).Y + descFont.MeasureString(line2).Y), textColor);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a Steam-like achievement
        /// </summary>
        /// <param name="name">The title of the achievement</param>
        /// <param name="line1">Line 1 of the description</param>
        /// <param name="image">The picture to be displayed next to the text</param>
        /// <param name="backColor">The color of the back rectangle</param>
        /// <param name="textColor">The color of the text</param>
        /// <param name="titleFont">The font to be used for the title</param>
        /// <param name="descFont">The font to be used for the description</param>
        public Achievement(String name, String line1, Texture2D image, Color backColor, Color textColor, SpriteFont titleFont, SpriteFont descFont)
        {
            this.name = name;
            this.line1 = line1;
            this.line2 = "";
            this.line3 = "";
            this.image = image;
            this.backColor = backColor;
            this.textColor = textColor;
            this.titleFont = titleFont;
            this.descFont = descFont;
        }
        /// <summary>
        /// Creates a Steam-like achievement
        /// </summary>
        /// <param name="name">The title of the achievement</param>
        /// <param name="line1">Line 1 of the description</param>
        /// <param name="line2">Line 2 of the description</param>
        /// <param name="image">The picture to be displayed next to the text</param>
        /// <param name="backColor">The color of the back rectangle</param>
        /// <param name="textColor">The color of the text</param>
        /// <param name="titleFont">The font to be used for the title</param>
        /// <param name="descFont">The font to be used for the description</param>
        public Achievement(String name, String line1, String line2, Texture2D image, Color backColor, Color textColor, SpriteFont titleFont, SpriteFont descFont)
        {
            this.name = name;
            this.line1 = line1;
            this.line2 = line2;
            this.line3 = "";
            this.image = image;
            this.backColor = backColor;
            this.textColor = textColor;
            this.titleFont = titleFont;
            this.descFont = descFont;
        }
        /// <summary>
        /// Creates a Steam-like achievement
        /// </summary>
        /// <param name="name">The title of the achievement</param>
        /// <param name="line1">Line 1 of the description</param>
        /// <param name="line2">Line 2 of the description</param>
        /// <param name="line3">Line 3 of the description</param>
        /// <param name="image">The picture to be displayed next to the text</param>
        /// <param name="backColor">The color of the back rectangle</param>
        /// <param name="textColor">The color of the text</param>
        /// <param name="titleFont">The font to be used for the title</param>
        /// <param name="descFont">The font to be used for the description</param>
        public Achievement(String name, String line1, String line2, String line3, Texture2D image, Color backColor, Color textColor, SpriteFont titleFont, SpriteFont descFont)
        {
            this.name = name;
            this.line1 = line1;
            this.line2 = line2;
            this.line3 = line3;
            this.image = image;
            this.backColor = backColor;
            this.textColor = textColor;
            this.titleFont = titleFont;
            this.descFont = descFont;
        }
        #endregion

        #region Properties
        public bool Achieved { get { return got; } set { got = value; } }
        public string Name { get { return name; } }
        #endregion
    }
}
