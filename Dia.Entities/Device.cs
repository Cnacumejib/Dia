﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dia.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Meter> Meters { get; set; }
    }
}
