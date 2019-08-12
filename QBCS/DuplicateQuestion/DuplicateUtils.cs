﻿using DuplicateQuestion.Entity;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace DuplicateQuestion
{
    public class DuplicateUtils
    {
        private static readonly bool NOT_SEEN = false;
        private static readonly double MINIMUM_DUPLICATE = 70; //  70%
        private static readonly double HIGH_DUPLICATE = 90; //  90%
        private static readonly double OPTION_DUPLICATE = 0.5;
        private static readonly double OPTION_MAX_DUPLICATE = 0.7;
        private static readonly double IMAGE_DUPLICATE = 0.5;
        private static readonly bool CORRECT = true;
        private static readonly bool INCORRECT = false;

        #region check duplicate question udpate
        [SqlProcedure]
        public static void CheckDuplicateAQuestion(SqlInt32 questionId, SqlInt32 logId)
        {
            var item = GetQuestion(questionId.Value);
            var bank = GetBank(item.CourseId, item.UpdateQuestionId.Value);
            GetImages(bank, true);
            bool isUpdate = false;

            if (item != null)
            {

                List<string> duplicatedList = new List<string>();
                CheckDuplicateAQuestionWithBank(item, bank, ref isUpdate, duplicatedList, false);
                item.Test = String.Join(",", duplicatedList.ToArray());

                if (isUpdate)
                {
                    using (SqlConnection connection = new SqlConnection("context connection=true"))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(
                          "UPDATE QuestionTemp " +
                          "SET Status=@status, DuplicatedString=@test " +
                          "WHERE Id=@id",
                          connection
                          );
                        command.Parameters.AddWithValue("@status", item.Status);
                        command.Parameters.AddWithValue("@test", item.Test);
                        command.Parameters.AddWithValue("@id", item.Id);

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    UpdateQuestion(item);
                    EnableLogUpdateQuestion(logId.Value);
                    RemoveQuestionTemp(item.Id);
                }

            }

        }

        private static void RemoveQuestionTemp(int id)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "DELETE QuestionTemp " +
                    "WHERE Id=@id",
                    connection
                    );

                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private static void EnableLogUpdateQuestion(int logId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                  "UPDATE Log " +
                  "SET IsDisable=@disable " +
                  "WHERE Id=@id",
                  connection
                  );
                command.Parameters.AddWithValue("@disable", false);
                command.Parameters.AddWithValue("@id", logId);

                command.ExecuteNonQuery();
            }
        }

        private static void UpdateQuestion(QuestionModel question)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                          "UPDATE Question " +
                          "SET QuestionContent=@content, LearningOutcomeId=@loId, LevelId=@levelId, CategoryId=@categoryId, Image=@image " +
                          "WHERE Id=@id",
                          connection
                          );
                command.Parameters.AddWithValue("@content", question.QuestionContent);
                command.Parameters.AddWithValue("@loId", question.LearningOutcomeId);
                command.Parameters.AddWithValue("@levelId", question.LevelId);
                command.Parameters.AddWithValue("@id", question.UpdateQuestionId);
                command.Parameters.AddWithValue("@categoryId", question.CategoryId);
                command.Parameters.AddWithValue("@image", question.Image);
                command.ExecuteNonQuery();
            }

            DeleteOptions(question.UpdateQuestionId.Value);
            AddOptions(question.Options, question.UpdateQuestionId.Value);

            DeleteImages(question.Id, question.UpdateQuestionId.Value);
            AddImages(question.Images, question.UpdateQuestionId.Value);
        }

        private static void DeleteOptions(int questionId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "DELETE [Option] " +
                    "WHERE QuestionId=@questionId",
                    connection
                    );

                command.Parameters.AddWithValue("@questionId", questionId);
                command.ExecuteNonQuery();
            }
        }

        private static void AddOptions(List<OptionModel> options, int questionId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                foreach (var option in options)
                {
                    SqlCommand optionCmd = new SqlCommand(
                          "INSERT INTO [dbo].[Option] " +
                          "([QuestionId] " +
                          ",[OptionContent] " +
                          ",[IsCorrect] " +
                          ",[Image])" +
                          "VALUES (@questionId, @content, @correct, @image)",
                          connection
                          );
                    optionCmd.Parameters.AddWithValue("@content", option.OptionContent);
                    optionCmd.Parameters.AddWithValue("@correct", option.IsCorrect);
                    optionCmd.Parameters.AddWithValue("@questionId", questionId);
                    optionCmd.Parameters.AddWithValue("@image", option.Image);
                    optionCmd.ExecuteNonQuery();
                }

            }
        }

        private static void DeleteImages(int tempId, int questionId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "DELETE Image " +
                    "WHERE QuestionTempId=@tempId OR QuestionId=@questionId",
                    connection
                    );

                command.Parameters.AddWithValue("@tempId", tempId);
                command.Parameters.AddWithValue("@questionId", questionId);
                command.ExecuteNonQuery();
            }
        }

        private static void AddImages(List<ImageModel> images, int questionId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                foreach (var img in images)
                {
                    SqlCommand optionCmd = new SqlCommand(
                          "INSERT INTO [dbo].[Image] " +
                          "([Source] " +
                          ",[QuestionId]) " +
                          "VALUES (@source, @questionId)",
                          connection
                          );
                    optionCmd.Parameters.AddWithValue("@source", img.Source);
                    optionCmd.Parameters.AddWithValue("@questionId", questionId);
                    optionCmd.ExecuteNonQuery();
                }

            }
        }

        private static List<QuestionModel> GetBank(int courseId, int questionId)
        {
            List<QuestionModel> bank = new List<QuestionModel>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT q.Id, q.QuestionContent, o.IsCorrect, o.OptionContent " +
                    "FROM Question q inner join [Option] o on q.Id = o.QuestionId " +
                    "WHERE q.CourseId = @courseId AND q.Id != @questionId",
                    connection
                    );

                command.Parameters.AddWithValue("@courseId", courseId);
                command.Parameters.AddWithValue("@questionId", questionId);
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
                            OptionContent = (string)reader["OptionContent"],
                            IsCorrect = (bool)reader["IsCorrect"]
                        });
                        bank.Add(question);

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
            return bank;
        }

        private static QuestionModel GetQuestion(int id)
        {
            QuestionModel question = null;
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT " +
                        "q.Id, " +
                        "q.QuestionContent, " +
                        "o.OptionContent, " +
                        "o.IsCorrect, " +
                        "o.UpdateOptionId, " +
                        "o.Image, " +
                        "q.Category, " +
                        "q.LearningOutcome, " +
                        "q.LevelName, " +
                        "q.Image AS 'QuestionImage', " +
                        "q.UpdateQuestionId " +
                    "FROM QuestionTemp q inner join OptionTemp o on q.Id = o.TempId " +
                    "WHERE q.Id = @tempId",
                    connection
                    );

                command.Parameters.AddWithValue("@tempId", id);
                SqlDataReader reader = command.ExecuteReader();

                int prev = 0;

                while (reader.Read())
                {
                    if (prev != (int)reader["Id"])
                    {
                        question = new QuestionModel();
                        if (reader["QuestionContent"] != null)
                        {
                            question.QuestionContent = (string)reader["QuestionContent"];
                        }
                        //if (reader["Code"] != null)
                        //{
                        //    question.QuestionCode = (string)reader["Code"];
                        //}
                        question.Status = (int)StatusEnum.Success;

                        if (reader["UpdateQuestionId"] != null)
                        {
                            question.UpdateQuestionId = (int)reader["UpdateQuestionId"];
                        }

                        if (reader["Id"] != null)
                        {
                            question.Id = (int)reader["Id"];
                        }
                        if (reader["QuestionImage"] != DBNull.Value)
                        {
                            question.Image = (string)reader["QuestionImage"];
                        }
                        if (reader["Category"] != DBNull.Value)
                        {
                            int tmp = 0;
                            Int32.TryParse((string)reader["Category"], out tmp);
                            if (tmp != 0)
                            {
                                question.CategoryId = tmp;
                            }
                        }
                        if (reader["LearningOutcome"] != DBNull.Value)
                        {
                            int tmp = 0;
                            Int32.TryParse((string)reader["LearningOutcome"], out tmp);
                            if (tmp != 0)
                            {
                                question.LearningOutcomeId = tmp;
                            }

                        }
                        if (reader["LevelName"] != DBNull.Value)
                        {
                            int tmp = 0;
                            Int32.TryParse((string)reader["LevelName"], out tmp);
                            if (tmp != 0)
                            {
                                question.LevelId = tmp;
                            }

                        }
                        question.IsBank = false;
                        question.Options = new List<OptionModel>();
                        question.Options.Add(new OptionModel
                        {
                            Id = reader["UpdateOptionId"] != null ? (int)reader["UpdateOptionId"] : 0,
                            OptionContent = reader["OptionContent"] != null ? (string)reader["OptionContent"] : "",
                            IsCorrect = reader["IsCorrect"] != null ? (bool)reader["IsCorrect"] : false
                        });

                        prev = question.Id;
                    }
                    else
                    {
                        question.Options.Add(new OptionModel
                        {
                            Id = reader["UpdateOptionId"] != null ? (int)reader["UpdateOptionId"] : 0,
                            OptionContent = reader["OptionContent"] != null ? (string)reader["OptionContent"] : "",
                            IsCorrect = reader["IsCorrect"] != null ? (bool)reader["IsCorrect"] : false
                        });
                    }

                }
                reader.Close();
                SqlCommand quesCommand = new SqlCommand(
                    "SELECT " +
                        "CourseId " +
                    "FROM Question " +
                    "WHERE Id = @id",
                    connection
                    );

                quesCommand.Parameters.AddWithValue("@id", question.UpdateQuestionId);
                SqlDataReader quesReader = quesCommand.ExecuteReader();
                if (quesReader.Read())
                {
                    if (quesReader["CourseId"] != null)
                    {
                        question.CourseId = (int)quesReader["CourseId"];
                    }
                }


            }

            question.Images = GetImages(question.Id);

            return question;

        }

        private static List<ImageModel> GetImages(int tempId)
        {
            List<ImageModel> images = new List<ImageModel>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT " +
                        "Source " +
                    "FROM Image " +
                    "WHERE QuestionTempId=@tempId",
                    connection
                    );

                command.Parameters.AddWithValue("@tempId", tempId);
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    if (reader["Source"] != DBNull.Value)
                    {
                        images.Add(new ImageModel
                        {
                            Source = (string)reader["Source"]
                        });
                    }

                }
                reader.Close();

            }

            return images;
        }

        private static void CheckDuplicateAQuestionWithBank(QuestionModel item, List<QuestionModel> bank, ref bool isUpdate, List<string> duplicatedList, bool checkSemantic = true)
        {
            bool firstIsContent = true;

            //define main compare
            if (item.QuestionContent.Length < ConvertListOptionsToString(GetOptionsByStatus(item.Options, CORRECT)).Length)
            {
                firstIsContent = false;
            }
            foreach (var question in bank)
            {
                question.Result = "";
                if ((!question.IsBank && !item.IsBank && item.Id == question.Id)
                    || (question.IsBank && item.IsBank && question.Id == item.Id)
                    || (item.UpdateQuestionId.HasValue && item.UpdateQuestionId.Value == question.Id))
                {
                    continue;
                }

                if (String.IsNullOrWhiteSpace(item.QuestionContent)
                    || String.IsNullOrWhiteSpace(StringUtils.RemoveSignals(item.QuestionContent))
                    || String.IsNullOrWhiteSpace(question.QuestionContent)
                    || String.IsNullOrWhiteSpace(StringUtils.RemoveSignals(question.QuestionContent))
                    )
                {
                    if (item.Images != null && item.Images.Count > 0)
                    {
                        if (question.Images != null && question.Images.Count > 0)
                        {
                            if (CheckImageQuestion(item.Images, question.Images))
                            {
                                question.Result += "i_true" + "_";

                                var allOptionResult = CalculateListOptionSimilar(question.Options, item.Options, checkSemantic);
                                question.Result += "ao_" + allOptionResult + "_";
                                if (allOptionResult > OPTION_MAX_DUPLICATE)
                                {
                                    AssignDuplicated(question, item, StatusEnum.Editable);
                                    isUpdate = true;

                                    if (duplicatedList != null)
                                    {
                                        duplicatedList.Add(question.ToString());
                                    }
                                }
                                else
                                {
                                    double optionRightResult = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, CORRECT),
                                                                               GetOptionsByStatus(item.Options, CORRECT), checkSemantic);
                                    question.Result += "or_" + optionRightResult + "_";

                                    if (optionRightResult > OPTION_DUPLICATE) //same correct option
                                    {
                                        AssignDuplicated(question, item, StatusEnum.Editable);
                                        isUpdate = true;

                                        if (duplicatedList != null)
                                        {
                                            duplicatedList.Add(question.ToString());
                                        }
                                    }


                                }
                            }
                        }
                    }
                }
                else
                {
                    if (firstIsContent)
                    {
                        #region main is question content

                        //Check question content
                        var questionResult = CaculateStringSimilar(question.QuestionContent, item.QuestionContent, checkSemantic);
                        question.Result += "q_" + questionResult + "_";
                        if (questionResult >= HIGH_DUPLICATE) //same question content
                        {
                            var allOptionResult = CalculateListOptionSimilar(question.Options, item.Options, checkSemantic);
                            question.Result += "ao_" + allOptionResult + "_";
                            if (allOptionResult > OPTION_MAX_DUPLICATE)
                            {
                                #region check image
                                if (item.Images != null && item.Images.Count > 0)
                                {
                                    if (question.Images != null && question.Images.Count > 0)
                                    {
                                        if (CheckImageQuestion(item.Images, question.Images))
                                        {
                                            question.Result += "i_true" + "_";

                                            AssignDuplicated(question, item, StatusEnum.Editable);
                                            isUpdate = true;

                                            if (duplicatedList != null)
                                            {
                                                duplicatedList.Add(question.ToString());
                                            }
                                        }
                                    }
                                }

                                if ((item.Images == null || item.Images.Count == 0)
                                    && (question.Images == null || question.Images.Count == 0))
                                {
                                    question.Result += "ni_true" + "_";

                                    AssignDuplicated(question, item, StatusEnum.Editable);
                                    isUpdate = true;

                                    if (duplicatedList != null)
                                    {
                                        duplicatedList.Add(question.ToString());
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region check correct option
                                double optionRightResult = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, CORRECT),
                                                                                    GetOptionsByStatus(item.Options, CORRECT), checkSemantic);
                                question.Result += "or_" + optionRightResult + "_";

                                if (optionRightResult > OPTION_DUPLICATE) //same correct option
                                {
                                    #region check image
                                    if (item.Images != null && item.Images.Count > 0)
                                    {
                                        if (question.Images != null && question.Images.Count > 0)
                                        {
                                            if (CheckImageQuestion(item.Images, question.Images))
                                            {
                                                question.Result += "i_true" + "_";

                                                AssignDuplicated(question, item, StatusEnum.Editable);
                                                isUpdate = true;

                                                if (duplicatedList != null)
                                                {
                                                    duplicatedList.Add(question.ToString());
                                                }
                                            }
                                        }
                                    }

                                    if ((item.Images == null || item.Images.Count == 0)
                                        && (question.Images == null || question.Images.Count == 0))
                                    {
                                        question.Result += "ni_true" + "_";

                                        AssignDuplicated(question, item, StatusEnum.Editable);
                                        isUpdate = true;

                                        if (duplicatedList != null)
                                        {
                                            duplicatedList.Add(question.ToString());
                                        }
                                    }

                                    #endregion

                                }

                                #endregion
                            }

                        } // end if > HIGH_Duplicate
                        else if (questionResult >= MINIMUM_DUPLICATE)
                        {

                            #region check correct option
                            double optionRightResult = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, CORRECT),
                                                                                GetOptionsByStatus(item.Options, CORRECT), checkSemantic);
                            question.Result += "or_" + optionRightResult + "_";

                            if (optionRightResult > OPTION_DUPLICATE) //same correct option
                            {
                                #region check wrong options
                                double optionWrongResult = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, INCORRECT),
                                                                                GetOptionsByStatus(item.Options, INCORRECT), checkSemantic);
                                question.Result += "ow_" + optionWrongResult + "_";
                                if (optionWrongResult >= OPTION_DUPLICATE)
                                {
                                    #region check image
                                    if (item.Images != null && item.Images.Count > 0)
                                    {
                                        if (question.Images != null && question.Images.Count > 0)
                                        {
                                            if (CheckImageQuestion(item.Images, question.Images))
                                            {
                                                question.Result += "i_true" + "_";

                                                AssignDuplicated(question, item, StatusEnum.Editable);
                                                isUpdate = true;

                                                if (duplicatedList != null)
                                                {
                                                    duplicatedList.Add(question.ToString());
                                                }
                                            }
                                        }
                                    }

                                    if ((item.Images == null || item.Images.Count == 0)
                                        && (question.Images == null || question.Images.Count == 0))
                                    {
                                        question.Result += "ni_true" + "_";

                                        AssignDuplicated(question, item, StatusEnum.Editable);
                                        isUpdate = true;

                                        if (duplicatedList != null)
                                        {
                                            duplicatedList.Add(question.ToString());
                                        }
                                    }

                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        } // end if > MINIMUM_DUPLICATE
                        else
                        {
                            double questionAndOptionResult = CaculateStringSimilar(question.QuestionContent + " " + ConvertListOptionsToString(GetOptionsByStatus(question.Options, CORRECT)),
                                                                                    item.QuestionContent + " " + ConvertListOptionsToString(GetOptionsByStatus(item.Options, CORRECT)), checkSemantic);
                            question.Result += "qo_" + questionAndOptionResult + "_";

                            if (questionAndOptionResult > MINIMUM_DUPLICATE)
                            {
                                
                                #region check wrong options
                                
                                double checkOptionWrong = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, INCORRECT),
                                                                                GetOptionsByStatus(item.Options, INCORRECT), checkSemantic);
                                question.Result += "ow_" + checkOptionWrong + "_";

                                if (checkOptionWrong >= OPTION_DUPLICATE)
                                {
                                    #region check image
                                    if (item.Images != null && item.Images.Count > 0)
                                    {
                                        if (question.Images != null && question.Images.Count > 0)
                                        {
                                            if (CheckImageQuestion(item.Images, question.Images))
                                            {
                                                question.Result += "i_true" + "_";

                                                AssignDuplicated(question, item, StatusEnum.Editable);
                                                isUpdate = true;

                                                if (duplicatedList != null)
                                                {
                                                    duplicatedList.Add(question.ToString());
                                                }
                                            }
                                        }
                                    }

                                    if ((item.Images == null || item.Images.Count == 0)
                                        && (question.Images == null || question.Images.Count == 0))
                                    {
                                        question.Result += "ni_true" + "_";

                                        AssignDuplicated(question, item, StatusEnum.Editable);
                                        isUpdate = true;

                                        if (duplicatedList != null)
                                        {
                                            duplicatedList.Add(question.ToString());
                                        }
                                    }

                                    #endregion
                                }
                                
                                #endregion
    
                            }
                            else
                            {
                                if (item.Images != null && item.Images.Count > 0)
                                {
                                    if (question.Images != null && question.Images.Count > 0)
                                    {
                                        if (CheckImageQuestion(item.Images, question.Images))
                                        {
                                            question.Result += "i_true" + "_";

                                            var allOptionResult = CalculateListOptionSimilar(question.Options, item.Options, checkSemantic);
                                            question.Result += "ao_" + allOptionResult + "_";
                                            if (allOptionResult > OPTION_MAX_DUPLICATE)
                                            {
                                                AssignDuplicated(question, item, StatusEnum.Editable);
                                                isUpdate = true;

                                                if (duplicatedList != null)
                                                {
                                                    duplicatedList.Add(question.ToString());
                                                }
                                            }
                                            else
                                            {
                                                double optionRightResult = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, CORRECT),
                                                                                    GetOptionsByStatus(item.Options, CORRECT), checkSemantic);
                                                question.Result += "or_" + optionRightResult + "_";

                                                if (optionRightResult > OPTION_DUPLICATE) //same correct option
                                                {
                                                    AssignDuplicated(question, item, StatusEnum.Editable);
                                                    isUpdate = true;

                                                    if (duplicatedList != null)
                                                    {
                                                        duplicatedList.Add(question.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        #endregion
                    }
                    else
                    {
                        #region main is options

                        double optionRightResult = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, CORRECT),
                                                                            GetOptionsByStatus(item.Options, CORRECT), checkSemantic);
                        question.Result += "or_" + optionRightResult + "_";

                        if (optionRightResult > OPTION_DUPLICATE) //same correct option
                        {
                            var questionResult = CaculateStringSimilar(question.QuestionContent, item.QuestionContent, checkSemantic);
                            question.Result += "q_" + questionResult + "_";

                            if (questionResult >= HIGH_DUPLICATE) //same question content
                            {

                                #region check image
                                if (item.Images != null && item.Images.Count > 0)
                                {
                                    if (question.Images != null && question.Images.Count > 0)
                                    {
                                        if (CheckImageQuestion(item.Images, question.Images))
                                        {

                                            question.Result += "i_true" + "_";

                                            AssignDuplicated(question, item, StatusEnum.Editable);
                                            isUpdate = true;

                                            if (duplicatedList != null)
                                            {
                                                duplicatedList.Add(question.ToString());
                                            }
                                        }
                                    }
                                }

                                if ((item.Images == null || item.Images.Count == 0)
                                    && (question.Images == null || question.Images.Count == 0))
                                {
                                    question.Result += "ni_true" + "_";

                                    AssignDuplicated(question, item, StatusEnum.Editable);
                                    isUpdate = true;

                                    if (duplicatedList != null)
                                    {
                                        duplicatedList.Add(question.ToString());
                                    }
                                }

                                #endregion

                            } // end if > HIGH_Duplicate
                            else if (questionResult >= MINIMUM_DUPLICATE)
                            {
                                #region check wrong options
                                double optionWrongResult = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, INCORRECT),
                                                                                GetOptionsByStatus(item.Options, INCORRECT), checkSemantic);
                                question.Result += "ow_" + optionWrongResult + "_";

                                if (optionWrongResult >= OPTION_DUPLICATE)
                                {
                                    #region check image
                                    if (item.Images != null && item.Images.Count > 0)
                                    {
                                        if (question.Images != null && question.Images.Count > 0)
                                        {
                                            if (CheckImageQuestion(item.Images, question.Images))
                                            {
                                                question.Result += "i_true" + "_";

                                                AssignDuplicated(question, item, StatusEnum.Editable);
                                                isUpdate = true;

                                                if (duplicatedList != null)
                                                {
                                                    duplicatedList.Add(question.ToString());
                                                }
                                            }
                                        }
                                    }

                                    if ((item.Images == null || item.Images.Count == 0)
                                        && (question.Images == null || question.Images.Count == 0))
                                    {
                                        question.Result += "ni_true" + "_";
                                        AssignDuplicated(question, item, StatusEnum.Editable);
                                        isUpdate = true;

                                        if (duplicatedList != null)
                                        {
                                            duplicatedList.Add(question.ToString());
                                        }
                                    }

                                    #endregion
                                }
                                #endregion

                            } // end if > MINIMUM_DUPLICATE
                            else
                            {

                                if (item.Images != null && item.Images.Count > 0)
                                {
                                    if (question.Images != null && question.Images.Count > 0)
                                    {
                                        if (CheckImageQuestion(item.Images, question.Images))
                                        {
                                            question.Result += "i_true" + "_";

                                            AssignDuplicated(question, item, StatusEnum.Editable);
                                            isUpdate = true;

                                            if (duplicatedList != null)
                                            {
                                                duplicatedList.Add(question.ToString());
                                            }
                                        }
                                    }
                                }

                                /*
                                double questionAndOptionResult = CaculateStringSimilar(question.QuestionContent + " " + ConvertListOptionsToString(GetOptionsByStatus(question.Options, CORRECT)),
                                                                                    item.QuestionContent + " " + ConvertListOptionsToString(GetOptionsByStatus(item.Options, CORRECT)), checkSemantic);
                                question.Result += "qo_" + questionAndOptionResult + "_";
                                if (questionAndOptionResult > MINIMUM_DUPLICATE)
                                {
                                    #region check wrong options
                                    double checkOptionWrong = CalculateListOptionSimilar(GetOptionsByStatus(question.Options, INCORRECT),
                                                                                    GetOptionsByStatus(item.Options, INCORRECT), checkSemantic);
                                    question.Result += "ow_" + checkOptionWrong + "_";

                                    if (checkOptionWrong >= OPTION_DUPLICATE)
                                    {
                                        #region check image
                                        if (item.Images != null && item.Images.Count > 0)
                                        {
                                            if (question.Images != null && question.Images.Count > 0)
                                            {
                                                if (CheckImageQuestion(item.Images, question.Images))
                                                {
                                                    question.Result += "i_true" + "_";

                                                    AssignDuplicated(question, item, StatusEnum.Editable);
                                                    isUpdate = true;

                                                    if (duplicatedList != null)
                                                    {
                                                        duplicatedList.Add(question.ToString());
                                                    }
                                                }
                                            }
                                        }

                                        if ((item.Images == null || item.Images.Count == 0)
                                            && (question.Images == null || question.Images.Count == 0))
                                        {
                                            question.Result += "ni_true" + "_";

                                            AssignDuplicated(question, item, StatusEnum.Editable);
                                            isUpdate = true;

                                            if (duplicatedList != null)
                                            {
                                                duplicatedList.Add(question.ToString());
                                            }
                                        }

                                        #endregion
                                    }
                                    #endregion
                                }
                                */
                            }
                        }
                        else
                        {
                            var questionResult = CaculateStringSimilar(question.QuestionContent, item.QuestionContent, checkSemantic);
                            question.Result += "q_" + questionResult + "_";
                            if (questionResult >= HIGH_DUPLICATE)
                            {
                                var allOptionResult = CalculateListOptionSimilar(question.Options, item.Options, checkSemantic);
                                question.Result += "ao_" + allOptionResult + "_";

                                if (allOptionResult > OPTION_MAX_DUPLICATE)
                                {
                                    #region check image
                                    if (item.Images != null && item.Images.Count > 0)
                                    {
                                        if (question.Images != null && question.Images.Count > 0)
                                        {
                                            if (CheckImageQuestion(item.Images, question.Images))
                                            {
                                                question.Result += "i_true" + "_";

                                                AssignDuplicated(question, item, StatusEnum.Editable);
                                                isUpdate = true;

                                                if (duplicatedList != null)
                                                {
                                                    duplicatedList.Add(question.ToString());
                                                }
                                            }
                                        }
                                    }

                                    if ((item.Images == null || item.Images.Count == 0)
                                        && (question.Images == null || question.Images.Count == 0))
                                    {
                                        question.Result += "ni_true" + "_";

                                        AssignDuplicated(question, item, StatusEnum.Editable);
                                        isUpdate = true;

                                        if (duplicatedList != null)
                                        {
                                            duplicatedList.Add(question.ToString());
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                }


                //if (isUpdate)
                //{
                //    break;
                //}
            }
        }

        private static bool CheckImageQuestion(List<ImageModel> temp, List<ImageModel> question)
        {

            List<ImageModel> firstList = null;
            List<ImageModel> secondList = null;
            if (temp.Count < question.Count)
            {
                firstList = temp;
                secondList = question;
            }
            else
            {
                firstList = question;
                secondList = temp;
            }

            int countDuplicate = 0;
            foreach (var tmpImage in firstList)
            {
                foreach (var image in secondList)
                {
                    if (CheckDuplicatedImage.CheckDuplicateImage(tmpImage.Source, image.Source))
                    {
                        countDuplicate = countDuplicate + 1;
                        break;
                    }
                }
            }
            //return ((float)countDuplicate / maxCount) >= 0.7;
            return ((float)countDuplicate / firstList.Count) >= IMAGE_DUPLICATE;
        }

        #endregion

        #region check duplicate import
        [SqlProcedure]
        public static void CheckDuplidate(SqlInt32 importId, SqlBoolean checkSemantic)
        {
            var importModel = GetImport(importId.Value);
            if (importModel != null)
            {
                var bank = GetBank(importModel.CourseId);
                var import = GetImportedQuestion(importModel.ImportId, (int)StatusEnum.NotCheck);
                var otherImpor = GetOtherImportQuestion(importId.Value, importModel.CourseId);

                GetImages(bank, true);
                GetImages(import, false);
                GetImages(otherImpor, false);

                //GetOptionImageTemp(import);

                bank.AddRange(import);
                bank.AddRange(otherImpor);



                CheckDuplicateAndUpdateDb(bank, import, checkSemantic.IsTrue);

                #region move temp to bank
                if (importModel.Status == (int)StatusEnum.Editing)
                {
                    importModel.TotalSuccess = InsertTempToBank(importModel);
                    RemoveTemp(importModel.ImportId);
                }
                #endregion

                #region update import status
                if (importModel.Status == (int)StatusEnum.NotCheck)
                {
                    importModel.Status = (int)StatusEnum.Checked;
                }
                else if (importModel.Status == (int)StatusEnum.Editing)
                {
                    importModel.Status = (int)StatusEnum.Done;
                }
                UpdateImport(importModel);
                #endregion
            }
        }

        private static void CheckDuplicateAndUpdateDb(List<QuestionModel> bank, List<QuestionModel> import, bool checkSemantic = true)
        {

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                bool isUpdate = false;
                //bool firstIsContent = true;
                foreach (var item in import)
                {

                    isUpdate = false;

                    List<string> duplicatedList = new List<string>();
                    CheckDuplicateAQuestionWithBank(item, bank, ref isUpdate, duplicatedList, checkSemantic);
                    item.Test = String.Join(",", duplicatedList.ToArray());

                    if (item.IsNotImage)
                    {
                        item.Status = (int)StatusEnum.Editable;
                    }

                    //update database
                    SqlCommand command = new SqlCommand(
                       "UPDATE QuestionTemp " +
                       "SET Status=@status, DuplicatedId=@duplicatedId, DuplicateInImportId=@duplicatedWithImport, DuplicatedString=@test " +
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
                    //if (!isUpdate)
                    //{
                    //    bank.Add(item);
                    //}

                }

            }
        }

        private static double CaculateStringSimilar(string source, string target, bool checkSemantic = true)
        {

            double result = LevenshteinDistance.CalculateSimilarity(source, target);
            if (result < 70)
            {
                result = TFAlgorithm.CaculateSimilar(source, target);
                if (result < 70 && checkSemantic)
                {
                    result = NLPAlgorithm.CaculateSimilar(source, target);
                    if (result > 95)
                    {
                        result = HIGH_DUPLICATE;
                    }
                    else if (result > 80)
                    {
                        result = MINIMUM_DUPLICATE;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }

            return result;
        }

        private static void AssignDuplicated(QuestionModel source, QuestionModel target, StatusEnum status)
        {
            target.Status = (int)status;
            if (source.IsBank)
            {
                target.DuplicatedQuestionId = source.Id;
            }
            else
            {
                target.DuplicatedWithImportId = source.Id;
            }
        }

        private static double CalculateListOptionSimilar(List<OptionModel> source, List<OptionModel> target, bool checkSemantic = true)
        {
            int countDuplicate = 0;
            List<OptionModel> first = null;
            List<OptionModel> second = null;
            if (source.Count < target.Count)
            {
                first = source;
                second = target;
            }
            else
            {
                first = target;
                second = source;
            }

            foreach (var t in first)
            {
                foreach (var s in second)
                {

                    if ((t.Images != null && t.Images.Count > 0)
                        && (s.Images != null && s.Images.Count > 0))
                    {
                        if (CheckImageQuestion(t.Images, s.Images))
                        {
                            countDuplicate = countDuplicate + 1;
                            break;
                        }
                    }

                    if (CaculateStringSimilar(s.OptionContent, t.OptionContent, checkSemantic) >= 70)
                    {
                        countDuplicate = countDuplicate + 1;
                        break;
                    }
                }

            }

            return ((float)countDuplicate) / first.Count;
        }

        private static List<OptionModel> GetOptionsByStatus(List<OptionModel> options, bool isCorrect)
        {
            List<OptionModel> result = new List<OptionModel>();
            foreach (var o in options)
            {
                if (o.IsCorrect == isCorrect)
                {
                    result.Add(o);
                }
            }

            return result;
        }

        private static string ConvertListOptionsToString(List<OptionModel> options)
        {
            List<string> result = new List<string>();
            foreach (var item in options)
            {
                result.Add(item.OptionContent);
            }

            return String.Join(" ", result.ToArray());

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
                    "SELECT q.Id, q.QuestionContent, o.Id AS 'OptionId', o.IsCorrect, o.OptionContent " +
                    "FROM Question q inner join [Option] o on q.Id = o.QuestionId " +
                    "WHERE q.CourseId = @courseId",
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
                            Id = (int)reader["OptionId"],
                            OptionContent = (string)reader["OptionContent"],
                            IsCorrect = (bool)reader["IsCorrect"]
                        });
                        bank.Add(question);

                        prev = question.Id;
                    }
                    else
                    {
                        question.Options.Add(new OptionModel
                        {
                            Id = (int)reader["OptionId"],
                            OptionContent = (string)reader["OptionContent"],
                            IsCorrect = (bool)reader["IsCorrect"]
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
                    "SELECT q.Id, q.Code, q.QuestionContent, o.OptionContent, o.IsCorrect, o.Id AS 'OptionId', q.Status, q.Category, q.LearningOutcome, q.LevelName, q.Image, q.IsNotImage " +
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
                        question.QuestionContent = reader["QuestionContent"] != DBNull.Value ? (string)reader["QuestionContent"] : "";
                        if (reader["Image"] != DBNull.Value)
                        {
                            question.Image = (string)reader["Image"];
                        }
                        question.QuestionCode = (string)reader["Code"];
                        question.Status = (int)StatusEnum.Success;
                        question.Id = (int)reader["Id"];
                        if (reader["Category"] != DBNull.Value)
                        {
                            question.Category = (string)reader["Category"];
                        }
                        if (reader["LearningOutcome"] != DBNull.Value)
                        {
                            question.LearningOutcome = (string)reader["LearningOutcome"];
                        }
                        if (reader["LevelName"] != DBNull.Value)
                        {
                            question.Level = (string)reader["LevelName"];
                        }
                        question.IsBank = false;
                        question.Options = new List<OptionModel>();
                        question.Options.Add(new OptionModel
                        {
                            Id = (int)reader["OptionId"],
                            OptionContent = reader["OptionContent"] != DBNull.Value ? (string)reader["OptionContent"] : "",
                            IsCorrect = (bool)reader["IsCorrect"]
                        });

                        if (reader["IsNotImage"] != DBNull.Value)
                        {
                            question.IsNotImage = (bool)reader["IsNotImage"];
                        }
                        else
                        {
                            question.IsNotImage = false;
                        }

                        import.Add(question);

                        prev = question.Id;
                    }
                    else
                    {
                        question.Options.Add(new OptionModel
                        {
                            Id = (int)reader["OptionId"],
                            OptionContent = reader["OptionContent"] != DBNull.Value ? (string)reader["OptionContent"] : "",
                            IsCorrect = (bool)reader["IsCorrect"]
                        });
                    }

                }

            }

            return import;
        }

        private static void GetImages(List<QuestionModel> questions, bool isBank)
        {
            string idCol = isBank ? "QuestionId" : "QuestionTempId";
            string idOptionCol = isBank ? "OptionId" : "OptionTempId";

            foreach (var question in questions)
            {
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(
                        "SELECT Source " +
                        "FROM Image " +
                        "WHERE " + idCol + " = @id",
                        connection
                        );

                    command.Parameters.AddWithValue("@id", question.Id);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (question.Images == null)
                        {
                            question.Images = new List<ImageModel>();
                        }
                        question.Images.Add(new ImageModel
                        {
                            Source = (string)reader["Source"],
                        });
                    }
                    reader.Close();
                    foreach (var option in question.Options)
                    {
                        SqlCommand opCmd = new SqlCommand(
                       "SELECT Id, Source " +
                       "FROM Image " +
                       "WHERE " + idOptionCol + " = @id",
                       connection
                       );

                        opCmd.Parameters.AddWithValue("@id", option.Id);
                        SqlDataReader opReader = opCmd.ExecuteReader();

                        while (opReader.Read())
                        {
                            if (option.Images == null)
                            {
                                option.Images = new List<ImageModel>();
                            }
                            option.Images.Add(new ImageModel
                            {
                                Id = (int)opReader["Id"],
                                Source = (string)opReader["Source"]
                            });
                        }
                        opReader.Close();
                    }

                }
            }
        }

        private static void GetOptionImageTemp(List<QuestionModel> questions)
        {

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                foreach (var question in questions)
                {
                    foreach (var option in question.Options)
                    {
                        SqlCommand command = new SqlCommand(
                       "SELECT Id, Source " +
                       "FROM Image " +
                       "WHERE OptionTempId = @id",
                       connection
                       );

                        command.Parameters.AddWithValue("@id", option.Id);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            if (option.Images == null)
                            {
                                option.Images = new List<ImageModel>();
                            }
                            option.Images.Add(new ImageModel
                            {
                                Id = (int)reader["Id"],
                                Source = (string)reader["Source"]
                            });
                        }
                        reader.Close();
                    }
                }

            }
        }

        private static void UpdateImport(ImportModel model)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                string query = "UPDATE Import " +
                                "SET Status=@status, UpdatedDate=@date, Seen=@seen " +
                                "WHERE Id=@importId";
                if (model.Status == (int)StatusEnum.Done)
                {
                    query = "UPDATE Import " +
                            "SET Status=@status, Seen=@seen, UpdatedDate=@date, TotalSuccess=@success " +
                            "WHERE Id=@importId";
                }


                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@status", model.Status);
                command.Parameters.AddWithValue("@seen", NOT_SEEN);
                command.Parameters.AddWithValue("@importId", model.ImportId);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                if (model.Status == (int)StatusEnum.Done)
                {
                    command.Parameters.AddWithValue("@success", model.TotalSuccess);
                }
                command.ExecuteNonQuery();
            }
        }

        private static void RemoveTemp(int importId)
        {
            #region remove image

            var listDelete = GetImportedQuestion(importId, (int)StatusEnum.Delete);
            listDelete.AddRange(GetImportedQuestion(importId, (int)StatusEnum.Editable));
            listDelete.AddRange(GetImportedQuestion(importId, (int)StatusEnum.Invalid));
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                foreach (var question in listDelete)
                {
                    foreach (var option in question.Options)
                    {
                        SqlCommand optionCommand = new SqlCommand(
                                                    "DELETE Image " +
                                                    "WHERE OptionTempId=@id"
                                                    , connection);

                        optionCommand.Parameters.AddWithValue("@id", option.Id);
                        optionCommand.ExecuteNonQuery();
                    }
                }
            }

            #endregion

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

        private static int InsertTempToBank(ImportModel import)
        {
            var importSuccessList = GetImportedQuestion(import.ImportId, (int)StatusEnum.Success);
            int totalSuccess = importSuccessList.Count;
            //assign category id, learning outcome id, level id
            foreach (QuestionModel question in importSuccessList)
            {
                question.CategoryId = GetCategory(question.Category, import.CourseId);
                question.LearningOutcomeId = GetLearningOutcome(question.LearningOutcome, import.CourseId);
                question.LevelId = GetLevel(question.Level);
            }

            //generate code
            GenerateCode(importSuccessList, import.CourseId);

            //add question
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                foreach (var question in importSuccessList)
                {
                    #region insert question
                    SqlCommand command = new SqlCommand(
                        "INSERT Question (QuestionContent,CourseId,QuestionCode, CategoryId, LearningOutcomeId, ImportId, LevelId, Image) " +
                        "OUTPUT INSERTED.Id AS 'Id' " +
                        "VALUES ( " +
                            "@questionContent, " +
                            "@courseId, " +
                            "@questionCode, " +
                            "@category, " +
                            "@learningOutcome, " +
                            "@importId, " +
                            "@level, " +
                            "@image" +
                        ")",
                        connection
                        );

                    command.Parameters.AddWithValue("@questionContent", question.QuestionContent);
                    command.Parameters.AddWithValue("@courseId", import.CourseId);
                    command.Parameters.AddWithValue("@questionCode", question.QuestionCode);
                    command.Parameters.AddWithValue("@category", question.CategoryId);
                    command.Parameters.AddWithValue("@learningOutcome", question.LearningOutcomeId);
                    command.Parameters.AddWithValue("@importId", import.ImportId);
                    command.Parameters.AddWithValue("@level", question.LevelId);
                    command.Parameters.AddWithValue("@image", question.Image);
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
                    question.QuestionId = id;
                }
            }

            #region add option
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                foreach (var question in importSuccessList)
                {
                    foreach (var option in question.Options)
                    {
                        SqlCommand optionCommand = new SqlCommand(
                                                    "INSERT [Option](QuestionId, OptionContent, IsCorrect) " +
                                                    "OUTPUT INSERTED.Id AS 'Id' " +
                                                    "VALUES ( " +
                                                        "@questionId, " +
                                                        "@content, " +
                                                        "@isCorrect " +
                                                    ")", connection);

                        optionCommand.Parameters.AddWithValue("@questionId", option.QuestionId);
                        optionCommand.Parameters.AddWithValue("@content", option.OptionContent);
                        optionCommand.Parameters.AddWithValue("@isCorrect", option.IsCorrect);
                        var rOp = optionCommand.ExecuteReader();
                        if (rOp.Read())
                        {
                            option.OptionId = (int)rOp["Id"];
                        }
                        rOp.Close();
                    }

                }
            }
            #endregion

            #region update image

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                foreach (var question in importSuccessList)
                {
                    SqlCommand optionCommand = new SqlCommand(
                                                "UPDATE Image " +
                                                "SET QuestionId=@questionId " +
                                                "WHERE QuestionTempId=@tempId"
                                                , connection);

                    optionCommand.Parameters.AddWithValue("@questionId", question.QuestionId);
                    optionCommand.Parameters.AddWithValue("@tempId", question.Id);
                    optionCommand.ExecuteNonQuery();

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
                                                "UPDATE Image " +
                                                "SET OptionTempId= NULL, OptionId = @optionId " +
                                                "WHERE OptionTempId=@id"
                                                , connection);

                        optionCommand.Parameters.AddWithValue("@id", option.Id);
                        optionCommand.Parameters.AddWithValue("@optionId", option.OptionId);
                        optionCommand.ExecuteNonQuery();

                    }

                }
            }

            #endregion

            return totalSuccess;
        }

        private static int? GetLevel(string name)
        {
            if (name != null)
            {
                if (name.Trim().ToLower().Equals(LevelEnum.Easy.ToString().ToLower()))
                {
                    return (int)LevelEnum.Easy;
                }

                if (name.Trim().ToLower().Equals(LevelEnum.Medium.ToString().ToLower()))
                {
                    return (int)LevelEnum.Medium;
                }

                if (name.Trim().ToLower().Equals(LevelEnum.Hard.ToString().ToLower()))
                {
                    return (int)LevelEnum.Hard;
                }
            }

            return null;
        }

        private static int? GetCategory(string name, int courseId)
        {
            if (name != null && !String.IsNullOrEmpty(name.Trim()))
            {
                int id = 0;
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(
                        "SELECT Id " +
                        "FROM Category " +
                        "WHERE Name = @name AND CourseId=@courseId",
                        connection
                        );

                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        id = (int)reader["id"];
                    }
                }

                if (id == 0)
                {
                    id = AddCategory(name, courseId);
                }
                if (id != 0)
                {
                    return id;
                }
            }
            return null;
        }

        private static int? GetLearningOutcome(string name, int courseId)
        {
            if (name != null && !String.IsNullOrEmpty(name.Trim()))
            {
                int id = 0;
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(
                        "SELECT Id " +
                        "FROM LearningOutcome " +
                        "WHERE Name = @name AND CourseId=@courseId",
                        connection
                        );

                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        id = (int)reader["id"];
                    }
                }


                if (id == 0)
                {
                    id = AddLearningOutcome(name, courseId);
                }
                if (id != 0)
                {
                    return id;
                }
            }
            return null;
        }

        private static int AddCategory(string name, int courseId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                                            "INSERT Category (Name, CourseId)" +
                                            "OUTPUT INSERTED.Id AS 'Id' " +
                                            "VALUES ( " +
                                                "@name, " +
                                                "@courseId " +
                                            ")", connection);

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@courseId", courseId);
                var reader = command.ExecuteReader();
                int id = 0;
                if (reader.Read())
                {
                    id = (int)reader["Id"];
                }
                reader.Close();
                return id;
            }
        }

        private static int AddLearningOutcome(string name, int courseId)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                                            "INSERT LearningOutcome (Name, CourseId)" +
                                            "OUTPUT INSERTED.Id AS 'Id' " +
                                            "VALUES ( " +
                                                "@name, " +
                                                "@courseId " +
                                            ")", connection);

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@courseId", courseId);
                var reader = command.ExecuteReader();
                int id = 0;
                if (reader.Read())
                {
                    id = (int)reader["Id"];
                }
                reader.Close();
                return id;
            }
        }

        private static int GetLastCode()
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT TOP 1 QuestionCode FROM Question ORDER BY Id DESC",
                    connection
                    );

                SqlDataReader reader = command.ExecuteReader();
                string code = "";
                if (reader.Read())
                {
                    if (reader["QuestionCode"] != null)
                    {
                        code = (string)reader["QuestionCode"];
                    }
                }

                int qIndex = code.LastIndexOf('Q');
                if (qIndex >= 0)
                {
                    int result = 0;
                    Int32.TryParse(code.Split('Q')[1], out result);
                    return result;
                }
            }
            return 0;
        }

        private static void GenerateCode(List<QuestionModel> questions, int courseId)
        {
            var no = GetLastCode() + 1;
            foreach (var q in questions)
            {
                //string prefix = q.QuestionCode.Split('-')[0];
                q.QuestionCode = "C" + courseId.ToString("D3") + '-' + 'Q' + no.ToString("D6");
                no += 1;
            }
        }

        private static List<QuestionModel> GetOtherImportQuestion(int importId, int courseId)
        {
            List<QuestionModel> import = new List<QuestionModel>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "select q.Id, q.Code, q.QuestionContent, o.OptionContent, o.IsCorrect, o.Id AS 'OptionId', q.Status, q.Category, q.LearningOutcome, q.LevelName, q.Image " +
                    "from Import i inner join QuestionTemp q on i.Id = q.ImportId inner join OptionTemp o on q.Id = o.TempId " +
                    "where i.CourseId = @courseId and i.Id != @importId and (i.Status = @checked or i.Status= @fixing)",
                    connection
                    );

                command.Parameters.AddWithValue("@importId", importId);
                command.Parameters.AddWithValue("@courseId", courseId);
                command.Parameters.AddWithValue("@checked", StatusEnum.Checked);
                command.Parameters.AddWithValue("@fixing", StatusEnum.Editing);
                SqlDataReader reader = command.ExecuteReader();

                int prev = 0;
                QuestionModel question = null;
                while (reader.Read())
                {
                    if (prev != (int)reader["Id"])
                    {
                        question = new QuestionModel();
                        question.QuestionContent = (string)reader["QuestionContent"];
                        if (reader["Image"] != DBNull.Value)
                        {
                            question.Image = (string)reader["Image"];
                        }
                        question.QuestionCode = (string)reader["Code"];
                        question.Status = (int)StatusEnum.Success;
                        question.Id = (int)reader["Id"];
                        if (reader["Category"] != DBNull.Value)
                        {
                            question.Category = (string)reader["Category"];
                        }
                        if (reader["LearningOutcome"] != DBNull.Value)
                        {
                            question.LearningOutcome = (string)reader["LearningOutcome"];
                        }
                        if (reader["LevelName"] != DBNull.Value)
                        {
                            question.Level = (string)reader["LevelName"];
                        }
                        question.IsBank = false;
                        question.Options = new List<OptionModel>();
                        question.Options.Add(new OptionModel
                        {
                            Id = (int)reader["OptionId"],
                            OptionContent = reader["OptionContent"] != DBNull.Value ? (string)reader["OptionContent"] : "",
                            IsCorrect = (bool)reader["IsCorrect"]
                        });
                        import.Add(question);

                        prev = question.Id;
                    }
                    else
                    {
                        question.Options.Add(new OptionModel
                        {
                            Id = (int)reader["OptionId"],
                            OptionContent = reader["OptionContent"] != DBNull.Value ? (string)reader["OptionContent"] : "",
                            IsCorrect = (bool)reader["IsCorrect"]
                        });
                    }

                }

            }
            return import;
        }

        #endregion

        #region check duplicate bank

        [SqlProcedure]
        public static void CheckBank()
        {
            var courseIds = GetCourseIds();

            foreach (var courseId in courseIds)
            {
                var questions = GetBank(courseId);
                GetImages(questions, true);
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();
                    foreach (var question in questions)
                    {
                        bool isUpdate = false;

                        List<string> duplicatedList = new List<string>();
                        CheckDuplicateAQuestionWithBank(question, questions, ref isUpdate, duplicatedList);
                        question.Test = String.Join(",", duplicatedList.ToArray());

                        //if (question.IsNotImage)
                        //{
                        //    question.Status = (int)StatusEnum.Editable;
                        //}


                        //update database
                        SqlCommand command = new SqlCommand(
                       "UPDATE Question " +
                       "SET Status=@status, DuplicatedQuestion=@test " +
                       "WHERE Id=@id",
                       connection
                       );

                        command.Parameters.AddWithValue("@status", question.Status != 0 ? question.Status : (int)StatusEnum.Success);
                        command.Parameters.AddWithValue("@test", question.Test);
                        command.Parameters.AddWithValue("@id", question.Id);

                        command.ExecuteNonQuery();

                    }
                }
            }

        }

        private static List<int> GetCourseIds()
        {
            List<int> ids = new List<int>();
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT Id " +
                    "FROM Course " +
                    "WHERE IsDisable = 0",
                    connection
                    );

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ids.Add((int)reader["Id"]);
                }

            }

            return ids;
        }

        #endregion
    }
}
