namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Import")]
    public partial class Import
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Import()
        {
            Questions = new HashSet<Question>();
            QuestionTemps = new HashSet<QuestionTemp>();
        }

        public int Id { get; set; }

        public int? UserId { get; set; }

        public DateTime? ImportedDate { get; set; }

        public int? Status { get; set; }

        public int? TotalQuestion { get; set; }

        public bool? Seen { get; set; }

        public int? CourseId { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? TotalSuccess { get; set; }

        [StringLength(200)]
        public string OwnerName { get; set; }

        public int? OwnerId { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionTemp> QuestionTemps { get; set; }
    }
}
