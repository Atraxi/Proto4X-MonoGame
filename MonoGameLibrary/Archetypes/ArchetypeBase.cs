using System;

namespace MonoGameLibrary.Archetypes
{
    public abstract class ArchetypeBase<TPayload>(int initialCapacity) : ArchetypeBase(initialCapacity)
    {
        public void AddEntity(int entityId, TPayload payload)
        {
            int index = count++;

            if (index == DenseToEntity.Length)
            {
                Grow();
            }

            DenseToEntity[index] = entityId;
            EntityToDense[entityId] = index;

            OnAddEntity(index, payload);
        }
        protected abstract void OnAddEntity(int entityId, TPayload payload);

        public void RemoveEntity(int entityId)
        {
            int index = EntityToDense[entityId];
            int last = --count;
    
            if (index != last)
            {
                int movedEntity = DenseToEntity[last];
                DenseToEntity[index] = movedEntity;
                EntityToDense[movedEntity] = index;
            }

            OnRemoveEntity(index);
        }
        protected abstract void OnRemoveEntity(int entityId);
    }

    public abstract class ArchetypeBase(int initialCapacity)
    {
        protected int[] DenseToEntity = new int[initialCapacity];
        protected int[] EntityToDense = new int[initialCapacity];

        public int Count => count;
        protected int count;

        public ReadOnlySpan<int> EntityIds => entityIds;
        private int[] entityIds = new int[initialCapacity];

        protected void Grow()
        {
            int newSize = DenseToEntity.Length * 2;

            Array.Resize(ref DenseToEntity, newSize);
            Array.Resize(ref EntityToDense, newSize);
            Array.Resize(ref entityIds, newSize);

            Grow(newSize);
        }

        protected abstract void Grow(int newSize);
    }
}
