namespace QBCS.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QBCSContext : DbContext
    {
        public QBCSContext()
            : base("name=QBCSContext")
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(e => e.CourseName)
                .IsFixedLength();

            modelBuilder.Entity<Option>()
                .Property(e => e.OptionContent)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.QuestionContent)
                .IsUnicode(false);
        }
    }
}
