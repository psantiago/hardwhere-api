using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwhereApi.Core.Models;
using HardwhereApi.Core.Utilities;

namespace HardwhereApi.Core.Dto
{
    public class DynamicAssetPropertyDto
    {
        public DynamicAssetPropertyDto(
            TypeProperty typeProperty,
            Dictionary<int, Section> sections,
            object currentAssetPropertyValue = null)
        {
            var currentSection = sections[typeProperty.SectionId];

            Value = currentAssetPropertyValue;
            SectionName = currentSection.Name;
            SectionOrder = currentSection.Order;
            PropertyOrder = typeProperty.Order;
        }

        public object Value { get; set; }
        public string SectionName { get; set; }
        public int SectionOrder { get; set; }
        public int PropertyOrder { get; set; }
    }
}
