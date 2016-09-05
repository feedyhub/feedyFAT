namespace FeedyWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerID)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.CountDatas",
                c => new
                    {
                        CountDataID = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        AnswerID = c.Int(nullable: false),
                        EventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CountDataID)
                .ForeignKey("dbo.Answers", t => t.AnswerID)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .Index(t => t.AnswerID)
                .Index(t => t.EventID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        Place = c.String(nullable: false),
                        ParticipantsCount = c.Int(nullable: false),
                        Date = c.DateTime(),
                        QuestionnaireID = c.Int(nullable: false),
                        IsSelected = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireID, cascadeDelete: true)
                .Index(t => t.QuestionnaireID);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        EvaluationID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.EvaluationID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        EvalMode = c.Int(nullable: false),
                        QuestionnaireID = c.Int(nullable: false),
                        IsSelected = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireID, cascadeDelete: true)
                .Index(t => t.QuestionnaireID);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        QuestionnaireID = c.Int(nullable: false, identity: true),
                        dummy = c.Int(nullable: false),
                        Name = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.QuestionnaireID);
            
            CreateTable(
                "dbo.TextDatas",
                c => new
                    {
                        TextDataID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        AnswerID = c.Int(nullable: false),
                        EventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextDataID)
                .ForeignKey("dbo.Answers", t => t.AnswerID)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .Index(t => t.AnswerID)
                .Index(t => t.EventID);
            
            CreateTable(
                "dbo.QuestionQuery",
                c => new
                    {
                        QuestionID = c.Int(nullable: false),
                        QueryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionID, t.QueryID })
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: true)
                .ForeignKey("dbo.Evaluations", t => t.QueryID, cascadeDelete: true)
                .Index(t => t.QuestionID)
                .Index(t => t.QueryID);
            
            CreateTable(
                "dbo.EventQuery",
                c => new
                    {
                        EventID = c.Int(nullable: false),
                        QueryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventID, t.QueryID })
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .ForeignKey("dbo.Evaluations", t => t.QueryID, cascadeDelete: true)
                .Index(t => t.EventID)
                .Index(t => t.QueryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextDatas", "EventID", "dbo.Events");
            DropForeignKey("dbo.TextDatas", "AnswerID", "dbo.Answers");
            DropForeignKey("dbo.EventQuery", "QueryID", "dbo.Evaluations");
            DropForeignKey("dbo.EventQuery", "EventID", "dbo.Events");
            DropForeignKey("dbo.Questions", "QuestionnaireID", "dbo.Questionnaires");
            DropForeignKey("dbo.Events", "QuestionnaireID", "dbo.Questionnaires");
            DropForeignKey("dbo.QuestionQuery", "QueryID", "dbo.Evaluations");
            DropForeignKey("dbo.QuestionQuery", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.CountDatas", "EventID", "dbo.Events");
            DropForeignKey("dbo.CountDatas", "AnswerID", "dbo.Answers");
            DropIndex("dbo.EventQuery", new[] { "QueryID" });
            DropIndex("dbo.EventQuery", new[] { "EventID" });
            DropIndex("dbo.QuestionQuery", new[] { "QueryID" });
            DropIndex("dbo.QuestionQuery", new[] { "QuestionID" });
            DropIndex("dbo.TextDatas", new[] { "EventID" });
            DropIndex("dbo.TextDatas", new[] { "AnswerID" });
            DropIndex("dbo.Questions", new[] { "QuestionnaireID" });
            DropIndex("dbo.Events", new[] { "QuestionnaireID" });
            DropIndex("dbo.CountDatas", new[] { "EventID" });
            DropIndex("dbo.CountDatas", new[] { "AnswerID" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.EventQuery");
            DropTable("dbo.QuestionQuery");
            DropTable("dbo.TextDatas");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.Questions");
            DropTable("dbo.Evaluations");
            DropTable("dbo.Events");
            DropTable("dbo.CountDatas");
            DropTable("dbo.Answers");
        }
    }
}
