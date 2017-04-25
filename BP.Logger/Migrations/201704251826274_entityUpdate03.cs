namespace BP.Logger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate03 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SMTPTo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SMTPMessageToAddressTable",
                c => new
                    {
                        ToSMTPMessageId = c.Int(nullable: false),
                        SMTPMessageToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToSMTPMessageId, t.SMTPMessageToId })
                .ForeignKey("dbo.SMTPMessage", t => t.ToSMTPMessageId, cascadeDelete: true)
                .ForeignKey("dbo.SMTPTo", t => t.SMTPMessageToId, cascadeDelete: true)
                .Index(t => t.ToSMTPMessageId)
                .Index(t => t.SMTPMessageToId);
            
            DropColumn("dbo.SMTPMessage", "TO");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SMTPMessage", "TO", c => c.String());
            DropForeignKey("dbo.SMTPMessageToAddressTable", "SMTPMessageToId", "dbo.SMTPTo");
            DropForeignKey("dbo.SMTPMessageToAddressTable", "ToSMTPMessageId", "dbo.SMTPMessage");
            DropIndex("dbo.SMTPMessageToAddressTable", new[] { "SMTPMessageToId" });
            DropIndex("dbo.SMTPMessageToAddressTable", new[] { "ToSMTPMessageId" });
            DropTable("dbo.SMTPMessageToAddressTable");
            DropTable("dbo.SMTPTo");
        }
    }
}
