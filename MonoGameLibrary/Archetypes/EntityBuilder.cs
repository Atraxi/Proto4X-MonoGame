using MonoGameLibrary.Components;
using System;
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

            components.Add(new ComponentContainer<T>(payload));

            return this;
        }

        internal interface IComponentContainer
        {
            void WriteComponent(Archetype archetype, int entityIndex);

            void CreateComponentColumn(Archetype archetype, int columnIndex, int initialCapacity);

        }
        internal struct ComponentContainer<T>(T payload) : IComponentContainer where T : struct
        {
            public readonly T Component => payload;

            public static Type GetComponentType() => typeof(T);

            public readonly void CreateComponentColumn(Archetype archetype, int columnIndex, int initialCapacity)
            {
                archetype.CreateComponentColumn<T>(columnIndex, initialCapacity);
            }

            public readonly void WriteComponent(Archetype archetype, int entityIndex)
            {
                archetype.WriteComponent(payload, entityIndex);
            }
        }
    }
}