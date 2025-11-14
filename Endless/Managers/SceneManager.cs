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
        /// <summary>
        /// the curren game Screen
        /// </summary>
        public GameScenes currentScreen;
        private Vector2 dimensions;
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// the graphics Device
        /// </summary>
        public GraphicsDevice GraphicsDevice => graphicsDevice;

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

        public ContentManager Content => content;

        /// <summary>
        /// adds scenes to the stack
        /// </summary>
        /// <param name="scene">the given scene</param>
        public void AddScene(GameScenes scene, bool pauseCurrent = true)
        {
            if (currentScreen != null && pauseCurrent)
                currentScreen.IsPaused = true; // optional, if you track pause state

            sceneStack.Push(scene);
            currentScreen = scene;

            currentScreen.Initialize();
            currentScreen.LoadContent(content);
        }


        /// <summary>
        /// removes a scene
        /// </summary>
        public void RemoveScene()
        {
            if (sceneStack.Count == 0) return;

            // Remove current scene
            currentScreen.UnloadContent();
            sceneStack.Pop();

            // Resume previous scene
            if (sceneStack.Count > 0)
            {
                currentScreen = sceneStack.Peek();
                currentScreen.IsPaused = false; // optional
            }
            else
            {
                currentScreen = null;
            }
        }

        /// <summary>
        /// changes a scene to a new one
        /// </summary>
        /// <param name="newScene">a new gamescene</param>
        public void ChangeScene(GameScenes newScene)
        {
            // Unload all existing scenes
            while (sceneStack.Count > 0)
            {
                currentScreen.UnloadContent();
                var scene = sceneStack.Pop();
                scene.UnloadContent();
            }

            // Add the new scene
            AddScene(newScene, pauseCurrent: false);
        }

        public T GetScene<T>() where T : GameScenes
        {
            foreach (var scene in sceneStack)
            {
                if (scene is T typed)
                    return typed;
            }
            return null;
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
        public void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            content = Content;
            graphicsDevice = graphics;
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
