using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedyWPF.Models;
using System.Data.Entity;

namespace FeedyWPF.Models
{
    class FeedyDbContext : DbContext
    {
        public FeedyDbContext() : base("FeedyDbContext")
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<TextData> TextDataSet { get; set; }
        public DbSet<CountData> CountDataSet { get; set; }
      //  public DbSet<Evaluation> Evaluations { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Question>()
            .HasMany(c => c.Querys).WithMany(i => i.Questions)
            .Map(t => t.MapLeftKey("QuestionID")
            .MapRightKey("QueryID")
            .ToTable("QuestionQuery"));

            modelBuilder.Entity<Event>()
            .HasMany(c => c.Querys).WithMany(i => i.Events)
            .Map(t => t.MapLeftKey("EventID")
            .MapRightKey("QueryID")
            .ToTable("EventQuery"));

            modelBuilder.Entity<TextData>()
                 .HasRequired(m => m.Answer)
                 .WithMany(t => t.TextDataSet)
                 .HasForeignKey(m => m.AnswerID)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<TextData>()
                 .HasRequired(m => m.Answer)
                 .WithMany(t => t.TextDataSet)
                 .HasForeignKey(m => m.AnswerID)
                 .WillCascadeOnDelete(false);


            modelBuilder.Entity<CountData>()
                  .HasRequired(m => m.Answer)
                 .WithMany(t => t.CountDataSet)
                 .HasForeignKey(m => m.AnswerID)
                 .WillCascadeOnDelete(false);

        }
    }
}
