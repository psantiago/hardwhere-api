using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwhereApi.Core.Models
{
    public class Section
    {
        public int Id { get; set; }
        public int AssetTypeId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsGeneral { get; set; }

        public virtual AssetType AssetType { get; set; }
        public virtual ICollection<TypeProperty> TypeProperties { get; set; }
    }
}
