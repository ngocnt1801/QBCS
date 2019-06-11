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
            var importModel = GetImport(importId.Value);
            if (importModel != null)
            {
                var bank = GetBank(importModel.CourseId);
                var import = GetImportedQuestion(importModel.ImportId, (int)StatusEnum.NotCheck);

                CheckDuplicateAndUpdateDb(bank, import);

                #region move temp to bank
                if (importModel.Status == (int)StatusEnum.Fixing)
                {
                    InsertTempToBank(importModel);
                    RemoveTemp(importModel.ImportId);
                }
                #endregion

                #region update import status
                if (importModel.Status == (int)StatusEnum.NotCheck)
                {
                    importModel.Status = (int)StatusEnum.Checked;
                }
                else if (importModel.Status == (int)StatusEnum.Fixing)
                {
                    importModel.Status = (int)StatusEnum.Done;
                }
                UpdateImport(importModel);
                #endregion
            }
        }

        private static void CheckDuplicateAndUpdateDb(List<QuestionModel> bank, List<QuestionModel> import)
        {
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
                        var result = LevenshteinDistance.CalculateSimilarity(source, target);
                        //item.Test = result.ToString() + " - " + source + " - " + target; //remove
                        if (result >= 70)
                        {
                            item.Test = "Levenshtein";
                            //check option
                            int countDuplicate = 0;
                            int targetRightCount = 0;

                            #region get duplicate %
                            foreach (var iOption in item.Options)
                            {
                                if (iOption.IsCorrect)
                                {
                                    targetRightCount = targetRightCount + 1;
                                    string iOptionTarget = StringUtils.NormalizeString(iOption.OptionContent);
                                    foreach (var bOption in question.Options)
                                    {
                                        string bOptionSource = StringUtils.NormalizeString(bOption.OptionContent);
                                        if (LevenshteinDistance.CalculateSimilarity(bOptionSource, iOptionTarget) >= 70)
                                        {
                                            countDuplicate = countDuplicate + 1;
                                            break;
                                        }
                                    }
                                }
                            }

                            float optionCheckResult = ((float)countDuplicate) / targetRightCount;
                            #endregion

                            if (optionCheckResult > 0.5)
                            {
                                item.Status = (int)StatusEnum.Delete;
                                if (question.IsBank)
                                {
                                    item.DuplicatedQuestionId = question.Id;
                                }
                                else
                                {
                                    item.DuplicatedWithImportId = question.Id;
                                }
                                isUpdate = true;
                            }
                            else
                            {
                                item.Status = (int)StatusEnum.Editable;
                                if (question.IsBank)
                                {
                                    item.DuplicatedQuestionId = question.Id;
                                }
                                else
                                {
                                    item.DuplicatedWithImportId = question.Id;
                                }
                                isUpdate = true;
                            }

                        }
                        else // check with TF + Consine similarity
                        {
                            item.Test = "TF+Consine";
                            string targetTF = item.QuestionContent;
                            string sourceTF = question.QuestionContent;

                            #region get right option string
                            string rightOptionTaget = "";
                            string rightOptionSource = "";
                            foreach (var option in item.Options)
                            {
                                if (option.IsCorrect)
                                {
                                    rightOptionTaget += option.OptionContent;
                                }
                            }

                            foreach (var option in question.Options)
                            {
                                if (option.IsCorrect)
                                {
                                    rightOptionSource += option.OptionContent;
                                }
                            }
                            #endregion

                            result = TFAlgorithm.CaculateSimilar(sourceTF, targetTF);
                            if (result >= 70)
                            {
                                double optionResult = TFAlgorithm.CaculateSimilar(rightOptionSource, rightOptionTaget);
                                if (optionResult >= 70)
                                {
                                    item.Status = (int)StatusEnum.Delete;
                                    if (question.IsBank)
                                    {
                                        item.DuplicatedQuestionId = question.Id;
                                    }
                                    else
                                    {
                                        item.DuplicatedWithImportId = question.Id;
                                    }
                                    isUpdate = true;
                                }
                                else
                                {
                                    item.Status = (int)StatusEnum.Editable;
                                    if (question.IsBank)
                                    {
                                        item.DuplicatedQuestionId = question.Id;
                                    }
                                    else
                                    {
                                        item.DuplicatedWithImportId = question.Id;
                                    }
                                    isUpdate = true;
                                }

                            }
                            else // check quesiton + option
                            {
                                sourceTF = sourceTF + " " + rightOptionSource;
                                targetTF = targetTF + " " + rightOptionTaget;

                                double resultQwithO = TFAlgorithm.CaculateSimilar(sourceTF, targetTF);
                                if (resultQwithO >= 70)
                                {
                                    item.Status = (int)StatusEnum.Delete;
                                    if (question.IsBank)
                                    {
                                        item.DuplicatedQuestionId = question.Id;
                                    }
                                    else
                                    {
                                        item.DuplicatedWithImportId = question.Id;
                                    }
                                    isUpdate = true;
                                }
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
                       "SET Status=@status, DuplicatedId=@duplicatedId, DuplicateInImportId=@duplicatedWithImport, OptionsContent=@test " +
                       "WHERE Id=@id",
                       connection
                       );
                    command.Parameters.AddWithValue("@status", item.Status);
                    command.Parameters.AddWithValue("@duplicatedId", item.DuplicatedQuestionId);
                    command.Parameters.AddWithValue("@duplicatedWithImport", item.DuplicatedWithImportId);
                    command.Parameters.AddWithValue("@test", item.Test);
                    command.Parameters.AddWithValue("@id", item.Id);

                    command.ExecuteNonQuery();

                    //add not duplicate question to check
                    if (!isUpdate)
                    {
                        bank.Add(item);
                    }

                }

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
                    "SELECT q.Id, q.QuestionContent, o.OptionContent " +
                    "FROM Question q inner join [Option] o on q.Id = o.QuestionId " +
                    "WHERE q.CourseId = @courseId AND o.IsCorrect = 1",
                    connection
                    );

                command.Parameters.AddWithValue("@courseId", courseId);
                SqlDataReader reader = command.ExecuteReader();

                int prev = 0;
                QuestionModel question = null;
                while (reader.Read())
                {
                    if (prev != (int)reader["Id"])
                    {
                        question = new QuestionModel();
                        question.QuestionContent = (string)reader["QuestionContent"];
                        question.Id = (int)reader["Id"];
                        question.IsBank = true;
                        question.Options = new List<OptionModel>();
                        question.Options.Add(new OptionModel
                        {
                            OptionContent = (string)reader["OptionContent"]
                        });
                        bank.Add(question);

                        prev = question.Id;
                    }
                    else
                    {
                        question.Options.Add(new OptionModel
                        {
                            OptionContent = (string)reader["OptionContent"]
                        });
                    }

                }

            }
            return bank;
        }

        private static List<QuestionModel> GetImportedQuestion(int importId, int status)
        {
            List<QuestionModel> import = new List<QuestionModel>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "SELECT q.Id, q.Code, q.QuestionContent, o.OptionContent, o.IsCorrect, q.Status " +
                    "FROM QuestionTemp q inner join OptionTemp o on q.Id = o.TempId " +
                    "WHERE q.ImportId = @importId AND q.Status= @status",
                    connection
                    );

                command.Parameters.AddWithValue("@importId", importId);
                command.Parameters.AddWithValue("@status", status);
                SqlDataReader reader = command.ExecuteReader();

                int prev = 0;
                QuestionModel question = null;
                while (reader.Read())
                {
                    if (prev != (int)reader["Id"])
                    {
                        question = new QuestionModel();
                        question.QuestionContent = (string)reader["QuestionContent"];
                        question.QuestionCode = (string)reader["Code"];
                        question.Status = (int)StatusEnum.Success;
                        question.Id = (int)reader["Id"];
                        question.IsBank = false;
                        question.Options = new List<OptionModel>();
                        question.Options.Add(new OptionModel
                        {
                            OptionContent = (string)reader["OptionContent"],
                            IsCorrect = (bool)reader["IsCorrect"]
                        });
                        import.Add(question);

                        prev = question.Id;
                    }
                    else
                    {
                        question.Options.Add(new OptionModel
                        {
                            OptionContent = (string)reader["OptionContent"],
                            IsCorrect = (bool)reader["IsCorrect"]
                        });
                    }

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

        private static void RemoveTemp(int importId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "DELETE QuestionTemp " +
                    "WHERE ImportId=@importId",
                    connection
                    );

                command.Parameters.AddWithValue("@importId", importId);
                command.ExecuteNonQuery();
            }
        }

        private static void InsertTempToBank(ImportModel import)
        {
            var importSuccessList = GetImportedQuestion(import.ImportId, (int)StatusEnum.Success);
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                foreach (var question in importSuccessList)
                {
                    #region insert question
                    SqlCommand command = new SqlCommand(
                        "INSERT Question (QuestionContent,CourseId,QuestionCode) " +
                        "OUTPUT INSERTED.Id AS 'Id' " +
                        "VALUES ( " +
                            "@questionContent, " +
                            "@courseId, " +
                            "@questionCode " +
                        ")",
                        connection
                        );

                    command.Parameters.AddWithValue("@questionContent", question.QuestionContent);
                    command.Parameters.AddWithValue("@courseId", import.CourseId);
                    command.Parameters.AddWithValue("@questionCode", question.QuestionCode);
                    var reader = command.ExecuteReader();
                    #endregion
                    int id = 0;
                    if (reader.Read())
                    {
                        id = (int)reader["Id"];
                    }
                    reader.Close();
                    foreach (var option in question.Options)
                    {
                        option.QuestionId = id;
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                foreach (var question in importSuccessList)
                {
                    foreach (var option in question.Options)
                    {
                        SqlCommand optionCommand = new SqlCommand(
                                                    "INSERT [Option](QuestionId, OptionContent, IsCorrect)" +
                                                    "VALUES ( " +
                                                        "@questionId, " +
                                                        "@content, " +
                                                        "@isCorrect " +
                                                    ")", connection);

                        optionCommand.Parameters.AddWithValue("@questionId", option.QuestionId);
                        optionCommand.Parameters.AddWithValue("@content", option.OptionContent);
                        optionCommand.Parameters.AddWithValue("@isCorrect", option.IsCorrect);
                        optionCommand.ExecuteNonQuery();
                    }

                }
            }
        }
    }
}
