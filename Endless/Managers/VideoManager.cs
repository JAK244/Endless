using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Endless.Managers
{
    public class VideoManager
    {
        private VideoPlayer videoPlayer;
        private Video video;
        private Texture2D currentFrame;



        public void LoadContent(ContentManager content)
        {
            // handles video 
            //video = content.Load<Video>("MemeThoughtsFixed2");
            videoPlayer = new VideoPlayer();
            videoPlayer.IsLooped = true;
            videoPlayer.Play(video);
        }

        public void Update(GameTime gameTime)
        {
            //video loop
            if (videoPlayer.State != MediaState.Stopped)
            {
                currentFrame = videoPlayer.GetTexture();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (currentFrame != null)
            {
                // Get the screen width/height
                int screenWidth = graphicsDevice.Viewport.Width;
                int screenHeight = graphicsDevice.Viewport.Height;

                // Position in top-right corner
                Vector2 position = new Vector2(screenWidth - 320, 10); // 320px from right
                Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, 300, 200); // size on screen

                spriteBatch.Draw(currentFrame, destRect, Color.White);
            }
        }
    }
}
