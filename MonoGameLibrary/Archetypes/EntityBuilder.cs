using MonoGameLibrary.Components.Infrastructure;
using System.Collections.Generic;

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
            var componentTypeId = ComponentTypeRegistry.GetId(payload.GetType());
            componentTypeMask = componentTypeMask.With(ComponentTypeMask.FromTypeId(componentTypeId));

            components.Add(new ComponentContainerDetached<T>(payload));

            return this;
        }
    }
}