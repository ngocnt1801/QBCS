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
        public int Id { get; set; }

        public int? TempId { get; set; }

        public string OptionContent { get; set; }

        public bool? IsCorrect { get; set; }

        public virtual QuestionTemp QuestionTemp { get; set; }
    }
}
