using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwhereApi.Core.Utilities;

namespace HardwhereApi.Core.Dto
{
    /// <summary>
    /// This inherits from SuperDynamic to give it a nicer name, and possibly additional properties/methods/etc.
    /// </summary>
    [Serializable]
    public class DynamicAssetDto : SuperDynamic
    {
        public DynamicAssetDto(Dictionary<string, object> properties) : base(properties)
        {
        }
    }
}
