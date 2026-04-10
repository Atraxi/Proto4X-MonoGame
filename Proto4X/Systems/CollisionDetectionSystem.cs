using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components;
using MonoGameLibrary.Components.Infrastructure;
using MonoGameLibrary.Systems;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    internal class CollisionDetectionSystem : SystemBase, IUpdateSystem
    {
        public override ComponentTypeMask RequiredComponentProviders => ComponentTypeMask.FromTypes(
            typeof(VirtualPredictedPosition),
            typeof(Collider)
            );

        public void Update(GameTime gameTime, List<ArchetypeChunk> archetypeChunks)
        {
            foreach (var archetypeChunk in archetypeChunks)
            {
                for (var entityIndex = 0; entityIndex < archetypeChunk.EntityCount; entityIndex++)
                {
                    ref var position = ref archetypeChunk.Get<VirtualPredictedPosition>(entityIndex);
                    ref var location = ref position.Location;
                    ref var willCollide = ref position.WillCollideWith;

                    ref var collider = ref archetypeChunk.Get<Collider>(entityIndex);

                    foreach (var archetypeChunkOther in archetypeChunks)
                    {
                        for (var entityIndexOther = 0; entityIndex < archetypeChunkOther.EntityCount; entityIndexOther++)
                        {
                            ref var positionOther = ref archetypeChunkOther.Get<VirtualPredictedPosition>(entityIndexOther);
                            ref var locationOther = ref position.Location;
                            ref var willCollideOther = ref position.WillCollideWith;

                            ref var colliderOther = ref archetypeChunk.Get<Collider>(entityIndex);

                            //Ewww. I hate it, I hate it, I hate it, 4 nested loops, n^2 test.
                            //This WILL be replaced with a spatial indexing system, but I need profiling to know which approach will work best in my archetecture

                            var isCollisionPredicted = collider.MaximumIntersectionRadiusSquared + colliderOther.MaximumIntersectionRadiusSquared >= (location - locationOther).LengthSquared();
                            //TODO: it's possible to have multiple entities collide in the same frame, maybe we should calculate which will collide first and only store that?
                            //    I think it's safe to always assume and compute collisions in pairs though, a 3 way collision should always be reasonable to compute in multiple stages of paired collisions
                            willCollide = isCollisionPredicted;
                            willCollideOther = isCollisionPredicted;
                        }
                    }
                }
            }
        }
    }
}
