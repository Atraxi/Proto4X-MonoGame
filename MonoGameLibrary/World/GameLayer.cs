using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Archetypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameLibrary.World
{
    public class GameLayer(int xPos, int yPos, int width, int height)
    {
        public readonly Dictionary<Texture2D, List<ArchetypeBase>> Archetypes = new();
        public readonly Rectangle Bounds = new(xPos, yPos, width, height);

        public List<ArchetypeBase> QueryRelevantArchetypes(IEnumerable<Type> componentProviderTypes)
        {
            List<ArchetypeBase> result = [];
            foreach(var archetypesPerTextureSheet in Archetypes)
            {
                foreach (ArchetypeBase archetype in archetypesPerTextureSheet.Value)
                {
                    if (componentProviderTypes.All(componentProviderType => 
                        componentProviderType.IsAssignableFrom(archetype.GetType())))
                    {
                        result.Add(archetype);
                    }
                }
            }
            return result;
        }
        /* Suggested by ChatGPT as a structure when comparing pre-made ECS libraries. I think I like it? But I don't know if it's worth converting to it
          world.GetEntities()
             .With<Position>()
             .With<Velocity>()
        */

        //public void Draw(SpriteBatch spriteBatch, Rectangle viewport)
        //{
        //    spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(Bounds.X, Bounds.Y, 0/*I think? 2d game not 3d. Confirm*/));

        //    spriteBatch.End();
        //}

        
    }
}