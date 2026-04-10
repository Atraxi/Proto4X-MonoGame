using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components;
using MonoGameLibrary.Components.Infrastructure;
using MonoGameLibrary.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proto4X.Systems
{
    public class VirtualMovementSystem : MovementSystem
    {
        public override ComponentTypeMask RequiredComponentProviders => ComponentTypeMask.FromTypes(
            typeof(Motion),
            typeof(VirtualPredictedPosition)
            );

        public override void Update(GameTime gameTime, List<ArchetypeChunk> archetypeChunks)
        {
            foreach (var archetypeChunk in archetypeChunks)
            {
                for (var entityIndex = 0; entityIndex < archetypeChunk.EntityCount; entityIndex++)
                {
                    ref var position = ref archetypeChunk.Get<VirtualPredictedPosition>(entityIndex);
                    ref var motion = ref archetypeChunk.Get<Motion>(entityIndex);

                    ref var location = ref position.Location;
                    ref var velocity = ref motion.Velocity;
                    ref var acceleration = ref motion.Acceleration;

                    ref var rotation = ref position.Rotation;
                    ref var angularVelocity = ref motion.AngularVelocity;
                    ref var angularAcceleration = ref motion.AngularAcceleration;

                    MoveEntity(gameTime, ref location, ref velocity, ref acceleration, ref rotation, ref angularVelocity, ref angularAcceleration);
                }
            }
        }
    }
}
