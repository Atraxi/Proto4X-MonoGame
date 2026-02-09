using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Systems;
using MonoGameLibrary.World;
using Proto4X.Archetypes;
using Proto4X.Systems;

namespace Proto4X
{
    public class Proto4X : Core
    {
        private GameLayer _world;
        private SystemScheduler _systemScheduler;
        private SpriteLibrary _spriteLibrary;

        public Proto4X() : base("Proto4X", 1920, 1080, false)
        {

        }

        protected override void Initialize()
        {
            _world = new GameLayer(0, 0, 1000, 1000);

            _systemScheduler = new SystemScheduler();
            _systemScheduler.Add(_world, new Renderer());
            _systemScheduler.Add(_world, new TimerSystem());
            _systemScheduler.Add(_world, new MovementSystem());

            _spriteLibrary = new SpriteLibrary();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var scout = Content.Load<Texture2D>("Ships/ship_scout_32x32");
            //Todo: actually use sprite library
            //_spriteLibrary.Add();

            var drawableArchetype = new DrawableTexturePosition(500);
            drawableArchetype.AddEntity(0, (new Vector2(50, 60), 90, scout, null));
            _world.Archetypes.Add(scout, [drawableArchetype]);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

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
