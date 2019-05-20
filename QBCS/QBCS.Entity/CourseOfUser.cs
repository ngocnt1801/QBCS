namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CourseOfUser")]
    public partial class CourseOfUser
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }
    }
}
