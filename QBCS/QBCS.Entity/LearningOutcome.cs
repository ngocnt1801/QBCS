namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LearningOutcome")]
    public partial class LearningOutcome
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LearningOutcome()
        {
            PartOfExaminations = new HashSet<PartOfExamination>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public int? CourseId { get; set; }

        public bool? IsDisable { get; set; }

        public int? SyllabusId { get; set; }

        public virtual Course Course { get; set; }

        public virtual SyllabusPartial SyllabusPartial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartOfExamination> PartOfExaminations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }
    }
}
