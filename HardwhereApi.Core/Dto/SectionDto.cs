using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwhereApi.Core.Dto
{
    public class SectionDto
    {
        public int Id { get; set; }
        public int AssetTypeId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsGeneral { get; set; }
    }
}
