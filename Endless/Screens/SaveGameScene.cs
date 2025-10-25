using Endless.Managers;
using Endless.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Screens
{
    [Serializable]
    public class GameSaveData
    {
        public Vector2 PlayerPosition { get; set; }
        public int HealthLeft { get; set; }
        public int CurrentWave { get; set; }
        public int[,] MapTiles { get; set; }
    }

    public class SaveGameScene : GameScenes
    {
        private SpriteFont Doto;
        private List<string> gameFiles;
        private int selectedIndex;
        private KeyboardState oldState;

        public override void LoadContent(ContentManager content)
        {
            Doto = content.Load<SpriteFont>("Doto-Black");
            gameFiles = new List<string> { "File1", "File2", "File3" };
        }

        public override void Update(GameTime game)
        {
            var keyboard = Keyboard.GetState();

            if (IsKeyPressed(Keys.Up, keyboard))
            {
                System.Diagnostics.Debug.WriteLine("ahhhhhhhhh");
                selectedIndex = (selectedIndex - 1 + gameFiles.Count) % gameFiles.Count;
            }

            if (IsKeyPressed(Keys.Down, keyboard))
                selectedIndex = (selectedIndex + 1) % gameFiles.Count;

            if (IsKeyPressed(Keys.Enter, keyboard))
            {
                var mainScene = SceneManager.Instance.currentScreen as MainGameScene;
                if (mainScene != null)
                {
                    // Check wave active
                    if (mainScene.waveManager.WaveActive)
                    {
                        Console.WriteLine("Cannot save during active wave!");
                        return;
                    }

                    // Collect save data
                    var saveData = new GameSaveData
                    {
                        PlayerPosition = mainScene.Traveler.position,
                        HealthLeft = mainScene.healthLeft, // implement in MainGameScene
                        CurrentWave = mainScene.waveManager.CurrentWave,
                        MapTiles = mainScene.map.SerializeTiles()
                    };

                    SaveToFile(gameFiles[selectedIndex], saveData);
                    Console.WriteLine($"Saved to {gameFiles[selectedIndex]}");
                    SceneManager.Instance.RemoveScene(); // return to pause menu
                }
            }

            oldState = keyboard;
        }

        private void SaveToFile(string fileName, GameSaveData data)
        {
            string path = Path.Combine(Environment.CurrentDirectory, fileName + ".json");
            var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(path, json);
        }

        public override void Draw(GameTime game)
        {
            var sb = SceneManager.Instance.SpriteBatch;
            sb.Begin();

            Vector2 pos = new Vector2(0, 0);

            for (int i = 0; i < gameFiles.Count; i++)
            {
                var text = gameFiles[i];
                var color = (i == selectedIndex) ? Color.Gold : Color.White;

                // draw selection mark for clarity
                if (i == selectedIndex)
                {
                    sb.DrawString(Doto, "> " + text, pos, color);
                }
                else
                {
                    sb.DrawString(Doto, text, pos, color);
                }

                pos.Y += 100f;
            }
            sb.End();
        }

        private bool IsKeyPressed(Keys key, KeyboardState current)
        {
            return current.IsKeyDown(key) && oldState.IsKeyUp(key);
        }

    }
}
