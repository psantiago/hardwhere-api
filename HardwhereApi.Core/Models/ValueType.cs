using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwhereApi.Core.Models
{
    public class ValueType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Regex { get; set; }
        public string ErrorMessage { get; set; }
    }
}
