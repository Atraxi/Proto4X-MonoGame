using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components;
using MonoGameLibrary.Systems;
using Proto4X.Components;
using System;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public class MovementSystem : SystemBase, IUpdateSystem
    {
        public float Friction = 0.001f;

        public float MaxSpeed = 500;
        public float MaxThrust = 100;

        public float MaxAngularVelocity = (float)Math.PI / 4;
        public float MaxAngularAcceleration = (float)Math.PI / 8;

        public override ComponentTypeMask RequiredComponentProviders => ComponentTypeMask.FromTypes(
            typeof(Motion),
            typeof(Position)
            );

        public void Update(GameTime gameTime, List<ArchetypeChunk> archetypeChunks)
        {
            foreach (var archetypeChunk in archetypeChunks)
            {
                var motions = archetypeChunk.Get<Motion>();
                var positions = archetypeChunk.Get<Position>();

                for (var entityIndex = 0; entityIndex < archetypeChunk.EntityCount; entityIndex++)
                {
                    ref var position = ref positions[entityIndex];
                    ref var location = ref position.Location;
                    ref var velocity = ref motions[entityIndex].Velocity;
                    ref var acceleration = ref motions[entityIndex].Acceleration;
                    ref var angularVelocity = ref motions[entityIndex].AngularVelocity;
                    ref var angularAcceleration = ref motions[entityIndex].AngularAcceleration;

                    velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (velocity.Length() > MaxSpeed)
                    {
                        velocity.Normalize();
                        velocity *= MaxSpeed;
                    }
                    velocity *= 1 - Friction;
                    location += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    angularVelocity += angularAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (angularVelocity > MaxAngularVelocity)
                    {
                        angularVelocity = MaxAngularVelocity;
                    }
                    angularVelocity *= 1 - Friction;
                    position.Rotation += angularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (position.Rotation > Math.PI * 2)
                    {
                        position.Rotation -= (float)(Math.PI * 2);
                    }
                    else if (position.Rotation < 0)
                    {
                        position.Rotation += (float)(Math.PI * 2);
                    }
                }
            }
        }

        //void AccelerateRelativeToRotation(Vector2 thrust)
        //{
        //    Accelerate(thrust * Position.Rotation);
        //}

        //void Accelerate(Vector2 acceleration)
        //{
        //    Acceleration = acceleration;

        //    if (Acceleration.Length() > MaxThrust)
        //    {
        //        Acceleration = Vector2.Normalize(Acceleration) * MaxThrust;
        //    }
        //}

        //void Rotate(float rotation)
        //{
        //    AngularAcceleration += rotation;
        //}

        //Vector2 GetPositionAtTimeT(int time)
        //{
        //    var xt = Position.Location.X + Velocity.X * time + 0.5 * Acceleration.X * Math.Pow(time, 2);
        //    var yt = Position.Location.Y + Velocity.Y * time + 0.5 * Acceleration.Y * Math.Pow(time, 2);
        //    return new((float)xt, (float)yt);
        //}

        //private void BoundsCheck(Rectangle bounds)
        //{
        //    if (Position.Location.X > bounds.Width)
        //    {
        //        Position.Location = Position.Location with { X = bounds.Width };
        //        Velocity.X = 0;
        //    }
        //    else if (Position.Location.X < 0)
        //    {
        //        Position.Location = Position.Location with { X = 0 };
        //        Velocity.X = 0;
        //    }

        //    if (Position.Location.Y > bounds.Height)
        //    {
        //        Position.Location = Position.Location with { Y = bounds.Height };

        //        Velocity.Y = 0;

        //    }
        //    else if (Position.Location.Y < 0)
        //    {
        //        Position.Location = Position.Location with { Y = 0 };
        //        Velocity.Y = 0;
        //    }
        //}
    }
}
