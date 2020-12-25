using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dia.IBLC;
using Dia.Entities;
using Dia.IDAC;
using Dia.BLL;

namespace Dia.BLL
{
    public class Logic : IBLContracts
    {

        public Logic()
        {
        }
                

        public Exam GetActionTable(int sectionId)
        {
            Exam exam = new Exam();
            exam.Meters = DataAccessProvider.DBAccessor.GetMeters(sectionId);
            GetTitles(exam);
            exam.Lines = GetLines(exam);
            return exam;
        }

        public IEnumerable<Exam> ReadReports(DateTime date)
        {
            IEnumerable<Exam> exams = DataAccessProvider.DBAccessor.GetExams(date);
            foreach (Exam exam in exams)
            {
                exam.Meters = DataAccessProvider.DBAccessor.GetReadings(exam.ExamId);
                GetTitles(exam);
                exam.Lines = GetLines(exam);
            }

            return exams;
        }

        private void GetTitles(Exam exam)
        {
            exam.Titles = new Dictionary<string, List<string>>();
            foreach (Meter meter in exam.Meters)
            {
                if (exam.Titles.ContainsKey(meter.DeviceName))
                {
                    if (!exam.Titles[meter.DeviceName].Contains(meter.ElementName))
                        exam.Titles[meter.DeviceName].Add(meter.ElementName);
                }
                else
                {
                    exam.Titles.Add(meter.DeviceName, new List<string>() { meter.ElementName });
                }
            }
        }

        private List<Line> GetLines(Exam exam)
        {
            List<Line> lines = new List<Line>();
            List<Device> devices = new List<Device>();
            List<Meter> meters = new List<Meter>();
            string currentLineName = string.Empty;
            string currentDeviceName = string.Empty;
            Meter lastMeter = exam.Meters.Last();
            foreach (Meter meter in exam.Meters)
            {
                if (currentLineName != meter.LineName)
                {
                    if (meters.Any())
                    {
                        devices.Add(new Device() { Name = currentDeviceName, Meters = meters });
                        meters = new List<Meter>();
                    }

                    if (devices.Any())
                    {
                        lines.Add(new Line() { Name = currentLineName, Devices = devices });
                        devices = new List<Device>();
                    }

                    currentDeviceName = meter.DeviceName;
                    currentLineName = meter.LineName;
                }
                else if (currentDeviceName != meter.DeviceName)
                {
                    if (meters.Any())
                    {
                        devices.Add(new Device() { Name = currentDeviceName, Meters = meters });
                        meters = new List<Meter>();
                    }

                    currentDeviceName = meter.DeviceName;
                }

                meters.Add(meter);
            }

            return lines;
        }

        public bool AddExam(Exam exam)
        {
            long examId= DataAccessProvider.DBAccessor.AddExam(exam);
            if(examId ==-1)
            {
                return false;
            }

            if(DataAccessProvider.DBAccessor.AddReadings(exam.Meters, exam.ExamId))
            {
                return true;
            }
            else
            {
                DataAccessProvider.DBAccessor.DeleteExam(examId);
                return false;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            return DataAccessProvider.DBAccessor.GetUsers(); ;
        }

        public IEnumerable<Section> GetSections()
        {
            return DataAccessProvider.DBAccessor.GetSections();
        }
    }
}
