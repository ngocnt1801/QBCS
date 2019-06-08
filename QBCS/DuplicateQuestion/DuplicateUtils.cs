using DuplicateQuestion.Entity;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace DuplicateQuestion
{
    public class DuplicateUtils
    {
        private static readonly bool NOT_SEEN = false;

        [SqlProcedure]
        public static void CheckDuplidate(SqlInt32 importId)
        {
            //init list question
            List<QuestionModel> bank = null;
            List<QuestionModel> import = null;

            var importModel = GetImport(importId.Value);
            if (importModel != null)
            {
                bank = GetBank(importModel.CourseId);
                import = GetImportedQuestion(importModel.ImportId);

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
                            else // check with TF + Consine similarity
                            {
                                target = item.QuestionContent + item.RightOptions;
                                source = question.QuestionContent + question.RightOptions;
                                result = TFAlgorithm.CaculateSimilar(source, target);
                                if (result > 70)
                                {
                                    item.Status = (int)StatusEnum.Editable;
                                    item.DuplicatedQuestionId = question.Id;
                                    isUpdate = true;
                                }
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
                importModel.Status = (int)StatusEnum.Checked;
                UpdateImport(importModel);
                #endregion
            }
        }

        private static ImportModel GetImport(int importId)
        {
            ImportModel model = null;
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT CourseId, Status " +
                    "FROM Import " +
                    "WHERE Id = @importId",
                    connection
                    );

                command.Parameters.AddWithValue("@importId", importId);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    model = new ImportModel();
                    model.ImportId = importId;
                    model.CourseId = (int)reader["CourseId"];
                    model.Status = (int)reader["Status"];
                }

            }
            return model;
        }

        private static List<QuestionModel> GetBank(int courseId)
        {
            List<QuestionModel> bank = new List<QuestionModel>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Id, QuestionContent " +
                    "FROM Question " +
                    "WHERE CourseId = @courseId",
                    connection
                    );

                command.Parameters.AddWithValue("@courseId", courseId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    QuestionModel question = new QuestionModel();
                    question.QuestionContent = (string)reader["QuestionContent"];
                    question.Id = (int)reader["Id"];

                    bank.Add(question);
                }

            }
            return bank;
        }

        private static List<QuestionModel> GetImportedQuestion(int importId)
        {
            List<QuestionModel> import = new List<QuestionModel>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Id, QuestionContent " +
                    "FROM QuestionTemp " +
                    "WHERE ImportId = @importId AND Status = @status",
                    connection
                    );

                command.Parameters.AddWithValue("@importId", importId);
                command.Parameters.AddWithValue("@status", StatusEnum.NotCheck);
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
            return import;
        }

        private static void UpdateImport(ImportModel model)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "UPDATE Import " +
                    "SET Status=@status, Seen=@seen " +
                    "WHERE Id=@importId",
                    connection
                    );

                command.Parameters.AddWithValue("@status", model.Status);
                command.Parameters.AddWithValue("@seen", NOT_SEEN);
                command.Parameters.AddWithValue("@importId", model.ImportId);
                command.ExecuteNonQuery();
            }
        }
    }
}
