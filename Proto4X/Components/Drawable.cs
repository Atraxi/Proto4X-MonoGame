using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Proto4x.Components;

namespace Proto4X.Components
{
    public struct Drawable(Rectangle? textureRegion)
    {
        public Rectangle? TextureRegion { get; set; } = textureRegion;
    }

    public interface IDrawableProvider : IComponentProvider
    {
        public Drawable[] Drawables { get; }
        public Texture2D Texture { get; }
    }
}
