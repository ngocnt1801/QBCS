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
            QuestionTemp1 = new HashSet<QuestionTemp>();
        }

        public int Id { get; set; }

        public int? ImportId { get; set; }

        public string QuestionContent { get; set; }

        public string OptionsContent { get; set; }

        public int? Status { get; set; }

        public int? DuplicatedId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public int? DuplicateInImportId { get; set; }

        public virtual Import Import { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OptionTemp> OptionTemps { get; set; }

        public virtual Question DuplicatedWithBank { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionTemp> QuestionTemp1 { get; set; }

        public virtual QuestionTemp DuplicatedWithImport { get; set; }
    }
}
