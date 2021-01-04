using System;
using Blauhaus.MVVM.MonoGame.Scenes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Games
{
    public abstract class BaseSceneGame : Game, ISceneGame
    {
        protected GraphicsDeviceManager GraphicsDeviceManager;
        private SpriteBatch _spriteBatch = null!;
        
        private IScene? _activeScene;
        private IScene? _nextScene;

         
        protected BaseSceneGame() 
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                //otherwise have to use short instead of int for IndexBuffer so no more than 32k indices
                GraphicsProfile = GraphicsProfile.HiDef
            };
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        
        protected override void LoadContent()
        {
            base.LoadContent();
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (_nextScene != null)
            {
                TransitionScene();
            }

            _activeScene?.Update(gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            if(_activeScene != null)
            {
                _activeScene.BeforeDraw(_spriteBatch);
                _activeScene.Draw(_spriteBatch);
                _activeScene.AfterDraw(_spriteBatch);
            }

            base.Draw(gameTime);
        }
        
        public void ChangeScene(IScene next)
        {
            if(_activeScene != next)
            {
                _nextScene = next;
            }
        }


        private void TransitionScene()
        {
            _activeScene?.UnloadContent();

            GC.Collect();

            _activeScene = _nextScene;
            _nextScene = null;

            _activeScene?.Initialize();
        }
    }
}