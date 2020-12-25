using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dia.Entities;

namespace Dia.IDAC
{
    public interface IDAContracts
    {
        IEnumerable<User> GetUsers();
        IEnumerable<Section> GetSections();
        IEnumerable<Meter> GetMeters(int sectionId);
        IEnumerable<Meter> GetReadings(long examId);
        IEnumerable<Exam> GetExams(DateTime date);
        bool AddReadings(IEnumerable<Meter> readings, long examId);
        long AddExam(Exam exam);
        void DeleteExam(long examId);
    }
}
