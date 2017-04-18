namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LoginAttribute",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.LoginId",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        LoginIdType_ID = c.Int(),
                        LoginAttribute_AccountID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LoginIdType", t => t.LoginIdType_ID)
                .ForeignKey("dbo.LoginAttribute", t => t.LoginAttribute_AccountID)
                .Index(t => t.LoginIdType_ID)
                .Index(t => t.LoginAttribute_AccountID);
            
            CreateTable(
                "dbo.LoginIdType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PreferenceAttribute",
                c => new
                    {
                        LoginIdID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoginIdID)
                .ForeignKey("dbo.LoginId", t => t.LoginIdID)
                .Index(t => t.LoginIdID);
            
            CreateTable(
                "dbo.NewsLetter",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProfileAttribute",
                c => new
                    {
                        LoginIdID = c.Int(nullable: false),
                        Bio = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Hobbies = c.String(),
                        Goals = c.String(),
                        Gender_ID = c.Int(),
                    })
                .PrimaryKey(t => t.LoginIdID)
                .ForeignKey("dbo.Gender", t => t.Gender_ID)
                .ForeignKey("dbo.LoginId", t => t.LoginIdID)
                .Index(t => t.LoginIdID)
                .Index(t => t.Gender_ID);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PreferenceNewsLettersTable",
                c => new
                    {
                        NewsLettersPreferenceIds = c.Int(nullable: false),
                        PreferenceNewsLettersIds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewsLettersPreferenceIds, t.PreferenceNewsLettersIds })
                .ForeignKey("dbo.PreferenceAttribute", t => t.NewsLettersPreferenceIds, cascadeDelete: true)
                .ForeignKey("dbo.NewsLetter", t => t.PreferenceNewsLettersIds, cascadeDelete: true)
                .Index(t => t.NewsLettersPreferenceIds)
                .Index(t => t.PreferenceNewsLettersIds);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoginId", "LoginAttribute_AccountID", "dbo.LoginAttribute");
            DropForeignKey("dbo.ProfileAttribute", "LoginIdID", "dbo.LoginId");
            DropForeignKey("dbo.ProfileAttribute", "Gender_ID", "dbo.Gender");
            DropForeignKey("dbo.PreferenceNewsLettersTable", "PreferenceNewsLettersIds", "dbo.NewsLetter");
            DropForeignKey("dbo.PreferenceNewsLettersTable", "NewsLettersPreferenceIds", "dbo.PreferenceAttribute");
            DropForeignKey("dbo.PreferenceAttribute", "LoginIdID", "dbo.LoginId");
            DropForeignKey("dbo.LoginId", "LoginIdType_ID", "dbo.LoginIdType");
            DropForeignKey("dbo.LoginAttribute", "AccountID", "dbo.Account");
            DropIndex("dbo.PreferenceNewsLettersTable", new[] { "PreferenceNewsLettersIds" });
            DropIndex("dbo.PreferenceNewsLettersTable", new[] { "NewsLettersPreferenceIds" });
            DropIndex("dbo.ProfileAttribute", new[] { "Gender_ID" });
            DropIndex("dbo.ProfileAttribute", new[] { "LoginIdID" });
            DropIndex("dbo.PreferenceAttribute", new[] { "LoginIdID" });
            DropIndex("dbo.LoginId", new[] { "LoginAttribute_AccountID" });
            DropIndex("dbo.LoginId", new[] { "LoginIdType_ID" });
            DropIndex("dbo.LoginAttribute", new[] { "AccountID" });
            DropTable("dbo.PreferenceNewsLettersTable");
            DropTable("dbo.Gender");
            DropTable("dbo.ProfileAttribute");
            DropTable("dbo.NewsLetter");
            DropTable("dbo.PreferenceAttribute");
            DropTable("dbo.LoginIdType");
            DropTable("dbo.LoginId");
            DropTable("dbo.LoginAttribute");
            DropTable("dbo.Account");
        }
    }
}
