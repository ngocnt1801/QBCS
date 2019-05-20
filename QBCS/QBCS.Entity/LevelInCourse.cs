namespace QBCS.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LevelInCourse")]
    public partial class LevelInCourse
    {
        public int Id { get; set; }

        public int? LevelId { get; set; }

        public int? CourseId { get; set; }

        public double? Mark { get; set; }

        public virtual Course Course { get; set; }

        public virtual Level Level { get; set; }
    }
}
