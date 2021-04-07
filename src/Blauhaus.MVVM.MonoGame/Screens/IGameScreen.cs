using Blauhaus.MVVM.Abstractions.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Screens
{
    public interface IGameScreen : IView
    {
        public object BindingContext { get; }
        
        public void Initialize();
        public void LoadContent();

        public void Update(GameTime gameTime);

        public void BeforeDraw(SpriteBatch spriteBatch);
        public void Draw(SpriteBatch spriteBatch);
        public void AfterDraw(SpriteBatch spriteBatch);

        public void UnloadContent();
        
    }
}