namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogAction")]
    public partial class LogAction
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        [StringLength(200)]
        public string Action { get; set; }

        public string Message { get; set; }

        public DateTime? Date { get; set; }

        public int? TargetId { get; set; }

        [StringLength(200)]
        public string TargetName { get; set; }

        [StringLength(50)]
        public string Controller { get; set; }

        [StringLength(250)]
        public string Method { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public bool? IsDisable { get; set; }

        [StringLength(200)]
        public string Fullname { get; set; }

        [StringLength(200)]
        public string UserCode { get; set; }

        public int? Status { get; set; }
        public int? CourseId { get; set; }
        public int? LearningOutcomeId { get; set; }
        public int? CategoryId { get; set; }

    }
}
