using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dia.Entities
{
    public class Exam
    {
        public long ExamId { get; set; }
        public User ExamUser { get; set; }
        public Section ExamSection { get; set; }
        public DateTime FilledFrom { get; set; }
        public DateTime FilledTo { get; set; }
        public IEnumerable<Meter> Meters { get; set; }
        public IEnumerable<Line> Lines { get; set; }
        public Dictionary<string, List<string>> Titles { get; set; }
}
}
