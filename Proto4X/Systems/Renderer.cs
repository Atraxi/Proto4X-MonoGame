using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components;
using MonoGameLibrary.Systems;
using Proto4X.Components;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public class Renderer : SystemBase, IDrawSystem
    {
        public override ComponentTypeMask RequiredComponentProviders => ComponentTypeMask.FromTypes(
            typeof(Position),
            typeof(Drawable)
            );

        public void Draw(SpriteBatch spriteBatch, Rectangle viewport, List<ArchetypeChunk> archetypeChunks)
        {
            foreach(var archetypeChunk in archetypeChunks)
            {
                var drawables = archetypeChunk.Get<Drawable>();
                var entityPositions = archetypeChunk.Get<Position>();

                for (var entityIndex = 0; entityIndex < archetypeChunk.EntityCount; entityIndex++)
                {
                    ref var drawable = ref drawables[entityIndex];
                    ref var position = ref entityPositions[entityIndex];

                    if(!viewport.Contains(position.Location))
                    {
                        continue;
                    }

                    spriteBatch.Draw(SpriteLibrary.GetTexture(drawable.SpriteId), position.Location, drawable.TextureRegion, Color.White, position.Rotation, Vector2.Zero, 1, SpriteEffects.None, 1);
                }
            }
        }
    }
}