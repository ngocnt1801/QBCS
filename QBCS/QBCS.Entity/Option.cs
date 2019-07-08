namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Option")]
    public partial class Option
    {
        public int Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        public string OptionContent { get; set; }

        public int? QuestionId { get; set; }

        public bool? IsCorrect { get; set; }

        public string Image { get; set; }

        public virtual Question Question { get; set; }
    }
}
