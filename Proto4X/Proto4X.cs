using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace Proto4X
{
    public class Proto4X : Core
    {
        private Texture2D _logo;

        public Proto4X() : base("Proto4X", 1920, 1080, false)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _logo = Content.Load<Texture2D>("images/logo");
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

            SpriteBatch.Begin();

            // Draw the logo texture
            SpriteBatch.Draw(_logo, Vector2.Zero, Color.White);

            // Always end the sprite batch when finished.
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
