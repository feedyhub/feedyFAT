namespace FeedyWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerID = c.Int(nullable: false, identity: true),
                        Text = c.String(unicode: false),
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
                        Place = c.String(nullable: false, unicode: false),
                        ParticipantsCount = c.Int(nullable: false),
                        Date = c.DateTime(precision: 0),
                        QuestionnaireID = c.Int(nullable: false),
                        IsSelected = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Evaluation_EvaluationID = c.Int(),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireID, cascadeDelete: true)
                .ForeignKey("dbo.Evaluations", t => t.Evaluation_EvaluationID)
                .Index(t => t.QuestionnaireID)
                .Index(t => t.Evaluation_EvaluationID);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        QuestionnaireID = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Comments = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.QuestionnaireID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        Text = c.String(unicode: false),
                        QuestionType = c.Int(nullable: false),
                        EvalMode = c.Int(nullable: false),
                        QuestionnaireID = c.Int(nullable: false),
                        IsSelected = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Evaluation_EvaluationID = c.Int(),
                    })
                .PrimaryKey(t => t.QuestionID)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireID, cascadeDelete: true)
                .ForeignKey("dbo.Evaluations", t => t.Evaluation_EvaluationID)
                .Index(t => t.QuestionnaireID)
                .Index(t => t.Evaluation_EvaluationID);
            
            CreateTable(
                "dbo.TextDatas",
                c => new
                    {
                        TextDataID = c.Int(nullable: false, identity: true),
                        Text = c.String(unicode: false),
                        AnswerID = c.Int(nullable: false),
                        EventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TextDataID)
                .ForeignKey("dbo.Answers", t => t.AnswerID)
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .Index(t => t.AnswerID)
                .Index(t => t.EventID);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        EvaluationID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.EvaluationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "Evaluation_EvaluationID", "dbo.Evaluations");
            DropForeignKey("dbo.Events", "Evaluation_EvaluationID", "dbo.Evaluations");
            DropForeignKey("dbo.TextDatas", "EventID", "dbo.Events");
            DropForeignKey("dbo.TextDatas", "AnswerID", "dbo.Answers");
            DropForeignKey("dbo.Questions", "QuestionnaireID", "dbo.Questionnaires");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Events", "QuestionnaireID", "dbo.Questionnaires");
            DropForeignKey("dbo.CountDatas", "EventID", "dbo.Events");
            DropForeignKey("dbo.CountDatas", "AnswerID", "dbo.Answers");
            DropIndex("dbo.TextDatas", new[] { "EventID" });
            DropIndex("dbo.TextDatas", new[] { "AnswerID" });
            DropIndex("dbo.Questions", new[] { "Evaluation_EvaluationID" });
            DropIndex("dbo.Questions", new[] { "QuestionnaireID" });
            DropIndex("dbo.Events", new[] { "Evaluation_EvaluationID" });
            DropIndex("dbo.Events", new[] { "QuestionnaireID" });
            DropIndex("dbo.CountDatas", new[] { "EventID" });
            DropIndex("dbo.CountDatas", new[] { "AnswerID" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.Evaluations");
            DropTable("dbo.TextDatas");
            DropTable("dbo.Questions");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.Events");
            DropTable("dbo.CountDatas");
            DropTable("dbo.Answers");
        }
    }
}
