using Microsoft.Xna.Framework;
using System.Linq;

namespace MonoGameLibrary.Components
{
    public struct Collider
    {
        //public Point[] Vertices; //TODO when I have non-rectangular collision bounds, or if I have colliders that don't match the sprite bounds
        
        //This is for fast and simple distance checks before computing the real collision geometry
        public float MaximumIntersectionRadiusSquared;

        public Collider(Drawable drawable)
        {
            var bounds = drawable.TextureRegion ?? SpriteLibrary.GetTexture(drawable.SpriteId).Bounds;

            MaximumIntersectionRadiusSquared = new Point[] {
                bounds.Location,
                new(bounds.Right, bounds.Top),
                new(bounds.Right, bounds.Bottom),
                new(bounds.Left, bounds.Bottom)
            }.Max(vertex => (vertex - bounds.Center).ToVector2().LengthSquared());
        }
    }
}
