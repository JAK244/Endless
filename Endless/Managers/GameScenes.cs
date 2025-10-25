using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

/// <summary>
/// the gameScenes class
/// </summary>
public class GameScenes
{
    /// <summary>
    /// the content manger for the scenes
    /// </summary>
    protected ContentManager content;

    public bool IsPaused;

    /// <summary>
    /// a virtual initalize method
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// a virtual loadcontent method
    /// </summary>
    /// <param name="Content">the content manager</param>
    public virtual void LoadContent(ContentManager Content)
    {
        // Store the reference from Game1
        content = Content;
    }

    /// <summary>
    /// virtual unloadContent method
    /// </summary>
    public virtual void UnloadContent()
    {
        
    }

    /// <summary>
    /// virtual updata method
    /// </summary>
    /// <param name="gameTime">the game time</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// virtual draw method
    /// </summary>
    /// <param name="gameTime">the game time</param>
    public virtual void Draw(GameTime gameTime) { }
}
