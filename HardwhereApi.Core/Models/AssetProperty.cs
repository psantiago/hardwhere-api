using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwhereApi.Core.Models
{
    public class AssetProperty
    {
        [Key]
        [Column(Order = 0)]
        public int AssetId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int PropertyId { get; set; }
        public string Value { get; set; }


        public virtual TypeProperty TypeProperty { get; set; }
        public virtual Asset Asset { get; set; }
    }
}
