namespace BP.Logger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SMTPAccount",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        AppSettings = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SMTPSendDate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SendDate = c.DateTime(nullable: false),
                        SendCount = c.Int(nullable: false),
                        SMTPAccounts_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SMTPAccount", t => t.SMTPAccounts_ID)
                .Index(t => t.SMTPAccounts_ID);
            
            CreateTable(
                "dbo.SMTPMessage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TO = c.String(),
                        Subject = c.String(),
                        Message = c.String(),
                        SendDate = c.DateTime(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        SMTPAccount_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SMTPAccount", t => t.SMTPAccount_ID)
                .Index(t => t.SMTPAccount_ID);
            
            CreateTable(
                "dbo.SMTPAttachment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        SMTPMessages_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SMTPMessage", t => t.SMTPMessages_ID)
                .Index(t => t.SMTPMessages_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SMTPAttachment", "SMTPMessages_ID", "dbo.SMTPMessage");
            DropForeignKey("dbo.SMTPMessage", "SMTPAccount_ID", "dbo.SMTPAccount");
            DropForeignKey("dbo.SMTPSendDate", "SMTPAccounts_ID", "dbo.SMTPAccount");
            DropIndex("dbo.SMTPAttachment", new[] { "SMTPMessages_ID" });
            DropIndex("dbo.SMTPMessage", new[] { "SMTPAccount_ID" });
            DropIndex("dbo.SMTPSendDate", new[] { "SMTPAccounts_ID" });
            DropTable("dbo.SMTPAttachment");
            DropTable("dbo.SMTPMessage");
            DropTable("dbo.SMTPSendDate");
            DropTable("dbo.SMTPAccount");
        }
    }
}
