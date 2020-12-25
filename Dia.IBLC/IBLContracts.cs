using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dia.Entities;

namespace Dia.IBLC
{
    public interface IBLContracts
    {
        IEnumerable<User> GetUsers();
        IEnumerable<Section> GetSections();
        IEnumerable<Exam> ReadReports(DateTime date);
        Exam GetActionTable(int sectionId);
        bool AddExam(Exam exam);
    }
}

