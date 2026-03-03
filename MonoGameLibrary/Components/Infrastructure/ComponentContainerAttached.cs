using MonoGameLibrary.Archetypes;
using System;

namespace MonoGameLibrary.Components.Infrastructure
{
    public interface IComponentContainer
    {
        void WriteComponentByIndex(Archetype archetype, int entityIndex);
        void CreateComponentColumn(Archetype archetype, int columnIndex, int initialCapacity);
        Type GetComponentType();
        void SetComponent(int entityId);
    }

    public class ComponentContainerDetached<T>(T payload) : IComponentContainer where T : struct
    {
        protected readonly T Payload = payload;

        public T Component => Payload;

        public Type GetComponentType() => typeof(T);

        public void CreateComponentColumn(Archetype archetype, int columnIndex, int initialCapacity)
        {
            archetype.CreateComponentColumn<T>(columnIndex, initialCapacity);
        }

        public void WriteComponentByIndex(Archetype archetype, int entityIndex)
        {
            archetype.WriteComponentByIndex(Payload, entityIndex);
        }

        public void SetComponent(int entityId)
        {
            throw new NotImplementedException();
        }
    }

    public class ComponentContainerAttached<T>(T payload, Archetype archetype) : ComponentContainerDetached<T>(payload) where T : struct
    {
        public void WriteComponentById(int entityId, T newPayload)
        {
            archetype.WriteComponentById(newPayload, entityId);
        }
    }
}
