using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwhereApi.Core.Dto
{
    public class AssetPropertyDto
    {
        public int AssetId { get; set; }
        public int TypePropertyId { get; set; }
        public string Value { get; set; }
    }
}
