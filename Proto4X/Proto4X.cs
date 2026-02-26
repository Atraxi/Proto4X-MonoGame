using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Systems;
using MonoGameLibrary.World;
using Proto4X.Components;
using Proto4X.Systems;

namespace Proto4X
{
    public class Proto4X() : Core("Proto4X", 1920, 1080, false)
    {
        private GameWorld _world;
        private SystemScheduler _systemScheduler;

        protected override void Initialize()
        {
            _world = new GameWorld(0, 0, 1000, 1000);

            _systemScheduler = new SystemScheduler();
            _systemScheduler.Add(_world, new Renderer());
            _systemScheduler.Add(_world, new TimerSystem());
            _systemScheduler.Add(_world, new MovementSystem());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var scout = Content.Load<Texture2D>("Ships/ship_scout_32x32");
            //Todo: actually use sprite library
            var scoutId = SpriteLibrary.Add(scout);

            _world.AddEntity(EntityBuilder.CreateEntity()
                .With(new Position(new Vector2(50, 60), float.DegreesToRadians(90)))
                .With(new Drawable(scoutId, null))
                .With(new Motion(new Vector2(20, 40), default, 1f)));
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
