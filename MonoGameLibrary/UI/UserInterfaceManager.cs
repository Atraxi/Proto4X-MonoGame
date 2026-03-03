using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Components.Infrastructure;
using MonoGameLibrary.Input;
using MonoGameLibrary.World;
using Proto4X.Components;
using System;
using System.Collections.Generic;

namespace MonoGameLibrary.UI
{
    public class UserInterfaceManager(GameWorld gameWorld)
    {
        private const int ACCELERATION = 500;
        private const int ANGULAR_ACCELERATION = 1000;
        private int selectedEntityId = 0;
        private Dictionary<Type, IComponentContainer>? selectedEntityComponents = null;

        public void Update()
        {
            UpdateAcceleration();
        }

        private void UpdateAcceleration()
        {
            if (selectedEntityId != -1 && gameWorld.HasEntity(selectedEntityId))
            {
                selectedEntityComponents = gameWorld.GetComponentsForEntity(selectedEntityId);
                if(selectedEntityComponents.TryGetValue(typeof(Motion), out var maybeMotion) &&
                    selectedEntityComponents.TryGetValue(typeof(Position), out var maybePosition))
                {
                    var newAcceleration = new Vector2();
                    var newAngularAcceleration = 0f;
                    if (InputManager.Instance.KeyboardInfo.IsKeyDown(Keys.W))
                    {
                        newAcceleration += new Vector2(0, -1);
                    }
                    if (InputManager.Instance.KeyboardInfo.IsKeyDown(Keys.S))
                    {
                        newAcceleration += new Vector2(0, 1);
                    }
                    if (InputManager.Instance.KeyboardInfo.IsKeyDown(Keys.A))
                    {
                        newAcceleration += new Vector2(-1, 0);
                    }
                    if (InputManager.Instance.KeyboardInfo.IsKeyDown(Keys.D))
                    {
                        newAcceleration += new Vector2(1, 0);
                    }
                    if (InputManager.Instance.KeyboardInfo.IsKeyDown(Keys.Q))
                    {
                        newAngularAcceleration -= float.DegreesToRadians(ANGULAR_ACCELERATION);
                    }
                    if (InputManager.Instance.KeyboardInfo.IsKeyDown(Keys.E))
                    {
                        newAngularAcceleration += float.DegreesToRadians(ANGULAR_ACCELERATION);
                    }

                    if (newAcceleration != Vector2.Zero && maybePosition is ComponentContainerAttached<Position> positionContainer)
                    {
                        newAcceleration.Rotate(positionContainer.Component.Rotation);
                        newAcceleration = Vector2.Normalize(newAcceleration) * ACCELERATION;
                    }

                    if (maybeMotion is ComponentContainerAttached<Motion> motionContainer)
                    {
                        var newMotion = motionContainer.Component with
                        {
                            Acceleration = newAcceleration,
                            AngularAcceleration = newAngularAcceleration,
                        };
                        motionContainer.WriteComponentById(selectedEntityId, newMotion);
                    }
                }
            }
        }
    }
}
