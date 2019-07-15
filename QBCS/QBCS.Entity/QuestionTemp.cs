namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuestionTemp")]
    public partial class QuestionTemp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuestionTemp()
        {
            OptionTemps = new HashSet<OptionTemp>();
        }

        public int Id { get; set; }

        public int? ImportId { get; set; }

        public string QuestionContent { get; set; }

        public string DuplicatedString { get; set; }

        public int? Status { get; set; }

        public int? DuplicatedId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public int? DuplicateInImportId { get; set; }

        [StringLength(200)]
        public string Category { get; set; }

        [StringLength(200)]
        public string Topic { get; set; }

        [StringLength(200)]
        public string LevelName { get; set; }

        public string Message { get; set; }

        [StringLength(200)]
        public string LearningOutcome { get; set; }

        public string Image { get; set; }

        public int? Type { get; set; }

        public int? UpdateQuestionId { get; set; }

        public int? OldStatus { get; set; }

        public virtual Import Import { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OptionTemp> OptionTemps { get; set; }
    }
}
