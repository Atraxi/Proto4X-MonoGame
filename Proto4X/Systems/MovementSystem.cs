using Microsoft.Xna.Framework;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Systems;
using Proto4x.Components;
using System;
using System.Collections.Generic;

namespace Proto4X.Systems
{
    public class MovementSystem : SystemBase, IUpdateSystem
    {
        public float Friction = 0.005f;

        public float MaxSpeed = 500;
        public float MaxThrust = 100;

        public float MaxAngularVelocity = (float)Math.PI / 4;
        public float MaxAngularAcceleration = (float)Math.PI / 8;

        public override IEnumerable<Type> RequiredComponentProviders => [
            typeof(IMotionProvider),
            typeof(IPositionProvider)];

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

        public void Update(GameTime gameTime, List<ArchetypeBase> archetypes)
        {
            foreach (ArchetypeBase archetype in archetypes)
            {
                var motionProvider = (IMotionProvider)archetype;
                var positionProvider = (IPositionProvider)archetype;

                for (int i = 0; i < positionProvider.Positions.Length; i++)
                {
                    var position = positionProvider.Positions[i];
                    var velocity = motionProvider.MotionValues[i].Velocity;
                    var acceleration = motionProvider.MotionValues[i].Acceleration;
                    var angularVelocity = motionProvider.MotionValues[i].AngularVelocity;
                    var angularAcceleration = motionProvider.MotionValues[i].AngularAcceleration;

                    velocity += acceleration * gameTime.ElapsedGameTime.Ticks;
                    if (velocity.Length() > MaxSpeed)
                    {
                        velocity.Normalize();
                        velocity *= MaxSpeed;
                    }
                    velocity *= 1 - Friction;
                    position.Location += velocity * gameTime.ElapsedGameTime.Ticks;

                    angularVelocity += angularAcceleration * gameTime.ElapsedGameTime.Ticks;
                    if (angularVelocity > MaxAngularVelocity)
                    {
                        angularVelocity = MaxAngularVelocity;
                    }
                    angularVelocity *= 1 - Friction;
                    position.Rotation += angularVelocity * gameTime.ElapsedGameTime.Ticks;
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
    }
}
