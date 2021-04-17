using System;
using Blauhaus.MonoGame.Vertices;
using Blauhaus.MVVM.MonoGame.Games;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Screens
{
    public abstract class Base3DScreen<TViewModel> : BaseUpdateScreen<TViewModel>
    {
        
        protected Matrix WorldMatrix = Matrix.Identity;
        protected Matrix ViewMatrix;
        protected Matrix ProjectionMatrix;
        
        protected BasicEffect Effect = null!;

        protected VertexBuffer? VertexBuffer;
        protected IndexBuffer? IndexBuffer;
        protected int[] Indices = Array.Empty<int>();
        protected VertexPositionColorNormal[]? Vertices;

        protected SpriteFont DefaultFont = null!;

        protected Base3DScreen(IScreenGame game, TViewModel viewModel) : base(game, viewModel)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            
            Effect = new BasicEffect(GraphicsDevice);

            DefaultFont = ScreenContent.Load<SpriteFont>("Fonts/Default");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Vertices == null)
            {
                return;
            }

            Effect.VertexColorEnabled = true;

            Effect.World = WorldMatrix;
            Effect.Projection = ProjectionMatrix;
            Effect.View = ViewMatrix;

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                DrawVertices();
            }
        }
         
        protected abstract void DrawVertices();
        
        protected void DrawText(SpriteBatch spriteBatch, string text, float x, float y)
        {
            spriteBatch.DrawString(DefaultFont, text, new Vector2(x,y), Color.White);
        }
    }
}