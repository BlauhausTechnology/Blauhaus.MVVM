using System;
using Blauhaus.MVVM.MonoGame.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Games
{
    public interface IScreenGame
    {
        void ChangeScene(IGameScreen next);

        ContentManager Content { get; }
        GameServiceContainer Services { get; }
        GraphicsDevice GraphicsDevice { get; }
    }
}