using Microsoft.Xna.Framework;
using Proto4x.World;
using System;

namespace Proto4x.Components
{
    public record Position : ComponentBase
    {
        public Vector2 Location { get; protected set; }

        public float Rotation { get; protected set; }

        public Position(int id, Vector2 location, float rotation = 0) : base(id)
        {
            Location = location;
            Rotation = rotation;
        }
    }

    public record Movable(int id, Vector2 Location, float Rotation, GameLayer GameLayer) : Position(id, Location, Rotation)
    {
        public Vector2 Velocity = new();
        public Vector2 Acceleration = new();
        
        public float AngularVelocity = 0;
        public float AngularAcceleration = 0;

        public float Friction = 0.005f;

        public float MaxSpeed = 500;
        public float MaxThrust = 100;

        public float MaxAngularVelocity = (float)Math.PI / 4;
        public float MaxAngularAcceleration = (float)Math.PI / 8;

        void Update(float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
		    if(Velocity.Length() > MaxSpeed) {
                Velocity.Normalize();
                Velocity *= MaxSpeed;
            }
            Velocity *= 1 - Friction;
            Location += Velocity * deltaTime;

            AngularVelocity += AngularAcceleration * deltaTime;
            if (AngularVelocity > MaxAngularVelocity)
            {
                AngularVelocity = MaxAngularVelocity;
            }
            AngularVelocity *= 1 - Friction;
            Rotation += AngularVelocity * deltaTime;
            if (Rotation > Math.PI * 2)
            {
                Rotation -= (float)(Math.PI * 2);
            }
            else if (Rotation < 0)
            {
                Rotation += (float)(Math.PI * 2);
            }

            this.BoundsCheck();
          }

        void AccelerateRelativeToRotation(Vector2 thrust)
        {
            Accelerate(thrust * Rotation);
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
            var xt = Location.X + Velocity.X * time + 0.5 * Acceleration.X * Math.Pow(time, 2);
            var yt = Location.Y + Velocity.Y * time + 0.5 * Acceleration.Y * Math.Pow(time, 2);
            return new ((float)xt, (float)yt);
        }

        private void BoundsCheck()
        {
            if (Location.X > GameLayer.Bounds.Width)
            {
                Location = Location with { X = GameLayer.Bounds.Width };
                Velocity.X = 0;
            }
            else if (Location.X < 0)
            {
                Location = Location with { X = 0 };
                Velocity.X = 0;
            }

            if (Location.Y > GameLayer.Bounds.Height)
            {
                Location = Location with { Y = GameLayer.Bounds.Height };

                Velocity.Y = 0;

            }
            else if (Location.Y < 0)
            {
                Location = Location with { Y = 0 };
                Velocity.Y = 0;
            }
        }
    }
}
