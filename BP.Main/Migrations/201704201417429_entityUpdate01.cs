namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NameAttribute",
                c => new
                    {
                        ProfileAttributeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileAttributeID)
                .ForeignKey("dbo.ProfileAttribute", t => t.ProfileAttributeID)
                .Index(t => t.ProfileAttributeID);
            
            CreateTable(
                "dbo.UserName",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameAttribute_ProfileAttributeID = c.Int(),
                        UserNameType_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NameAttribute", t => t.NameAttribute_ProfileAttributeID)
                .ForeignKey("dbo.UserNameType", t => t.UserNameType_ID)
                .Index(t => t.NameAttribute_ProfileAttributeID)
                .Index(t => t.UserNameType_ID);
            
            CreateTable(
                "dbo.UserNameType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserName", "UserNameType_ID", "dbo.UserNameType");
            DropForeignKey("dbo.UserName", "NameAttribute_ProfileAttributeID", "dbo.NameAttribute");
            DropForeignKey("dbo.NameAttribute", "ProfileAttributeID", "dbo.ProfileAttribute");
            DropIndex("dbo.UserName", new[] { "UserNameType_ID" });
            DropIndex("dbo.UserName", new[] { "NameAttribute_ProfileAttributeID" });
            DropIndex("dbo.NameAttribute", new[] { "ProfileAttributeID" });
            DropTable("dbo.UserNameType");
            DropTable("dbo.UserName");
            DropTable("dbo.NameAttribute");
        }
    }
}
