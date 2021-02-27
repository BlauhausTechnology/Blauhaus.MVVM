using Blauhaus.MonoGame.Vertices;
using Blauhaus.MVVM.MonoGame.Games;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blauhaus.MVVM.MonoGame.Screens
{
    public class Base3DScreen<TViewModel> : BaseScreen<TViewModel>
    {
        
        protected Matrix WorldMatrix = Matrix.Identity;
        protected Matrix ViewMatrix;
        protected Matrix ProjectionMatrix;
        
        protected BasicEffect Effect;
        protected VertexBuffer VertexBuffer;
        protected IndexBuffer IndexBuffer;
        protected VertexPositionColorNormal[] Vertices;

        protected SpriteFont DefaultFont;
        
        public Base3DScreen(IScreenGame game, TViewModel viewModel) : base(game, viewModel)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            
            Effect = new BasicEffect(GraphicsDevice);

            DefaultFont = ScreenContent.Load<SpriteFont>("Fonts/Default");
        }

        public override void Initialize()
        {
            base.Initialize();

        }

        protected void DrawText(SpriteBatch spriteBatch, string text, float x, float y)
        {
            spriteBatch.DrawString(DefaultFont, text, new Vector2(x,y), Color.White);
        }
    }
}