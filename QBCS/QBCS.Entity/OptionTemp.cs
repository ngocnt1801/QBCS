namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OptionTemp")]
    public partial class OptionTemp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OptionTemp()
        {
            Images = new HashSet<Image>();
        }

        public int Id { get; set; }

        public int? TempId { get; set; }

        public string OptionContent { get; set; }

        public bool? IsCorrect { get; set; }

        public int? UpdateOptionId { get; set; }

        public string Image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        public virtual QuestionTemp QuestionTemp { get; set; }
    }
}
