namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate02 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PresidenceType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.LoginId", "PresidenceType_ID", c => c.Int());
            CreateIndex("dbo.LoginId", "PresidenceType_ID");
            AddForeignKey("dbo.LoginId", "PresidenceType_ID", "dbo.PresidenceType", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoginId", "PresidenceType_ID", "dbo.PresidenceType");
            DropIndex("dbo.LoginId", new[] { "PresidenceType_ID" });
            DropColumn("dbo.LoginId", "PresidenceType_ID");
            DropTable("dbo.PresidenceType");
        }
    }
}
