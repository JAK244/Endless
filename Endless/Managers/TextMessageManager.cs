using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Endless.Managers
{
    /// <summary>
    /// Manages Text messages
    /// </summary>
    public class TextMessageManager
    {
        /// <summary>
        /// handles message stuff
        /// </summary>
        private class Message
        {
            public string Text;
            public Vector2 position;
            public float lifeTime;
            public Color Color;
        }

        private readonly List<Message> messages = new();
        private readonly SpriteFont font;

        /// <summary>
        /// the TextMessageMangaer Constructor
        /// </summary>
        /// <param name="font">the message font</param>
        public TextMessageManager(SpriteFont font)
        {
            this.font = font;
        }

        /// <summary>
        /// add a message 
        /// </summary>
        /// <param name="text">the text</param>
        /// <param name="Position">the text position</param>
        /// <param name="color">the text color</param>
        public void Add(string text, Vector2 Position, Color color)
        {
            messages.Add(new Message
            {
                Text = text,
                position = Position,
                lifeTime = 1.5f, // time on screen
                Color = color
            });
        }

        /// <summary>
        /// updates the textmessage using gameTime
        /// </summary>
        /// <param name="gameTime">the gameTime</param>
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = messages.Count - 1; i >= 0; i--)
            {
                messages[i].lifeTime -= dt;
                messages[i].position.Y -= 20f * dt; // slowly rise up

                if (messages[i].lifeTime <= 0)
                    messages.RemoveAt(i);
            }
        }

        /// <summary>
        /// Draws the message using SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">the spriteBatchs</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var msg in messages)
            {
                float alpha = MathHelper.Clamp(msg.lifeTime / 1.5f, 0, 1);
                spriteBatch.DrawString(font, msg.Text, new Vector2(500,200), msg.Color * alpha); 
            }
        }
    }


}

