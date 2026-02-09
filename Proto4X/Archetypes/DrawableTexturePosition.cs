using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Archetypes;
using Proto4x.Components;
using Proto4X.Components;
using System;

namespace Proto4X.Archetypes
{
    public class DrawableTexturePosition(int initialCapacity) : ArchetypeBase<(
            Vector2 position,
            float radiansRotation, 
            Texture2D texture, 
            Rectangle? textureRegion)>(initialCapacity),
        IPositionProvider,
        IDrawableProvider
    {
        public Position[] Positions => positions;
        private Position[] positions = new Position[initialCapacity];

        public Drawable[] Drawables => drawables;
        private Drawable[] drawables = new Drawable[initialCapacity];

        public Texture2D Texture { get; set; }

        protected override void Grow(int newSize)
        {
            Array.Resize(ref positions, newSize);
            Array.Resize(ref drawables, newSize);
        }

        protected override void OnAddEntity(int index, (Vector2 position, float radiansRotation, Texture2D texture, Rectangle? textureRegion) payload) {
            Positions[index] = new(payload.position, payload.radiansRotation);
            Drawables[index] = new(payload.textureRegion);
            Texture = payload.texture;
        }

        protected override void OnRemoveEntity(int index)
        {
            int last = Count;

            Positions[index] = Positions[last];
            Drawables[index] = Drawables[last];
        }
    }
}
