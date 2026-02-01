using Microsoft.Xna.Framework;
using MonoGameLibrary.Components;
using System;

namespace Proto4x.Components
{
    public struct Motion(Vector2 velocity,
        Vector2 acceleration = default,
        float angularVelocity = 0,
        float angularAcceleration = 0)
    {
        public Vector2 Velocity { get; } = velocity;
        public Vector2 Acceleration { get; } = acceleration;
        public float AngularVelocity { get; } = angularVelocity;
        public float AngularAcceleration { get; } = angularAcceleration;
    }

    public interface IMotionProvider : IComponentProvider {
        public Motion[] MotionValues { get; }
    }
}
