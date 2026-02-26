using Microsoft.Xna.Framework;
using MonoGameLibrary.Components;
using System;

namespace Proto4X.Components
{
    public struct Motion(Vector2 velocity,
        Vector2 acceleration = default,
        float angularVelocity = 0,
        float angularAcceleration = 0)
    {
        public Vector2 Velocity = velocity;
        public Vector2 Acceleration = acceleration;
        public float AngularVelocity = angularVelocity;
        public float AngularAcceleration = angularAcceleration;
    }
}
