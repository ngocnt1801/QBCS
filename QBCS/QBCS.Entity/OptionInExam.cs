namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OptionInExam")]
    public partial class OptionInExam
    {
        public int Id { get; set; }

        public string OptionContent { get; set; }

        public int? QuestionId { get; set; }

        public bool? IsCorrect { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public virtual QuestionInExam QuestionInExam { get; set; }
    }
}
