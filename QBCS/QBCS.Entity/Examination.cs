namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Examination")]
    public partial class Examination
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Examination()
        {
            PartOfExaminations = new HashSet<PartOfExamination>();
        }

        public int Id { get; set; }

        public int? CourseId { get; set; }

        public DateTime? GeneratedDate { get; set; }

        public int? NumberOfQuestion { get; set; }

        public int? NumberOfEasy { get; set; }

        public int? NumberOfMedium { get; set; }

        public int? NumberOfHard { get; set; }

        public double? MarkOfEasy { get; set; }

        public double? MarkOfMedium { get; set; }

        public double? MarkOfHard { get; set; }

        public bool? IsDisable { get; set; }

        public int? SemesterId { get; set; }

        [StringLength(50)]
        public string GroupExam { get; set; }

        [StringLength(10)]
        public string ExamCode { get; set; }

        public virtual Course Course { get; set; }

        public virtual Semester Semester { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartOfExamination> PartOfExaminations { get; set; }
    }
}
