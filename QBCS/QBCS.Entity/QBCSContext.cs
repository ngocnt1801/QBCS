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
        public virtual DbSet<CourseDepartment> CourseDepartments { get; set; }
        public virtual DbSet<CourseOfUser> CourseOfUsers { get; set; }
        public virtual DbSet<Examination> Examinations { get; set; }
        public virtual DbSet<LearningOutcome> LearningOutcomes { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<LevelInCourse> LevelInCourses { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<OptionInExam> OptionInExams { get; set; }
        public virtual DbSet<PartOfExamination> PartOfExaminations { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionInExam> QuestionInExams { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<CourseDepartment>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CourseDepartment>()
                .HasMany(e => e.Courses)
                .WithOptional(e => e.CourseDepartment)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<LearningOutcome>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LearningOutcome>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Level>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Option>()
                .Property(e => e.OptionContent)
                .IsUnicode(false);

            modelBuilder.Entity<OptionInExam>()
                .Property(e => e.OptionContent)
                .IsUnicode(false);

            modelBuilder.Entity<PartOfExamination>()
                .HasMany(e => e.QuestionInExams)
                .WithOptional(e => e.PartOfExamination)
                .HasForeignKey(e => e.PartId);

            modelBuilder.Entity<Question>()
                .Property(e => e.QuestionContent)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionInExams)
                .WithOptional(e => e.Question)
                .HasForeignKey(e => e.QuestionReference);

            modelBuilder.Entity<QuestionInExam>()
                .HasMany(e => e.OptionInExams)
                .WithOptional(e => e.QuestionInExam)
                .HasForeignKey(e => e.QuestionId);

            modelBuilder.Entity<QuestionType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionType>()
                .HasMany(e => e.Questions)
                .WithOptional(e => e.QuestionType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Topic>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Topic>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Fullname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}
