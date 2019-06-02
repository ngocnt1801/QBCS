namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rule")]
    public partial class Rule
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public bool? IsDisable { get; set; }

        public DateTime? ActivateDate { get; set; }

        public int? KeyId { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? ValueGroup { get; set; }

        public virtual RuleKey RuleKey { get; set; }
    }
}
