namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        public int Id { get; set; }

        public string Source { get; set; }

        public int? QuestionId { get; set; }

        public int? QuestionTempId { get; set; }

        public int? QuestionInExamId { get; set; }

        public int? OptionId { get; set; }

        public int? OptionTempId { get; set; }

        public int? OptionInExamId { get; set; }

        public virtual Option Option { get; set; }

        public virtual OptionInExam OptionInExam { get; set; }

        public virtual OptionTemp OptionTemp { get; set; }

        public virtual Question Question { get; set; }

        public virtual QuestionInExam QuestionInExam { get; set; }

        public virtual QuestionTemp QuestionTemp { get; set; }
    }
}
