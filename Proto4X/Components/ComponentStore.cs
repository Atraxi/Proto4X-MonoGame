using System.Collections.Generic;
using System.Diagnostics;

namespace Proto4x.Components
{
    public class ComponentStore<T> where T: ComponentBase
    {
        private readonly Dictionary<int, T> _components = [];

        public void Add(int id, T component)
        {
            _components[id] = component;
        }

        public IReadOnlyDictionary<int, T> GetAll()
        {
            return _components;
        }

        public T Get(int id)
        {
            Debug.Assert(_components.ContainsKey(id));
            return _components[id];
        }

        public void Remove(int id)
        {
            _components.Remove(id);
        }
    }
}
