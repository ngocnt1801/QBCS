using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
                        WParagraph wParagraph = bodyItemEntity as WParagraph;
                        if(wParagraph.ChildEntities.Count != 0)
                        {
                            ParagraphItem pItem = wParagraph.ChildEntities[0] as ParagraphItem;
                            switch (pItem.EntityType)
                            {
                                default:
                                    if (!(wParagraph.Text.Contains("[file") || wParagraph.Text.Equals("")) && quesModel.QuestionContent == null)
                                    {
                                        quesModel.QuestionContent = "[html] " + wParagraph.Text.Replace("\v", "<cbr>");
                                    }
                                    else if (!(wParagraph.Text.Contains("[file") || wParagraph.Text.Equals("")))
                                    {
                                        WTextRange text = pItem as WTextRange;
                                        quesModel.QuestionContent = quesModel.QuestionContent + "<cbr>" + wParagraph.Text;
                                    }
                                    break;
                                case EntityType.Picture:
                                    WPicture wPicture = pItem as WPicture;
                                    Image iImage = wPicture.Image;

                                    MemoryStream m = new MemoryStream();
                                    iImage.Save(m, iImage.RawFormat);
                                    byte[] imageBytes = m.ToArray();

                                    quesModel.Image = Convert.ToBase64String(imageBytes);
                                    break;
                            }
                        }
                        if(quesModel.Image != null)
                        {
                            break;
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
                        WParagraph wParagraph = bodyItemEntity as WParagraph;
                        if (wParagraph.Text != "")
                        {
                            ParagraphItem pItem = wParagraph.ChildEntities[0] as ParagraphItem;
                            switch (pItem.EntityType)
                            {
                                //case EntityType.TextRange:
                                default:
                                    if (!wParagraph.Text.Equals("") && optionModel.OptionContent == null)
                                    {
                                        optionModel.IsCorrect = false;
                                        optionModel.OptionContent = wParagraph.Text.Replace("\v", "<cbr>");
                                    }
                                    else if (!wParagraph.Text.Equals(""))
                                    {
                                        optionModel.OptionContent = optionModel.OptionContent + "<cbr>" + wParagraph.Text;
                                    }
                                    break;
                                case EntityType.Picture:
                                    //WPicture wPicture = pItem as WPicture;
                                    //Image iImage = wPicture.Image;

                                    //MemoryStream m = new MemoryStream();
                                    //iImage.Save(m, iImage.RawFormat);
                                    //byte[] imageBytes = m.ToArray();

                                    //quesModel.Image = Convert.ToBase64String(imageBytes);
                                    break;
                            }
                        }
                    }
                    optionCheck.Content = optionModel.OptionContent;
                    optionCheckList.Add(optionCheck);
                    if (optionModel.OptionContent != null)
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
                                if (optionCheck.Code.Equals(answers[i]))
                                {
                                    foreach (var option in options)
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
                        quesModel.LearningOutcome = "LearningOutcome" + paragraph.Text;
                    }
                }
                else if (key.Contains("MARK:"))
                {
                    IEntity bodyItemEntity = row.Cells[1].ChildEntities[0];
                    WParagraph paragraph = bodyItemEntity as WParagraph;
                    switch (paragraph.Text)
                    {
                        //default:
                        //    quesModel.Level = "Easy";
                        //    break;
                        case "1":
                            quesModel.Level = "Easy";
                            break;
                        case "2":
                            quesModel.Level = "Medium";
                            break;
                        case "3":
                            quesModel.Level = "Hard";
                            break;
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
