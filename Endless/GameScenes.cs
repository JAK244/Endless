using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

public class GameScenes
{
    protected ContentManager content;

    public virtual void Initialize() { }

    public virtual void LoadContent(ContentManager Content)
    {
        // Store the reference from Game1
        content = Content;
    }

    public virtual void UnloadContent()
    {
        // Only unload if it exists (optional)
        // Do NOT unload the main ContentManager from Game1
        // content?.Unload();
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(GameTime gameTime) { }
}
