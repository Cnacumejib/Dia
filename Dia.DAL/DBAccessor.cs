using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

using Dia.Entities;
using Dia.IDAC;

namespace Dia.DAL
{
    public class DBAccessor : IDAContracts
    {
        public IEnumerable<Section> GetSections()
        {
            IList<Section> sections = new List<Section>();
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select id, name from desk.dia.sections ORDER BY name";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        sections.Add(
                            new Section() {
                                Id = (int)reader["id"],
                                Name = (string)reader["name"]
                            });
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return sections;
        }

        public IEnumerable<User> GetUsers()
        {
            IList<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select id, name from desk.dia.users ORDER BY name";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        users.Add(new User()
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        });
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return users;
        }

        public IEnumerable<Exam> GetExams(DateTime date)
        {
            IList<Exam> exams = new List<Exam>();
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select exam_id, section_name, user_name, FilledFrom, FilledTo from desk.dia.getexams (@report_date) ORDER BY exam_id desc";
                command.Parameters.AddWithValue("@report_date", date);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        exams.Add(
                            new Exam()
                            {
                                ExamId = (long)reader["exam_id"],
                                ExamSection = new Section() { Name = (string)reader["section_name"] },
                                ExamUser = new User() { Name = (string)reader["user_name"] },
                                FilledFrom = (DateTime)reader["FilledFrom"],
                                FilledTo = (DateTime)reader["FilledTo"],
                            });
                    }
                }
                catch (Exception ee)
                {
                }
            }

            return exams;
        }

        public IEnumerable<Meter> GetMeters(int sectionId)
        {
            IList<Meter> meters = new List<Meter>();
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select * from desk.dia.getMeters(@section_id) ORDER BY row_number, col_number";
                command.Parameters.AddWithValue("@section_id", sectionId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        meters.Add(new Meter()
                        {
                            MeterId = (long)reader["meter_id"],
                            ColumnNumber = (int)(long)reader["col_number"],
                            RowNumber = (int)(long) reader["row_number"],
                            LineName = (string)reader["line_name"],
                            DeviceName = (string)reader["device_name"],
                            ElementName = (string)reader["element_name"],
                            /* 
                               ReadingId = (long)reader["reading_id"],
                               LineSorter = (int)reader["line_sorter"],
                               DeviceSorter = (int)reader["device_sorter"],
                               ElementSorter = (int)reader["element_sorter"],
                            */
                        });
                    }
                    catch (Exception ee)
                    {
                    }
                }
            }

            return meters;
        }

        public IEnumerable<Meter> GetReadings(long examId)
        {

            IList<Meter> readings = new List<Meter>();
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "select * from desk.dia.getReadings(@exam_id) ORDER BY  row_number, col_number"; //line_sorter,device_sorter,element_sorter
                command.Parameters.AddWithValue("@exam_id", examId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        Meter meter = new Meter()
                        {
                            ColumnNumber = (int)(long)reader["col_number"],
                            RowNumber = (int)(long)reader["row_number"],
                            LineName = (string)reader["line_name"],
                            DeviceName = (string)reader["device_name"],
                            ElementName = (string)reader["element_name"],
                        };

                        float value_float;
                        string value_string = (string)reader["value"];
                        if (float.TryParse(value_string.Replace('.', ','), out value_float))
                        {
                            meter.Value = value_float;
                        }

                        readings.Add(meter);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

            return readings;
        }

        public long AddExam(Exam exam)
        {
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = "INSERT INTO desk.dia.exams([user_Id],[section_id],[filledFrom],[filledTo])VALUES (@user_Id,@section_id,@filledFrom,@filledTo);\n" +
                                          "SELECT SCOPE_IDENTITY() AS [exam_id];";
                    command.Parameters.AddWithValue("@user_Id", exam.ExamUser.Id);
                    command.Parameters.AddWithValue("@section_id", exam.ExamSection.Id);
                    command.Parameters.AddWithValue("@filledFrom", exam.FilledFrom);
                    command.Parameters.AddWithValue("@filledTo", exam.FilledTo);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        long exam_id = (long)(decimal)reader["exam_id"];
                        reader.Close();
                        transaction.Commit();
                        return exam_id;
                    }
                    else
                    {
                        transaction.Rollback();
                        return -1;
                    }
                }
                catch (Exception exception1)
                {
                    transaction.Rollback();
                    return -1;
                }
            }
        }

        public bool AddReadings(IEnumerable<Meter> readings, long examId)
        {
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    foreach (Meter reading in readings)
                    {
                        if (reading.Value != null)
                        {
                            SqlCommand command = connection.CreateCommand();
                            command.Transaction = transaction;
                            command.CommandText = "INSERT INTO desk.dia.readings (exam_id, meter_id, value) VALUES (@exam_id, @meter_id, @value)";
                            command.Parameters.AddWithValue("@meter_id", reading.MeterId);
                            command.Parameters.AddWithValue("@value", reading.Value);
                            command.Parameters.AddWithValue("@exam_id", examId);
                            if (command.ExecuteNonQuery() == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception exception1)
                {
                    Console.WriteLine(exception1.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public void DeleteExam(long examId)
        {
            using (SqlConnection connection = new SqlConnection(DB.connectionString))
            {
                try
                {
                    SqlCommand command = connection.CreateCommand();
                  //  command.Connection = connection;
                    command.CommandText = "delete from desk.dia.exams where id=@exam_id)";
                    command.Parameters.AddWithValue("@exam_id", examId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception exception1)
                {
                }
            }
        }
    }
}
