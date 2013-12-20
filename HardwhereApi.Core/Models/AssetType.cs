using System.Collections.Generic;

namespace HardwhereApi.Core.Models
{
    public class AssetType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<TypeProperty> TypeProperties { get; set; }
    }
}
