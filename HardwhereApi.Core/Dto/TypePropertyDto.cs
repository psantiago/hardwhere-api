﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwhereApi.Core.Dto
{
    public class TypePropertyDto
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public int Order { get; set; }

        public int SectionId { get; set; }

        public int ValueTypeId { get; set; }
    }
}
