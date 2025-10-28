using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Endless.Managers
{
    public class TextMessageManager
    {
        private class Message
        {
            public string Text;
            public Vector2 position;
            public float lifeTime;
            public Color Color;
        }

        private readonly List<Message> messages = new();
        private readonly SpriteFont font;

        public TextMessageManager(SpriteFont font)
        {
            this.font = font;
        }

        public void Add(string text, Vector2 Position, Color color)
        {
            messages.Add(new Message
            {
                Text = text,
                position = Position,
                lifeTime = 1.5f, // seconds before fading
                Color = color
            });
        }

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

