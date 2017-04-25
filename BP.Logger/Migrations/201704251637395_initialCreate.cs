namespace BP.Logger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instigator",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        DateLine = c.DateTimeOffset(nullable: false, precision: 7),
                        Instigator_ID = c.Int(),
                        Subject_ID = c.Int(),
                        System_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Instigator", t => t.Instigator_ID)
                .ForeignKey("dbo.Subject", t => t.Subject_ID)
                .ForeignKey("dbo.System", t => t.System_ID)
                .Index(t => t.Instigator_ID)
                .Index(t => t.Subject_ID)
                .Index(t => t.System_ID);
            
            CreateTable(
                "dbo.Subject",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubjectLine = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.System",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Log", "System_ID", "dbo.System");
            DropForeignKey("dbo.Log", "Subject_ID", "dbo.Subject");
            DropForeignKey("dbo.Log", "Instigator_ID", "dbo.Instigator");
            DropIndex("dbo.Log", new[] { "System_ID" });
            DropIndex("dbo.Log", new[] { "Subject_ID" });
            DropIndex("dbo.Log", new[] { "Instigator_ID" });
            DropTable("dbo.System");
            DropTable("dbo.Subject");
            DropTable("dbo.Log");
            DropTable("dbo.Instigator");
        }
    }
}
