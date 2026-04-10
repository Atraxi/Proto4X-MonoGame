using MonoGameLibrary.Components;
using MonoGameLibrary.Components.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameLibrary.Archetypes
{
    public class EntityBuilder
    {
        private ComponentTypeMask componentTypeMask = new();
        public ComponentTypeMask ComponentTypeMask => componentTypeMask;

        private readonly List<IComponentContainer> components = [];
        internal List<IComponentContainer> Components => components;

        public static EntityBuilder CreateEntity()
        {
            return new EntityBuilder();
        }

        public EntityBuilder With<T>(T payload) where T: struct
        {
            if(payload is IDependsOnOtherComponents dependsOn && !componentTypeMask.ContainsAll(ComponentTypeMask.FromTypes(dependsOn.GetComponentDependencies)))
            {
                throw new ArgumentException($"The new component payload of type {payload.GetType()} declares it depends on other component types, however those components have not yet been registered to this EntityBuild instance. Dependencies must be registered first.");
            }

            var componentTypeId = ComponentTypeRegistry.GetId(payload.GetType());
            componentTypeMask = componentTypeMask.With(ComponentTypeMask.FromTypeId(componentTypeId));

            components.Add(new ComponentContainerDetached<T>(payload));

            return this;
        }

        //TODO: These With(...) overloads should not be manually implemented here, instead components should have a GeneratedFrom flag and some generic discovery code to load that
        public EntityBuilder With(Motion payload)
        {
            With<Motion>(payload);

            var positionContainer = (ComponentContainerDetached<Position>) Components.Single(component => component as ComponentContainerDetached<Position> != null);
            With(new VirtualPredictedPosition(positionContainer.Component));

            return this;
        }

        //TODO I probably shouldn't always run. There will eventually be non-colliding drawables almost certainly. Not sure how I want to specify that yet. Colliders might just need to become explicitly added
        public EntityBuilder With(Drawable payload)
        {
            With<Drawable>(payload);

            With(new Collider(payload));

            return this;
        }
    }
}