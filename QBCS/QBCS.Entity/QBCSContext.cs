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
            Database.CommandTimeout = 9000;
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseDepartment> CourseDepartments { get; set; }
        public virtual DbSet<CourseOfUser> CourseOfUsers { get; set; }
        public virtual DbSet<Examination> Examinations { get; set; }
        public virtual DbSet<Import> Imports { get; set; }
        public virtual DbSet<LearningOutcome> LearningOutcomes { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<LevelInCourse> LevelInCourses { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LogAction> LogActions { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<OptionInExam> OptionInExams { get; set; }
        public virtual DbSet<OptionTemp> OptionTemps { get; set; }
        public virtual DbSet<PartOfExamination> PartOfExaminations { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionInExam> QuestionInExams { get; set; }
        public virtual DbSet<QuestionTemp> QuestionTemps { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<RuleKey> RuleKeys { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Examination>()
                .Property(e => e.GroupExam)
                .IsUnicode(false);

            modelBuilder.Entity<Examination>()
                .Property(e => e.ExamCode)
                .IsUnicode(false);

            modelBuilder.Entity<LearningOutcome>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<LearningOutcome>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Level>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.TargetName)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Controller)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.Method)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.UserCode)
                .IsUnicode(false);

            modelBuilder.Entity<LogAction>()
                .Property(e => e.Controller)
                .IsUnicode(false);

            modelBuilder.Entity<LogAction>()
                .Property(e => e.Method)
                .IsUnicode(false);

            modelBuilder.Entity<Option>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<OptionInExam>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<OptionTemp>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<PartOfExamination>()
                .HasMany(e => e.QuestionInExams)
                .WithOptional(e => e.PartOfExamination)
                .HasForeignKey(e => e.PartId);

            modelBuilder.Entity<Question>()
                .Property(e => e.QuestionCode)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionInExams)
                .WithOptional(e => e.Question)
                .HasForeignKey(e => e.QuestionReference);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionTemps)
                .WithOptional(e => e.DuplicatedWithBank)
                .HasForeignKey(e => e.DuplicatedId);

            modelBuilder.Entity<QuestionInExam>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionInExam>()
                .Property(e => e.QuestionCode)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionInExam>()
                .HasMany(e => e.OptionInExams)
                .WithOptional(e => e.QuestionInExam)
                .HasForeignKey(e => e.QuestionId);

            modelBuilder.Entity<QuestionTemp>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionTemp>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<QuestionTemp>()
                .HasMany(e => e.OptionTemps)
                .WithOptional(e => e.QuestionTemp)
                .HasForeignKey(e => e.TempId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<QuestionTemp>()
                .HasMany(e => e.QuestionTemp1)
                .WithOptional(e => e.DuplicatedWithImport)
                .HasForeignKey(e => e.DuplicateInImportId);

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

            modelBuilder.Entity<RuleKey>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<RuleKey>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<RuleKey>()
                .HasMany(e => e.Rules)
                .WithOptional(e => e.RuleKey)
                .HasForeignKey(e => e.KeyId);

            modelBuilder.Entity<Semester>()
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
