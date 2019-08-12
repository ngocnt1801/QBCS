﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        static List<ImageViewModel> images = new List<ImageViewModel>();
        static OptionViewModel optionModel = new OptionViewModel();
        static string category = "";
        static string topic = "";
        static string level = "";
        static List<QuestionTmpModel> listQuestion = new List<QuestionTmpModel>();
        static List<OptionViewModel> options = new List<OptionViewModel>();
        static string globalPrefix = "";

        public List<QuestionTmpModel> ParseDoc(Stream inputStream, string prefix)
        {
            listQuestion = new List<QuestionTmpModel>();
            globalPrefix = prefix.Replace(" ","");
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
                if (key.Contains("QN=") || key.Contains("QN ="))
                {
                    quesModel.Code = key.Replace("QN=", "").Replace("QN =", "").Trim();
                    //for (int i = 0; i < row.Cells[1].ChildEntities.Count; i++)
                    //{
                    //    IEntity bodyItemEntity = row.Cells[1].ChildEntities[i];
                    //    WParagraph wParagraph = bodyItemEntity as WParagraph;
                    //    if (wParagraph.ChildEntities.Count != 0)
                    //    {
                    //        ParagraphItem pItem = wParagraph.ChildEntities[0] as ParagraphItem;
                    //        switch (pItem.EntityType)
                    //        {
                    //            default:
                    //                if (!(wParagraph.Text.Contains("[file") || wParagraph.Text.Equals("")) && quesModel.QuestionContent == null)
                    //                {
                    //                    quesModel.QuestionContent = "[html] " + wParagraph.Text.Replace("\v", "<cbr>");
                    //                }
                    //                else if (!(wParagraph.Text.Contains("[file") || wParagraph.Text.Equals("")))
                    //                {
                    //                    WTextRange text = pItem as WTextRange;
                    //                    quesModel.QuestionContent = quesModel.QuestionContent + "<cbr>" + wParagraph.Text;
                    //                }
                    //                break;
                    //            case EntityType.Picture:
                    //                WPicture wPicture = pItem as WPicture;
                    //                Image iImage = wPicture.Image;

                    //                MemoryStream m = new MemoryStream();
                    //                iImage.Save(m, iImage.RawFormat);
                    //                byte[] imageBytes = m.ToArray();

                    //                quesModel.Image = Convert.ToBase64String(imageBytes);
                    //                break;
                    //        }
                    //    }
                    //    if (quesModel.Image != null)
                    //    {
                    //        break;
                    //    }
                    //}

                    for (int i = 0; i < row.Cells[1].ChildEntities.Count; i++)
                    {
                        bool inputWholeParagraph = false;
                        IEntity bodyItemEntity = row.Cells[1].ChildEntities[i];
                        WParagraph wParagraph = bodyItemEntity as WParagraph;
                        if (wParagraph.ChildEntities.Count != 0)
                        {
                            foreach (var pChild in wParagraph.ChildEntities)
                            {
                                var pItem = pChild as ParagraphItem;
                                switch (pItem.EntityType)
                                {
                                    case EntityType.TextRange:
                                        if (!inputWholeParagraph)
                                        {
                                            if (!wParagraph.Text.Equals("") && quesModel.QuestionContent == null)
                                            {
                                                quesModel.QuestionContent = "[html] " + wParagraph.Text.Replace("\v", "<cbr>").Split(new string[] { "[file" }, StringSplitOptions.None)[0];
                                                inputWholeParagraph = true;
                                            }
                                            else if (!wParagraph.Text.Equals(""))
                                            {
                                                quesModel.QuestionContent = quesModel.QuestionContent + "<cbr>" + wParagraph.Text.Split(new string[] { "[file" }, StringSplitOptions.None)[0];
                                                inputWholeParagraph = true;
                                            }
                                        }
                                        break;
                                    case EntityType.Picture:
                                        WPicture wPicture = pItem as WPicture;
                                        System.Drawing.Image iImage = wPicture.Image;

                                        MemoryStream m = new MemoryStream();
                                        iImage.Save(m, iImage.RawFormat);
                                        byte[] imageBytes = m.ToArray();

                                        ImageViewModel image = new ImageViewModel();
                                        image.Source = Convert.ToBase64String(imageBytes);
                                        images.Add(image);
                                        break;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region option content
                else if (key.Contains("."))
                {
                    var optionImages = new List<ImageViewModel>();
                    var optionCheck = new DocViewModel();
                    optionCheck.Code = key.Replace(".", "").ToLower();
                    //for (int i = 0; i < row.Cells[1].ChildEntities.Count; i++)
                    //{
                    //    IEntity bodyItemEntity = row.Cells[1].ChildEntities[i];
                    //    WParagraph wParagraph = bodyItemEntity as WParagraph;
                    //    if (wParagraph.Text != "")
                    //    {
                    //        ParagraphItem pItem = wParagraph.ChildEntities[0] as ParagraphItem;
                    //        switch (pItem.EntityType)
                    //        {
                    //            //case EntityType.TextRange:
                    //            default:
                    //                if (!wParagraph.Text.Equals("") && optionModel.OptionContent == null)
                    //                {
                    //                    optionModel.IsCorrect = false;
                    //                    optionModel.OptionContent = wParagraph.Text.Replace("\v", "<cbr>");
                    //                }
                    //                else if (!wParagraph.Text.Equals(""))
                    //                {
                    //                    optionModel.OptionContent = optionModel.OptionContent + "<cbr>" + wParagraph.Text.Replace("\v", "<cbr>");
                    //                }
                    //                break;
                    //            case EntityType.Picture:
                    //                //WPicture wPicture = pItem as WPicture;
                    //                //Image iImage = wPicture.Image;

                    //                //MemoryStream m = new MemoryStream();
                    //                //iImage.Save(m, iImage.RawFormat);
                    //                //byte[] imageBytes = m.ToArray();

                    //                //quesModel.Image = Convert.ToBase64String(imageBytes);
                    //                break;
                    //        }
                    //    }
                    //}


                    for (int i = 0; i < row.Cells[1].ChildEntities.Count; i++)
                    {
                        bool inputWholeParagraph = false;
                        IEntity bodyItemEntity = row.Cells[1].ChildEntities[i];
                        WParagraph wParagraph = bodyItemEntity as WParagraph;
                        if (wParagraph.ChildEntities.Count != 0)
                        {
                            foreach (var pChild in wParagraph.ChildEntities)
                            {
                                var pItem = pChild as ParagraphItem;
                                switch (pItem.EntityType)
                                {
                                    case EntityType.TextRange:
                                        if (!inputWholeParagraph)
                                        {
                                            if (!wParagraph.Text.Equals("") && optionModel.OptionContent == null)
                                            {
                                                optionModel.IsCorrect = false;
                                                optionModel.OptionContent = wParagraph.Text.Replace("\v", "<cbr>");
                                                inputWholeParagraph = true;
                                            }
                                            else if (!wParagraph.Text.Equals(""))
                                            {
                                                optionModel.OptionContent = optionModel.OptionContent + "<cbr>" + wParagraph.Text;
                                            }
                                        }
                                        break;
                                    case EntityType.Picture:

                                        WPicture wPicture = pItem as WPicture;
                                        System.Drawing.Image iImage = wPicture.Image;

                                        MemoryStream m = new MemoryStream();
                                        iImage.Save(m, iImage.RawFormat);
                                        byte[] imageBytes = m.ToArray();

                                        ImageViewModel image = new ImageViewModel();
                                        image.Source = Convert.ToBase64String(imageBytes);
                                        optionImages.Add(image);
                                        break;
                                }
                            }
                        }
                    }
                    optionCheck.Content = optionModel.OptionContent;
                    optionCheckList.Add(optionCheck);
                    optionModel.Images = optionImages;
                    if (optionModel.OptionContent != null || (optionModel.Images != null && optionModel.Images.Count() != 0))
                    {
                        options.Add(optionModel);
                    }
                    optionModel = new OptionViewModel();
                }
                #endregion
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
                                if (optionCheck.Code.Equals(answers[i].ToString()))
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
                        var number = Regex.Match(paragraph.Text, @"\d+$").ToString();
                        if (globalPrefix == "")
                        {
                            quesModel.LearningOutcome = paragraph.Text;
                        }
                        else
                        {
                            quesModel.LearningOutcome = globalPrefix + " " + number;
                        }
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
                else if (key.Contains("CATEGORY:"))
                {
                    IEntity bodyItemEntity = row.Cells[1].ChildEntities[0];
                    WParagraph paragraph = bodyItemEntity as WParagraph;
                    if(paragraph != null && !paragraph.Text.Equals(""))
                    {
                        quesModel.Category = paragraph.Text;
                    }
                }
            }
            if(images != null)
            {
                quesModel.Images = images;
            }
            quesModel.Options = options;
            listQuestion.Add(quesModel);
            quesModel = new QuestionTmpModel();
            images = new List<ImageViewModel>();
            options = new List<OptionViewModel>();
            optionCheckList = new List<DocViewModel>();
        }
    }

    public class DocViewModel
    {
        public string Code { get; set; }
        public string Content { get; set; }
    }
}
