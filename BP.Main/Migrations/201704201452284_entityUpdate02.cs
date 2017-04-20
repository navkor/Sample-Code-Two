namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate02 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NameFormat",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NameFormatMap",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        UserNameType_ID = c.Int(),
                        NameFormat_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserNameType", t => t.UserNameType_ID)
                .ForeignKey("dbo.NameFormat", t => t.NameFormat_ID)
                .Index(t => t.UserNameType_ID)
                .Index(t => t.NameFormat_ID);
            
            CreateTable(
                "dbo.Title",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.NameAttribute", "NameFormat_ID", c => c.Int());
            AddColumn("dbo.NameAttribute", "Title_ID", c => c.Int());
            CreateIndex("dbo.NameAttribute", "NameFormat_ID");
            CreateIndex("dbo.NameAttribute", "Title_ID");
            AddForeignKey("dbo.NameAttribute", "NameFormat_ID", "dbo.NameFormat", "ID");
            AddForeignKey("dbo.NameAttribute", "Title_ID", "dbo.Title", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NameAttribute", "Title_ID", "dbo.Title");
            DropForeignKey("dbo.NameAttribute", "NameFormat_ID", "dbo.NameFormat");
            DropForeignKey("dbo.NameFormatMap", "NameFormat_ID", "dbo.NameFormat");
            DropForeignKey("dbo.NameFormatMap", "UserNameType_ID", "dbo.UserNameType");
            DropIndex("dbo.NameFormatMap", new[] { "NameFormat_ID" });
            DropIndex("dbo.NameFormatMap", new[] { "UserNameType_ID" });
            DropIndex("dbo.NameAttribute", new[] { "Title_ID" });
            DropIndex("dbo.NameAttribute", new[] { "NameFormat_ID" });
            DropColumn("dbo.NameAttribute", "Title_ID");
            DropColumn("dbo.NameAttribute", "NameFormat_ID");
            DropTable("dbo.Title");
            DropTable("dbo.NameFormatMap");
            DropTable("dbo.NameFormat");
        }
    }
}
