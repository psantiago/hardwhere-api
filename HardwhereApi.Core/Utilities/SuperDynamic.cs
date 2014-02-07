using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HardwhereApi.Core.Utilities
{
    [Serializable]
    public class SuperDynamic : DynamicObject, ISerializable
    {
        private readonly Dictionary<string, object> _properties;

        public SuperDynamic(Dictionary<string, object> properties)
        {
            _properties = properties;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                result = _properties[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                _properties[binder.Name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in _properties)
            {
                info.AddValue(kvp.Key, kvp.Value);
            }
        }
    }
}
