using System.Collections.Generic;

namespace HardwhereApi.Core.Models
{
    public class TypeProperty
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public int AssetTypeId { get; set; }

        public virtual AssetType AssetType { get; set; }

        public virtual ICollection<AssetProperty> AssetProperties { get; set; }
    }
}
