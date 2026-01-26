using Microsoft.Xna.Framework;
using Proto4x.Components;
using Proto4x.World;
using System;

namespace Proto4X.Systems
{
    public class Movement
    {
        public Position Position;
        public Vector2 Velocity = new();
        public Vector2 Acceleration = new();

        public float AngularVelocity = 0;
        public float AngularAcceleration = 0;

        public float Friction = 0.005f;

        public float MaxSpeed = 500;
        public float MaxThrust = 100;

        public float MaxAngularVelocity = (float)Math.PI / 4;
        public float MaxAngularAcceleration = (float)Math.PI / 8;

        void Update(float deltaTime, GameLayer gameLayer)
        {
            Velocity += Acceleration * deltaTime;
            if (Velocity.Length() > MaxSpeed)
            {
                Velocity.Normalize();
                Velocity *= MaxSpeed;
            }
            Velocity *= 1 - Friction;
            Position.Location += Velocity * deltaTime;

            AngularVelocity += AngularAcceleration * deltaTime;
            if (AngularVelocity > MaxAngularVelocity)
            {
                AngularVelocity = MaxAngularVelocity;
            }
            AngularVelocity *= 1 - Friction;
            Position.Rotation += AngularVelocity * deltaTime;
            if (Position.Rotation > Math.PI * 2)
            {
                Position.Rotation -= (float)(Math.PI * 2);
            }
            else if (Position.Rotation < 0)
            {
                Position.Rotation += (float)(Math.PI * 2);
            }

            this.BoundsCheck(gameLayer.Bounds);
        }

        void AccelerateRelativeToRotation(Vector2 thrust)
        {
            Accelerate(thrust * Position.Rotation);
        }

        void Accelerate(Vector2 acceleration)
        {
            Acceleration = acceleration;

            if (Acceleration.Length() > MaxThrust)
            {
                Acceleration = Vector2.Normalize(Acceleration) * MaxThrust;
            }
        }

        void Rotate(float rotation)
        {
            AngularAcceleration += rotation;
        }

        Vector2 GetPositionAtTimeT(int time)
        {
            var xt = Position.Location.X + Velocity.X * time + 0.5 * Acceleration.X * Math.Pow(time, 2);
            var yt = Position.Location.Y + Velocity.Y * time + 0.5 * Acceleration.Y * Math.Pow(time, 2);
            return new((float)xt, (float)yt);
        }

        private void BoundsCheck(Rectangle bounds)
        {
            if (Position.Location.X > bounds.Width)
            {
                Position.Location = Position.Location with { X = bounds.Width };
                Velocity.X = 0;
            }
            else if (Position.Location.X < 0)
            {
                Position.Location = Position.Location with { X = 0 };
                Velocity.X = 0;
            }

            if (Position.Location.Y > bounds.Height)
            {
                Position.Location = Position.Location with { Y = bounds.Height };

                Velocity.Y = 0;

            }
            else if (Position.Location.Y < 0)
            {
                Position.Location = Position.Location with { Y = 0 };
                Velocity.Y = 0;
            }
        }
    }
}
