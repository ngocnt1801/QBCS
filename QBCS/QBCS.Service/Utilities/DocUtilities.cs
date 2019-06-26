using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Entity;
using QBCS.Service.ViewModel;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace QBCS.Service.Utilities
{
    public class DocUtilities
    {
        static QuestionTmpModel quesModel = new QuestionTmpModel();
        static OptionTemp optionModel = new OptionTemp();
        static string category = "";
        static string topic = "";
        static string level = "";
        static List<QuestionTmpModel> listQuestion = new List<QuestionTmpModel>();
        static List<OptionTemp> options = new List<OptionTemp>();

        public List<QuestionTmpModel> ParseDoc(Stream inputStream)
        {
            listQuestion = new List<QuestionTmpModel>();
            WordDocument wordDocument = new WordDocument(inputStream, FormatType.Automatic);
            foreach (WSection section in wordDocument.Sections)
            {
                WTextBody sectionBody = section.Body;
                IterateTextBody(sectionBody);
            }
            wordDocument.Close();
            return listQuestion;
        }

        private static void IterateTextBody(WTextBody textBody)
        {
            for (int i = 0; i < textBody.ChildEntities.Count; i++)
            {
                IEntity bodyItemEntity = textBody.ChildEntities[i];
                switch (bodyItemEntity.EntityType)
                {
                    case EntityType.Paragraph:
                        WParagraph paragraph = bodyItemEntity as WParagraph;
                        break;
                    case EntityType.Table:
                        WTable table = bodyItemEntity as WTable;
                        IterateTable(table);
                        break;
                }
            }
        }

        private static void IterateTable(WTable table)
        {
            List<DocViewModel> optionCheckList = new List<DocViewModel>();
            foreach (WTableRow row in table.Rows)
            {
                #region get key

                IEntity titleEntity = row.Cells[0].ChildEntities[0];
                string key = (titleEntity as WParagraph).Text;

                #endregion
                #region question content
                if (key.Contains("QN="))
                {
                    quesModel.Code = key.Replace("QN=", "");
                    for (int i = 0; i < row.Cells[1].ChildEntities.Count; i++)
                    {
                        IEntity bodyItemEntity = row.Cells[1].ChildEntities[i];
                        WParagraph paragraph = bodyItemEntity as WParagraph;
                        WParagraph checkImageNameNext = row.Cells[1].ChildEntities[i].NextSibling != null ? row.Cells[1].ChildEntities[i].NextSibling as WParagraph : null;
                        WParagraph checkImageNamePrevious = row.Cells[1].ChildEntities[i].PreviousSibling != null ? row.Cells[1].ChildEntities[i].PreviousSibling as WParagraph : null;
                        if(checkImageNamePrevious == null)
                        {
                            quesModel.QuestionContent = paragraph.Text;
                        }
                        else if (checkImageNameNext == null)
                        {

                        }
                        else if (paragraph.Text.Contains("[file:"))
                        {

                        }
                        else
                        {
                            quesModel.QuestionContent = "<br/>" +paragraph.Text;
                        }
                    }
                }
                #endregion
                else if (key.Contains("."))
                {
                    var optionCheck = new DocViewModel();
                    optionCheck.Code = key.Replace(".", "").ToLower();
                    for (int i = 0; i < row.Cells[1].ChildEntities.Count; i++)
                    {
                        IEntity bodyItemEntity = row.Cells[1].ChildEntities[i];
                        WParagraph paragraph = bodyItemEntity as WParagraph;
                        if(!(row.Cells[1].Count == 1 && (paragraph.Text.Equals("") || paragraph.Text == null)))
                        {
                            optionModel.IsCorrect = false;
                            if (i == 0)
                            {
                                optionModel.OptionContent = paragraph.Text;
                            }
                            else
                            {
                                optionModel.OptionContent = optionModel.OptionContent + "<br/>" + paragraph.Text;
                            }

                        }
                    }
                    optionCheck.Content = optionModel.OptionContent;
                    optionCheckList.Add(optionCheck);
                    if(optionModel.OptionContent != null)
                    {
                        options.Add(optionModel);
                    }
                    optionModel = new OptionTemp();
                }
                else if (key.Contains("ANSWER:"))
                {
                    IEntity bodyItemEntity = row.Cells[1].ChildEntities[0];
                    WParagraph paragraph = bodyItemEntity as WParagraph;
                    if (!paragraph.Text.Equals(""))
                    {
                        var trim = paragraph.Text.Replace(" ", "").ToLower();
                        char[] answers = trim.ToCharArray();
                        foreach (var optionCheck in optionCheckList)
                        {
                            for (int i = 0; i < answers.Length; i++)
                            {
                                if(optionCheck.Code.Equals(answers[i]))
                                {
                                    foreach(var option in options)
                                    {
                                        if (option.OptionContent.Equals(optionCheck.Content))
                                        {
                                            option.IsCorrect = true;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        
                    }
                }
                else if (key.Contains("UNIT:"))
                {
                    IEntity bodyItemEntity = row.Cells[1].ChildEntities[0];
                    WParagraph paragraph = bodyItemEntity as WParagraph;
                    if (!paragraph.Text.Equals(""))
                    {
                        quesModel.LearningOutcome = paragraph.Text;

                    }
                }
                //foreach (WTableCell cell in row.Cells)
                //{
                //    IterateTextBody(cell);
                //}
            }
            quesModel.Options = options;
            listQuestion.Add(quesModel);
            quesModel = new QuestionTmpModel();
            options = new List<OptionTemp>();
            optionCheckList = new List<DocViewModel>();
        }
    }

    public class DocViewModel
    {
        public string Code { get; set; }
        public string Content { get; set; }
    }
}
