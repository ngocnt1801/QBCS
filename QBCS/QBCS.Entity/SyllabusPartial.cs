namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SyllabusPartial")]
    public partial class SyllabusPartial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SyllabusPartial()
        {
            LearningOutcomes = new HashSet<LearningOutcome>();
        }

        public int Id { get; set; }

        public int? AmountQuestion { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LearningOutcome> LearningOutcomes { get; set; }
    }
}
