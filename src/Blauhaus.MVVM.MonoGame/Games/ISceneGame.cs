using System;
using Blauhaus.MVVM.MonoGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Games
{
    public interface ISceneGame
    {
        void ChangeScene(IScene next);

        ContentManager Content { get; }
        GameServiceContainer Services { get; }
        GraphicsDevice GraphicsDevice { get; }
    }
}