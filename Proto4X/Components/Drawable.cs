using Microsoft.Xna.Framework;
using MonoGameLibrary.Components;

namespace Proto4X.Components
{
    public struct Drawable(int spriteId, Rectangle? textureRegion)
    {
        public int SpriteId { get; set; } = spriteId;
        public Rectangle? TextureRegion { get; set; } = textureRegion;
    }
}
