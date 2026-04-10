using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Archetypes;
using MonoGameLibrary.Components;
using MonoGameLibrary.Components.Infrastructure;
using MonoGameLibrary.Input;
using MonoGameLibrary.Utils;
using MonoGameLibrary.World;
using System;
using System.Collections.Generic;

namespace MonoGameLibrary.UI
{
    public class UserInterfaceManager(GameWorld gameWorld)//TODO partial pt2 in Proto4X??
    {
        private const int ACCELERATION = 100;
        private const int ANGULAR_ACCELERATION = 100;
        private int selectedEntityId = 0;
        private Dictionary<Type, IComponentContainer>? selectedEntityComponents = null;
        private Rectangle currentViewport = Rectangle.Empty;

        public Camera Camera { get; internal set; } = new();

        public void Update(Rectangle viewportBounds, GameTime gameTime)
        {
            InputManager.Instance.Update();
            currentViewport = viewportBounds;
            UpdatePlayerAcceleration();
            UpdateUserSelection();

            Camera.Update(viewportBounds, gameTime);
        }

        internal Rectangle GetVisibleScreenBoundsAABB()
        {
            var viewportPoints = new Vector2[] {
                currentViewport.Location.ToVector2(),
                new(currentViewport.Right, currentViewport.Top),
                new(currentViewport.Right, currentViewport.Bottom),
                new(currentViewport.Left, currentViewport.Bottom)
            }.Transform(Camera.GetCameraTransform());

            return viewportPoints.ToAABB();
        }

        void UpdateUserSelection()
        {
            var mouse = InputManager.Instance.MouseInfo;
            if (mouse.WasButtonJustPressed(MouseButton.Right))
            {
                Core.Instance?.World.AddEntity(EntityBuilder.CreateEntity()
                    .With(new Position(mouse.Position.ToVector2().Transform(Matrix.Invert(Camera.GetCameraTransform()))))
                    .With(new Drawable(1, null)));
            }
        }

        private void UpdatePlayerAcceleration()
        {
            //TODO this is a placeholder for quick PoC testing. I'm probably going to want to have other parts of the game be able to register behaviours or something, not sure how I want to handle that yet
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
