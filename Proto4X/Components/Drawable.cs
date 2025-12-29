using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto4x.Components;

namespace Proto4X.Components
{
    internal record Drawable(int Id, Texture2D Texture) : ComponentBase(Id)
    {
        public Rectangle GetBounds()
        {
            return Texture.Bounds;
        }
        public void Draw(SpriteBatch spriteBatch, Position position)
        {
            spriteBatch.Draw(Texture, position.Location, null, Color.White, position.Rotation, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
    }
}
