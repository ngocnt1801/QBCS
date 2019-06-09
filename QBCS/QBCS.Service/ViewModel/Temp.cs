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

        private decimal defaultgradeField;

        private bool defaultgradeFieldSpecified;

        private decimal penaltyField;

        private bool penaltyFieldSpecified;

        private byte hiddenField;

        private bool hiddenFieldSpecified;

        private bool singleField;

        private bool singleFieldSpecified;

        private bool shuffleanswersField;

        private bool shuffleanswersFieldSpecified;

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
        public decimal defaultgrade
        {
            get
            {
                return this.defaultgradeField;
            }
            set
            {
                this.defaultgradeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool defaultgradeSpecified
        {
            get
            {
                return this.defaultgradeFieldSpecified;
            }
            set
            {
                this.defaultgradeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal penalty
        {
            get
            {
                return this.penaltyField;
            }
            set
            {
                this.penaltyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool penaltySpecified
        {
            get
            {
                return this.penaltyFieldSpecified;
            }
            set
            {
                this.penaltyFieldSpecified = value;
            }
        }

        /// <remarks/>
        public byte hidden
        {
            get
            {
                return this.hiddenField;
            }
            set
            {
                this.hiddenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hiddenSpecified
        {
            get
            {
                return this.hiddenFieldSpecified;
            }
            set
            {
                this.hiddenFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool single
        {
            get
            {
                return this.singleField;
            }
            set
            {
                this.singleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool singleSpecified
        {
            get
            {
                return this.singleFieldSpecified;
            }
            set
            {
                this.singleFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool shuffleanswers
        {
            get
            {
                return this.shuffleanswersField;
            }
            set
            {
                this.shuffleanswersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool shuffleanswersSpecified
        {
            get
            {
                return this.shuffleanswersFieldSpecified;
            }
            set
            {
                this.shuffleanswersFieldSpecified = value;
            }
        }

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class quizQuestionQuestiontext
    {

        private string textField;

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

        private byte fractionField;

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
        public byte fraction
        {
            get
            {
                return this.fractionField;
            }
            set
            {
                this.fractionField = value;
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
    }



}
