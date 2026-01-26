using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using Proto4x.World;
using Proto4X.Systems;

namespace Proto4X
{
    public class Proto4X : Core
    {
        private GameLayer _world;
        private SystemScheduler _systemScheduler;

        public Proto4X() : base("Proto4X", 1920, 1080, false)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            _world = new GameLayer(0, 0, 1000, 1000);
            _systemScheduler = new SystemScheduler();
            _systemScheduler.Add(_world, new Renderer());
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

            _systemScheduler.Update(_world, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _systemScheduler.Draw(SpriteBatch, /*TODO UI viewport*/new Rectangle(0, 0, 1000, 1000), _world);

            base.Draw(gameTime);
        }
    }
}
