using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedyWPF.Models;
using System.Data.Entity;

namespace FeedyWPF.Models
{
    public class FeedyDbContext : DbContext
    {
        public FeedyDbContext() : base("server=*******;port=3306;database=feedy;uid=feedy;password=*******")
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<TextData> TextDataSet { get; set; }
        public DbSet<CountData> CountDataSet { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Many to Many relationship
          
  

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