using System.Collections.Generic;

namespace HardwhereApi.Core.Models
{
    public class TypeProperty
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public int AssetTypeId { get; set; }

        public int SectionId { get; set; }
        public int ValueTypeId { get; set; }
        public int Order { get; set; }

        public virtual AssetType AssetType { get; set; }
        public virtual Section Section { get; set; }
        public virtual ValueType ValueType { get; set; }

        public virtual ICollection<AssetProperty> AssetProperties { get; set; }
    }
}
