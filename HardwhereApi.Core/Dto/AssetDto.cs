using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwhereApi.Core.Models;

namespace HardwhereApi.Core.Dto
{
    public class AssetDto
    {
        public int Id { get; set; }
        public int AssetTypeId { get; set; }
        public ICollection<AssetPropertyDto> AssetProperties { get; set; } 

        public AssetTypeDto AssetType { get; set; }
    }
}
