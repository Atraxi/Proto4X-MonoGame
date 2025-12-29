using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using Proto4x.World;

namespace Proto4X
{
    public class Proto4X : Core
    {
        private GameLayer _world;

        public Proto4X() : base("Proto4X", 1920, 1080, false)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            _world = new GameLayer(0, 0, 1000, 1000);
        }

        protected override void LoadContent()
        {
            //var thing = Texture2D.FromFile(GraphicsDevice, "");
            //_logo = Content.Load<Texture2D>("images/logo");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _world.Draw(SpriteBatch, new Rectangle(0, 0, 1000, 1000));//TODO UI viewport

            base.Draw(gameTime);
        }
    }
}
