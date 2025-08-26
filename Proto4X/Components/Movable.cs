using Microsoft.Xna.Framework;
using Proto4x.Utils;
using Proto4x.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto4x.Components
{
    internal struct Position
    {
        public Vector2 _Position;

        public Position(Vector2 position) 
        { 
            _Position = position; 
        }
    }

    internal struct Movable
    {
        public Vector2 Position;
        public Vector2 Velocity = new Vector2();
        public Vector2 Acceleration = new Vector2();

        public float Rotation = 0;
        public float AngularVelocity = 0;
        public float AngularAcceleration = 0;

        public float Friction = 0.005f;

        public float MaxSpeed = 500;
        public float MaxThrust = 100;

        public float MaxAngularVelocity = (float)Math.PI / 4;
        public float MaxAngularAcceleration = (float)Math.PI / 8;

        public GameLayer GameLayer;

        public Movable(Vector2 position, GameLayer gameLayer)
        {
            Position = position;
            GameLayer = gameLayer;
        }

        void update(float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
		    if(Velocity.Length() > MaxSpeed) {
                Velocity.Normalize();
                Velocity *= MaxSpeed;
            }
            Velocity *= 1 - Friction;
            Position += Velocity * deltaTime;

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

            this.boundsCheck();
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

        void rotate(float rotation)
        {
            AngularAcceleration += rotation;
        }

        Vector2 getPositionAtTimeT(int time)
        {
            var xt = Position.X + Velocity.X * time + 0.5 * Acceleration.X * Math.Pow(time, 2);
            var yt = Position.Y + Velocity.Y * time + 0.5 * Acceleration.Y * Math.Pow(time, 2);
            return new ((float)xt, (float)yt);
        }

        private void boundsCheck()
        {
            if (Position.X > GameLayer.Bounds.Width)
            {
                Position.X = GameLayer.Bounds.Width;
                Velocity.X = 0;
            }
            else if (Position.X < 0)
            {
                Position.X = 0;
                Velocity.X = 0;
            }

            if (Position.Y > GameLayer.Bounds.Height)
            {
                Position.Y = GameLayer.Bounds.Height;

                Velocity.Y = 0;

            }
            else if (Position.Y < 0)
            {
                Position.Y = 0;
                Velocity.Y = 0;
            }
        }
    }
}
