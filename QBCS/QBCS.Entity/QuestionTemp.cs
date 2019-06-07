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
        public int Id { get; set; }

        public int? ImportId { get; set; }

        public string QuestionContent { get; set; }

        public string OptionsContent { get; set; }

        public int? Status { get; set; }

        public int? DuplicatedId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public virtual Import Import { get; set; }

        public virtual Question Question { get; set; }
    }
}
