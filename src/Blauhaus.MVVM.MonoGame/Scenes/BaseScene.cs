using Blauhaus.MVVM.Abstractions.ViewModels;
using Blauhaus.MVVM.MonoGame.Games;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Scenes
{
    public abstract class BaseScene<TViewModel> : IScene
    {
        protected readonly TViewModel ViewModel;
        
        protected ISceneGame Game;

        protected Color BackgroundColour = Color.Black;
        
        private ContentManager? _screenContent;
        protected ContentManager ScreenContent =>
            _screenContent ??= new ContentManager(Game.Services)
            {
                RootDirectory = Game.Content.RootDirectory
            };

        protected ContentManager GameContent;
        protected GraphicsDevice GraphicsDevice;
        
        protected BaseScene(ISceneGame game, TViewModel viewModel)
        {
            Game = game;
            ViewModel = viewModel;
            GameContent = Game.Content;
            GraphicsDevice = Game.GraphicsDevice;
        }
        
        public virtual void Initialize()
        { 
            LoadContent();

            if (ViewModel is IAppearingViewModel appearingViewModel)
            {
                appearingViewModel.AppearCommand.Execute();
            }
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }
        
        public virtual void BeforeDraw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(BackgroundColour);
            spriteBatch.Begin();
        }
        
        public virtual void Draw(SpriteBatch spriteBatch) { }
        
        public virtual void AfterDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }
        
        public virtual void UnloadContent() 
        {
            _screenContent?.Unload();
            _screenContent = null;
        }
    }
}