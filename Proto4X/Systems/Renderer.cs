using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Systems;
using Proto4x.Components;
using Proto4X.Components;
using System;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public class Renderer : SystemBase, IDrawSystem
    {
        public override IEnumerable<Type> RequiredComponentProviders => [
            typeof(IPositionProvider),
            typeof(IDrawableProvider),
            ];

        public void Draw(SpriteBatch spriteBatch, Rectangle viewport, List<ArchetypeBase> archetypes)
        {
            foreach (var archetype in archetypes)
            {
                var drawableProvider = archetype as IDrawableProvider;
                var drawables = drawableProvider.Drawables;
                var entityPositions = (archetype as IPositionProvider).Positions;
                
                for (var index = 0; index < archetype.Count; index++)
                {
                    ref var drawable = ref drawables[index];
                    ref var position = ref entityPositions[index];

                    if(!viewport.Contains(position.Location))
                    {
                        continue;
                    }

                    spriteBatch.Draw(drawableProvider.Texture, position.Location, drawable.TextureRegion, Color.White, position.Rotation, Vector2.Zero, 1, SpriteEffects.None, 1);
                }
            }
        }
    }
}