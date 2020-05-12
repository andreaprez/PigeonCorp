using System;
using System.Collections.Generic;

namespace PigeonCorp.ValueModifiers
{
    public class ValueModifiersRepository
    {
        private readonly List<BaseValueModifiers> _list;

        public ValueModifiersRepository()
        {
            _list = new List<BaseValueModifiers>();
        }
        
        public void Add(BaseValueModifiers item)
        {
            _list.Add(item);
        }
        
        public BaseValueModifiers Get<T>()
        where T : BaseValueModifiers
        {
            foreach (var valueModifiersClass in _list)
            {
                var type = valueModifiersClass.GetType();
                if (type.Name == typeof(T).Name)
                {
                    return valueModifiersClass;
                }
            }
            
            throw new Exception("No ValueModifiers class found with type: " + typeof(T).Name);
        }

    }
}