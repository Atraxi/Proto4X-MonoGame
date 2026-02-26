using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components;
using MonoGameLibrary.Utils;
using System;
using System.Collections.Generic;

namespace MonoGameLibrary.World
{
    public class GameWorld(int xPos, int yPos, int width, int height)
    {
        public readonly Dictionary<ComponentTypeMask, Archetype> Archetypes = [];
        public readonly Rectangle Bounds = new(xPos, yPos, width, height);

        internal List<ArchetypeChunk> QueryRelevantComponentArrays(ComponentTypeMask componentProviderTypes)
        {
            List<ArchetypeChunk> result = [];
            
            foreach (var archetypesKeyPair in Archetypes)
            {
                if (archetypesKeyPair.Key.ContainsAll(componentProviderTypes))
                {
                    var archetype = archetypesKeyPair.Value;
                    var archetypeChunk = archetype.GetMatchingComponents(componentProviderTypes);

                    result.Add(archetypeChunk);
                }
            }
            return result;
        }

        public void AddEntity(EntityBuilder entityBuilder)
        {
            Archetypes.TryGetValue(entityBuilder.ComponentTypeMask, out var archetype);
            if (archetype == null)
            {
                archetype = new Archetype(50, entityBuilder);
                Archetypes.Add(entityBuilder.ComponentTypeMask, archetype);
            }
            else
            {
                archetype.AddEntity(entityBuilder);
            }
        }

        /* Suggested by ChatGPT as a structure when comparing pre-made ECS libraries. I think I like it? But I don't know if it's worth converting to it
          world.GetEntities()
             .With<Position>()
             .With<Velocity>()
        */
    }
}