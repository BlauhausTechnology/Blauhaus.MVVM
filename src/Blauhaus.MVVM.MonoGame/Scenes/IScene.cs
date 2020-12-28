using Blauhaus.MVVM.Abstractions.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Scenes
{
    public interface IScene : IView
    {
        public void Initialize();
        public void LoadContent();

        public void Update(GameTime gameTime);

        public void BeforeDraw(SpriteBatch spriteBatch, Color clearColor);
        public void Draw(SpriteBatch spriteBatch);
        public void AfterDraw(SpriteBatch spriteBatch);

        public void UnloadContent();
        
    }
}