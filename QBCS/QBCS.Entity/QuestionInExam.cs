namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuestionInExam")]
    public partial class QuestionInExam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuestionInExam()
        {
            OptionInExams = new HashSet<OptionInExam>();
        }

        public int Id { get; set; }

        public int? PartId { get; set; }

        public int? QuestionReference { get; set; }
        [Column(TypeName ="NVARCHAR")]
        public string QuestionContent { get; set; }

        public int? LevelId { get; set; }

        public int? TypeId { get; set; }

        public int? CategoryId { get; set; }

        public int? Priority { get; set; }
        [StringLength(50)]
        public string QuestionCode { get; set; }

        public string Image { get; set; }

        public int? Frequency { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OptionInExam> OptionInExams { get; set; }

        public virtual PartOfExamination PartOfExamination { get; set; }

        public virtual Question Question { get; set; }
    }
}
