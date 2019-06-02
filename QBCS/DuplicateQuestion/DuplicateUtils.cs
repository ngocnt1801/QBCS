using DuplicateQuestion.Entity;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateQuestion
{
    public class DuplicateUtils
    {
        [SqlProcedure]
        public static void CheckDuplidate(SqlInt32 importId, SqlInt32 courseId)
        {
            //init list question
            List<QuestionModel> bank = new List<QuestionModel>();
            List<QuestionModel> import = new List<QuestionModel>();

            #region get imported question
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Id, QuestionContent " +
                    "FROM QuestionTemp " +
                    "WHERE ImportId = @importId",
                    connection
                    );

                command.Parameters.AddWithValue("@importId", importId.Value);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    QuestionModel question = new QuestionModel();
                    question.QuestionContent = (string)reader["QuestionContent"];
                    question.Id = (int)reader["Id"];
                    question.Status = (int)StatusEnum.Success;
                    import.Add(question);
                }

            }
            #endregion

            #region get bank
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Id, QuestionContent " +
                    "FROM Question ",
                    connection
                    );

                //command.Parameters.AddWithValue("@courseId", courseId.Value);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    QuestionModel question = new QuestionModel();
                    question.QuestionContent = (string)reader["QuestionContent"];
                    question.Id = (int)reader["Id"];

                    bank.Add(question);
                }

            }
            #endregion

            #region check duplicate and update database
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                bool isUpdate = false;
                foreach (var item in import)
                {
                    string target = StringUtils.NormalizeString(item.QuestionContent);
                    isUpdate = false;
                    foreach (var question in bank)
                    {
                        string source = StringUtils.NormalizeString(question.QuestionContent);
                        //Check question content
                        var result = LevenshteinDistance.CalculateSimilarity(source, target) * 100;
                        if (result <= 100 && result >= 90)
                        {
                            //check option content

                            item.Status = (int)StatusEnum.Delete;
                            item.DuplicatedQuestionId = question.Id;
                            isUpdate = true;

                        }
                        else if (result >= 70)
                        {
                            //check option content

                            item.Status = (int)StatusEnum.Editable;
                            item.DuplicatedQuestionId = question.Id;
                            isUpdate = true;
                        }

                        if (isUpdate)
                        {
                            break;
                        }

                    }

                    //update database
                    SqlCommand command = new SqlCommand(
                       "UPDATE QuestionTemp " +
                       "SET Status=@status, DuplicatedId=@duplicatedId " +
                       "WHERE Id=@id",
                       connection
                       );
                    command.Parameters.AddWithValue("@status", item.Status);
                    command.Parameters.AddWithValue("@duplicatedId", item.DuplicatedQuestionId);
                    command.Parameters.AddWithValue("@id", item.Id);

                    command.ExecuteNonQuery();

                    //add not duplicate question to check
                    if (!isUpdate)
                    {
                        bank.Add(item);
                    }

                }

            }
            #endregion

            #region update status import
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "UPDATE Import " +
                    "SET Status=@status " +
                    "WHERE Id=@importId",
                    connection
                    );

                command.Parameters.AddWithValue("@status", (int)StatusEnum.Done);
                command.Parameters.AddWithValue("@importId", importId.Value);
                command.ExecuteNonQuery();
            }
            #endregion
        }
    }
}
