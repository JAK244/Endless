using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Endless
{
    public class SceneManager
    {
        private static SceneManager instance;

        // This will just store the reference from Game1
        private ContentManager content;

        private Stack<GameScenes> sceneStack = new Stack<GameScenes>();
        private GameScenes currentScreen;
        private Vector2 dimensions;

        public SpriteBatch SpriteBatch { get; set; }

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();
                return instance;
            }
        }

        public Vector2 Dimensions
        {
            get => dimensions;
            set => dimensions = value;
        }

        // Switch to a new scene
        public void AddScene(GameScenes scene)
        {
            if (currentScreen != null)
                currentScreen.UnloadContent();

            sceneStack.Push(scene);
            currentScreen = scene;

            currentScreen.Initialize();
            // Load content using the same ContentManager from Game1
            currentScreen.LoadContent(content);
        }

        public void Initialize()
        {
            currentScreen = new TitleScene();
            currentScreen.Initialize();
        }

        public void LoadContent(ContentManager Content)
        {
            // Just store the reference, don't create a new ContentManager
            content = Content;
            currentScreen.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            currentScreen?.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            currentScreen?.Draw(gameTime);
        }
    }
}
