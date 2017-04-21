namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate03 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PreferenceAttribute", newName: "LoginPreferenceAttribute");
            CreateTable(
                "dbo.AccountDate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateLine = c.DateTimeOffset(nullable: false, precision: 7),
                        AccountDateType_ID = c.Int(),
                        EntityAttribute_AccountID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AccountDateType", t => t.AccountDateType_ID)
                .ForeignKey("dbo.EntityAttribute", t => t.EntityAttribute_AccountID)
                .Index(t => t.AccountDateType_ID)
                .Index(t => t.EntityAttribute_AccountID);
            
            CreateTable(
                "dbo.AccountDateType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EntityAttribute",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                        AccountName = c.String(),
                        AccountType_ID = c.Int(),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .ForeignKey("dbo.AccountType", t => t.AccountType_ID)
                .Index(t => t.AccountID)
                .Index(t => t.AccountType_ID);
            
            CreateTable(
                "dbo.EntityPreferenceAttribute",
                c => new
                    {
                        EntityAttributeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EntityAttributeID)
                .ForeignKey("dbo.EntityAttribute", t => t.EntityAttributeID)
                .Index(t => t.EntityAttributeID);
            
            CreateTable(
                "dbo.KeyWord",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AccountType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AvoidAccountPreferenceTable",
                c => new
                    {
                        EntityAccountId = c.Int(nullable: false),
                        AccountEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityAccountId, t.AccountEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.EntityAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.AccountEntityId, cascadeDelete: true)
                .Index(t => t.EntityAccountId)
                .Index(t => t.AccountEntityId);
            
            CreateTable(
                "dbo.AvoidKeyWordPreferenceTable",
                c => new
                    {
                        EntityKeyWordId = c.Int(nullable: false),
                        KeyWordEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityKeyWordId, t.KeyWordEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.EntityKeyWordId, cascadeDelete: true)
                .ForeignKey("dbo.KeyWord", t => t.KeyWordEntityId, cascadeDelete: true)
                .Index(t => t.EntityKeyWordId)
                .Index(t => t.KeyWordEntityId);
            
            CreateTable(
                "dbo.PreferredAccountPreferenceTable",
                c => new
                    {
                        PreferredAccountId = c.Int(nullable: false),
                        AccountEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PreferredAccountId, t.AccountEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.PreferredAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.AccountEntityId, cascadeDelete: true)
                .Index(t => t.PreferredAccountId)
                .Index(t => t.AccountEntityId);
            
            CreateTable(
                "dbo.PreferredKeyWordPreferenceTable",
                c => new
                    {
                        EntityKeywordId = c.Int(nullable: false),
                        KeyWordEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityKeywordId, t.KeyWordEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.EntityKeywordId, cascadeDelete: true)
                .ForeignKey("dbo.KeyWord", t => t.KeyWordEntityId, cascadeDelete: true)
                .Index(t => t.EntityKeywordId)
                .Index(t => t.KeyWordEntityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntityAttribute", "AccountType_ID", "dbo.AccountType");
            DropForeignKey("dbo.AccountDate", "EntityAttribute_AccountID", "dbo.EntityAttribute");
            DropForeignKey("dbo.EntityAttribute", "AccountID", "dbo.Account");
            DropForeignKey("dbo.PreferredKeyWordPreferenceTable", "KeyWordEntityId", "dbo.KeyWord");
            DropForeignKey("dbo.PreferredKeyWordPreferenceTable", "EntityKeywordId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.PreferredAccountPreferenceTable", "AccountEntityId", "dbo.Account");
            DropForeignKey("dbo.PreferredAccountPreferenceTable", "PreferredAccountId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.EntityPreferenceAttribute", "EntityAttributeID", "dbo.EntityAttribute");
            DropForeignKey("dbo.AvoidKeyWordPreferenceTable", "KeyWordEntityId", "dbo.KeyWord");
            DropForeignKey("dbo.AvoidKeyWordPreferenceTable", "EntityKeyWordId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.AvoidAccountPreferenceTable", "AccountEntityId", "dbo.Account");
            DropForeignKey("dbo.AvoidAccountPreferenceTable", "EntityAccountId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.AccountDate", "AccountDateType_ID", "dbo.AccountDateType");
            DropIndex("dbo.PreferredKeyWordPreferenceTable", new[] { "KeyWordEntityId" });
            DropIndex("dbo.PreferredKeyWordPreferenceTable", new[] { "EntityKeywordId" });
            DropIndex("dbo.PreferredAccountPreferenceTable", new[] { "AccountEntityId" });
            DropIndex("dbo.PreferredAccountPreferenceTable", new[] { "PreferredAccountId" });
            DropIndex("dbo.AvoidKeyWordPreferenceTable", new[] { "KeyWordEntityId" });
            DropIndex("dbo.AvoidKeyWordPreferenceTable", new[] { "EntityKeyWordId" });
            DropIndex("dbo.AvoidAccountPreferenceTable", new[] { "AccountEntityId" });
            DropIndex("dbo.AvoidAccountPreferenceTable", new[] { "EntityAccountId" });
            DropIndex("dbo.EntityPreferenceAttribute", new[] { "EntityAttributeID" });
            DropIndex("dbo.EntityAttribute", new[] { "AccountType_ID" });
            DropIndex("dbo.EntityAttribute", new[] { "AccountID" });
            DropIndex("dbo.AccountDate", new[] { "EntityAttribute_AccountID" });
            DropIndex("dbo.AccountDate", new[] { "AccountDateType_ID" });
            DropTable("dbo.PreferredKeyWordPreferenceTable");
            DropTable("dbo.PreferredAccountPreferenceTable");
            DropTable("dbo.AvoidKeyWordPreferenceTable");
            DropTable("dbo.AvoidAccountPreferenceTable");
            DropTable("dbo.AccountType");
            DropTable("dbo.KeyWord");
            DropTable("dbo.EntityPreferenceAttribute");
            DropTable("dbo.EntityAttribute");
            DropTable("dbo.AccountDateType");
            DropTable("dbo.AccountDate");
            RenameTable(name: "dbo.LoginPreferenceAttribute", newName: "PreferenceAttribute");
        }
    }
}
