using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dia.Entities
{
    public class Meter
    {
        public long MeterId { get; set; }
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
        public string DeviceName { get; set; }
        public string ElementName { get; set; }
        public string LineName { get; set; }
        public float? Value { get; set; }
    }
}
