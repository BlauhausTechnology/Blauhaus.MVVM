using Blauhaus.MVVM.MonoGame.Games;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Scenes
{
    public abstract class BaseScene<TGame> : IScene
        where TGame : BaseSceneGame
    {
        protected TGame Game;
        protected ContentManager? ScrenContent;
        protected ContentManager GameContent;

        protected BaseScene(TGame game)
        {
            Game = game;
            GameContent = Game.Content;
        }
        
        public virtual void Initialize()
        {
            ScrenContent = new ContentManager(Game.Services)
            {
                RootDirectory = Game.Content.RootDirectory
            };
            
            LoadContent();
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }
        
        public virtual void BeforeDraw(SpriteBatch spriteBatch, Color clearColor)
        {
            Game.GraphicsDevice.Clear(clearColor);
            spriteBatch.Begin();
        }
        
        public virtual void Draw(SpriteBatch spriteBatch) { }
        
        public virtual void AfterDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }
        
        public virtual void UnloadContent() 
        {
            ScrenContent?.Unload();
            ScrenContent = null;
        }
    }
}