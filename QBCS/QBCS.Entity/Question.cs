namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Options = new HashSet<Option>();
            QuestionInExams = new HashSet<QuestionInExam>();
            QuestionTemps = new HashSet<QuestionTemp>();
        }

        public int Id { get; set; }

        public string QuestionContent { get; set; }

        public int? CourseId { get; set; }

        public int? TopicId { get; set; }

        public int? LearningOutcomeId { get; set; }

        public int? LevelId { get; set; }

        public int? TypeId { get; set; }

        public int? Frequency { get; set; }

        public int? Priority { get; set; }

        public bool? IsDisable { get; set; }

        [StringLength(50)]
        public string QuestionCode { get; set; }

        public string Image { get; set; }

        public int? CategoryId { get; set; }

        public int? ImportId { get; set; }

        public int? Status { get; set; }

        public virtual Category Category { get; set; }

        public virtual Course Course { get; set; }

        public virtual Import Import { get; set; }

        public virtual LearningOutcome LearningOutcome { get; set; }

        public virtual Level Level { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Option> Options { get; set; }

        public virtual QuestionType QuestionType { get; set; }

        public virtual Topic Topic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionInExam> QuestionInExams { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionTemp> QuestionTemps { get; set; }
    }
}
