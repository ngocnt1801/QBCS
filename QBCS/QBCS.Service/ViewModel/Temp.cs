using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace QBCS.Service.ViewModel
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class quiz
    {

        private quizQuestion[] questionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("question")]
        public quizQuestion[] question
        {
            get
            {
                return this.questionField;
            }
            set
            {
                this.questionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestion
    {

        private quizQuestionName nameField;

        private quizQuestionQuestiontext questiontextField;

        private quizQuestionGeneralfeedback generalfeedbackField;

        private string defaultgradeField;

        //private bool defaultgradeFieldSpecified ;

        private string penaltyField;

        //private bool penaltyFieldSpecified;

        private string hiddenField;

        //private bool hiddenFieldSpecified;

        private string singleField;

       // private bool singleFieldSpecified;

        private string shuffleanswersField;

        //private bool shuffleanswersFieldSpecified;

        private string answernumberingField;

        private quizQuestionCorrectfeedback correctfeedbackField;

        private quizQuestionPartiallycorrectfeedback partiallycorrectfeedbackField;

        private quizQuestionIncorrectfeedback incorrectfeedbackField;

        private quizQuestionAnswer[] answerField;

        private quizQuestionCategory categoryField;

        private string typeField;

        /// <remarks/>
        public quizQuestionName name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public quizQuestionQuestiontext questiontext
        {
            get
            {
                return this.questiontextField;
            }
            set
            {
                this.questiontextField = value;
            }
        }

        /// <remarks/>
        public quizQuestionGeneralfeedback generalfeedback
        {
            get
            {
                return this.generalfeedbackField;
            }
            set
            {
                this.generalfeedbackField = value;
            }
        }

        /// <remarks/>
        public string defaultgrade
        {
            get
            {
                //return Decimal.Parse(this.defaultgradeField);
                return this.defaultgradeField.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.defaultgradeField = 0 + "";
                }
                else
                {
                    this.defaultgradeField = value.ToString();
                }
            }
            //set
            //{
            //    if (value.Equals(""))
            //    {
            //        this.defaultgradeField = 0 + "";
            //    }
            //    else
            //    {
            //        this.defaultgradeField = value + "";
            //    }
            //}
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool defaultgradeSpecified
        //{
        //    get
        //    {
        //        return this.defaultgradeFieldSpecified;
        //    }
        //    set
        //    {
        //        this.defaultgradeFieldSpecified = value;
        //    }
        //}

        /// <remarks/>
        public string penalty
        {
            get
            {
            
                return this.penaltyField.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.penaltyField = 0 + "";
                }
                else
                {
                    this.penaltyField = value.ToString();
                }
            }
            //get
            //{
            //    return Decimal.Parse(this.penaltyField);
            //}
            //set
            //{
            //    if (value.Equals(""))
            //    {
            //        this.penaltyField = 0 + "";
            //    }
            //    else
            //    {
            //        this.penaltyField = value + "";
            //    }
            //}

        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool penaltySpecified
        //{
        //    get
        //    {
        //        return this.penaltyFieldSpecified;
        //    }
        //    set
        //    {
        //        this.penaltyFieldSpecified = value;
        //    }
        //}

        /// <remarks/>
        public string hidden
        {
            get
            {
                return this.hiddenField.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.hiddenField = 0 + "";
                }
                else
                {
                    this.hiddenField = value.ToString();
                }
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool hiddenSpecified
        //{
        //    get
        //    {
        //        return this.hiddenFieldSpecified;
        //    }
        //    set
        //    {
        //        this.hiddenFieldSpecified = value;
        //    }
        //}

        /// <remarks/>
        public string single
        {
            get
            {
                return this.singleField.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.singleField = "true" + "";
                }
                else
                {
                    this.singleField = value.ToString();
                }
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool singleSpecified
        //{
        //    get
        //    {
        //        return this.singleFieldSpecified;
        //    }
        //    set
        //    {
        //        this.singleFieldSpecified = value;
        //    }
        //}

        /// <remarks/>
        public string shuffleanswers
        {
            get
            {
                return this.shuffleanswersField.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.shuffleanswersField = "true" + "";
                }
                else
                {
                    this.shuffleanswersField = value.ToString();
                }
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool shuffleanswersSpecified
        //{
        //    get
        //    {
        //        return this.shuffleanswersFieldSpecified;
        //    }
        //    set
        //    {
        //        this.shuffleanswersFieldSpecified = value;
        //    }
        //}

        /// <remarks/>
        public string answernumbering
        {
            get
            {
                return this.answernumberingField;
            }
            set
            {
                this.answernumberingField = value;
            }
        }

        /// <remarks/>
        public quizQuestionCorrectfeedback correctfeedback
        {
            get
            {
                return this.correctfeedbackField;
            }
            set
            {
                this.correctfeedbackField = value;
            }
        }

        /// <remarks/>
        public quizQuestionPartiallycorrectfeedback partiallycorrectfeedback
        {
            get
            {
                return this.partiallycorrectfeedbackField;
            }
            set
            {
                this.partiallycorrectfeedbackField = value;
            }
        }

        /// <remarks/>
        public quizQuestionIncorrectfeedback incorrectfeedback
        {
            get
            {
                return this.incorrectfeedbackField;
            }
            set
            {
                this.incorrectfeedbackField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("answer")]
        public quizQuestionAnswer[] answer
        {
            get
            {
                return this.answerField;
            }
            set
            {
                this.answerField = value;
            }
        }

        /// <remarks/>
        public quizQuestionCategory category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionName
    {

        private string textField;

        private quizQuestionNameFile fileField;

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public quizQuestionNameFile file
        {

            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionNameFile
    {

        private string nameField;

        private string pathField;

        private string encodingField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string encoding
        {
            get
            {
                return this.encodingField;
            }
            set
            {
                this.encodingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionQuestiontext
    {

        private string textField;

        private quizQuestionQuestiontextFile fileField;

        private string formatField;

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public quizQuestionQuestiontextFile file
        {
            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionQuestiontextFile
    {

        private string nameField;

        private string pathField;

        private string encodingField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string encoding
        {
            get
            {
                return this.encodingField;
            }
            set
            {
                this.encodingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionGeneralfeedback
    {

        private object textField;

        private string formatField;

        /// <remarks/>
        public object text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionCorrectfeedback
    {

        private object textField;

        private string formatField;

        /// <remarks/>
        public object text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionPartiallycorrectfeedback
    {

        private object textField;

        private string formatField;

        /// <remarks/>
        public object text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionIncorrectfeedback
    {

        private object textField;

        private string formatField;

        /// <remarks/>
        public object text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionAnswer
    {

        private string textField;

        private quizQuestionAnswerFeedback feedbackField;

        private string fractionField;

        private string formatField;

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public quizQuestionAnswerFeedback feedback
        {
            get
            {
                return this.feedbackField;
            }
            set
            {
                this.feedbackField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string fraction
        {
            get
            {
                return this.fractionField.ToString();
               // return Byte.Parse(this.fractionField);
            }
            set
            {
                if (value.Equals(""))
                {
                    this.fractionField = 0 + "";
                }
                else
                {
                    this.fractionField = value.ToString();
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionAnswerFeedback
    {

        private object textField;

        private string formatField;

        /// <remarks/>
        public object text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionCategory
    {

        private string textField;

        private quizQuestionCategoryFile fileField;

        /// <remarks/>
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        public quizQuestionCategoryFile file
        {
            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionCategoryFile
    {

        private string nameField;

        private string pathField;

        private string encodingField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string encoding
        {
            get
            {
                return this.encodingField;
            }
            set
            {
                this.encodingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
