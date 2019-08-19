using QBCS.Entity;
using QBCS.Repository.Interface;
using QBCS.Repository.ViewModel;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace QBCS.Repository.Implement
{
    public class ExaminationRepository : Repository<Examination>, IExaminationRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QBCSContext"].ConnectionString;

        public ExaminationRepository(DbContext context) : base(context)
        {

        }

        public ExaminationStatisticViewModel GetStatistic(int courseId)
        {
            var chart = new List<ExaminationChartViewModel>();
            var table = new List<ExamStatTableViewModel>();
            var stat = new ExaminationStatisticViewModel();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string query = " SELECT " +
                                      " b.GroupExam, " +
                                      " b.QuestionCode, " +
                                      " COUNT(b.QuestionReference), " +
                                      " b.QuestionReference " +
                                    " FROM(SELECT  " +
                                      " * " +
                                    " FROM(SELECT " +
                                      " poe.ExaminationId, " +
                                      " qie.QuestionCode, " +
                                      " qie.QuestionReference " +
                                    " FROM QuestionInExam qie " +
                                    " INNER JOIN PartOfExamination poe " +
                                      " ON poe.Id = qie.PartId) a " +
                                    " INNER JOIN Examination e " +
                                      " ON a.ExaminationId = e.id) b " +
                                    " WHERE b.GroupExam IN(SELECT " +
                                      " e.GroupExam " +
                                    " FROM Examination e " +
                                    " WHERE e.CourseId = @courseId " +
                                    " GROUP BY e.GroupExam) " +
                                    " GROUP BY b.GroupExam, " +
                                             " b.QuestionCode, " +
                                             " b.QuestionReference " +
                                    " HAVING COUNT(b.QuestionReference) >= 2 ";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@courseId", courseId);
                        command.Notification = null;
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlDataReader reader = command.ExecuteReader();
                        var statTable = new ExamStatTableViewModel();
                        while (reader.Read())
                        {
                            statTable = new ExamStatTableViewModel()
                            {
                                QuestionId = (int)reader[3],
                                GroupExam = (string)reader[0],
                                QuestionCode = (string)reader[1],
                                TotalNumber = (int)reader[2]

                            };
                            table.Add(statTable);
                        }
                    }


                    query = " select top 10 * from (select sum(e.NumberOfQuestion) as \"TotalQuestions\", e.GroupExam, h.TotalDuplicated, MAX(e.GeneratedDate) as \"GeneratedDate\" from Examination e inner join ( " +
                            " select ISNULL(g.total_duplicated, 0) as \"TotalDuplicated\", e.GroupExam, g.CourseId from Examination e left join(select count(f.GroupExam) as \"total_duplicated\", f.CourseId, f.GroupExam from( " +
                            " SELECT " +
                             " b.GroupExam, " +
                             " b.QuestionCode, " +
                             " COUNT(b.QuestionReference) AS \"total_occurence\", " +
                              " b.QuestionReference, " +
                              " b.CourseId " +
                            " FROM(SELECT " +
                              " * " +
                            " FROM(SELECT " +
                              " poe.ExaminationId, " +
                              " qie.QuestionCode, " +
                              " qie.QuestionReference " +
                            " FROM QuestionInExam qie " +
                            " INNER JOIN PartOfExamination poe " +
                              " ON poe.Id = qie.PartId) a " +
                            " INNER JOIN Examination e " +
                              " ON a.ExaminationId = e.id) b " +
                            " WHERE b.GroupExam IN(select e.GroupExam from Examination e where e.CourseId = @courseId1 group by e.GroupExam) " +
                            " GROUP BY b.GroupExam, " +
                                     " b.QuestionCode, " +
                                     " b.QuestionReference, " +
                                     " b.CourseId " +
                            " HAVING COUNT(b.QuestionReference) >= 2) f group by f.GroupExam, f.CourseId) g on g.GroupExam = e.GroupExam where e.CourseId = @courseId2 group by e.GroupExam, g.total_duplicated, g.CourseId " +
                            " ) h on h.GroupExam = e.GroupExam where e.CourseId = @courseId3 group by e.GroupExam, h.TotalDuplicated) i order by i.GeneratedDate desc; ";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@courseId1", courseId);
                        command.Parameters.AddWithValue("@courseId2", courseId);
                        command.Parameters.AddWithValue("@courseId3", courseId);
                        command.Notification = null;
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlDataReader reader = command.ExecuteReader();
                        var statChart = new ExaminationChartViewModel();
                        while (reader.Read())
                        {
                            statChart = new ExaminationChartViewModel()
                            {
                                TotalQuestions = (int)reader[0],
                                GroupExam = (string)reader[1],
                                TotalDuplicate = (int)reader[2]

                            };
                            chart.Add(statChart);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }

            }
            stat.Question = table;
            stat.Chart = chart;
            return stat;
        }
    }
}
