using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Endless.Screens;

namespace Endless.Managers
{
    /// <summary>
    /// the screenManger class
    /// </summary>
    public class SceneManager
    {
        private static SceneManager instance;
        private ContentManager content;
        private Stack<GameScenes> sceneStack = new Stack<GameScenes>();
        private GameScenes currentScreen;
        private Vector2 dimensions;

        /// <summary>
        /// the sprite batch
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// the currentTranslation 
        /// </summary>
        public Matrix CurrentTranslation { get; set; } = Matrix.Identity;


        /// <summary>
        /// the static sceneManager instance
        /// </summary>
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();
                return instance;
            }
        }

        /// <summary>
        /// the dimensions of the scenc
        /// </summary>
        public Vector2 Dimensions
        {
            get => dimensions;
            set => dimensions = value;
        }

        /// <summary>
        /// adds scenes to the stack
        /// </summary>
        /// <param name="scene">the given scene</param>
        public void AddScene(GameScenes scene)
        {
            if (currentScreen != null)
                currentScreen.UnloadContent();

            sceneStack.Push(scene);
            currentScreen = scene;

            currentScreen.Initialize();
            
            currentScreen.LoadContent(content);
        }

        /// <summary>
        /// initalizes the current scene
        /// </summary>
        public void Initialize()
        {
            currentScreen = new TitleScene();
            currentScreen.Initialize();
        }

        /// <summary>
        /// loads the content with a content manager
        /// </summary>
        /// <param name="Content">the content manager</param>
        public void LoadContent(ContentManager Content)
        {
            // Just store the reference, don't create a new ContentManager
            content = Content;
            currentScreen.LoadContent(content);
        }

        /// <summary>
        /// updates the current screen 
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            currentScreen?.Update(gameTime);
        }

        /// <summary>
        /// Draws the current scene 
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Draw(GameTime gameTime)
        {
            currentScreen?.Draw(gameTime);
        }
    }
}
