namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartOfExamination")]
    public partial class PartOfExamination
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartOfExamination()
        {
            QuestionInExams = new HashSet<QuestionInExam>();
        }

        public int Id { get; set; }

        public int? TopicId { get; set; }

        public int? LearningOutcomeId { get; set; }

        public int? ExaminationId { get; set; }

        public int? NumberOfQuestion { get; set; }

        public virtual Examination Examination { get; set; }

        public virtual LearningOutcome LearningOutcome { get; set; }

        public virtual Topic Topic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionInExam> QuestionInExams { get; set; }
    }
}
