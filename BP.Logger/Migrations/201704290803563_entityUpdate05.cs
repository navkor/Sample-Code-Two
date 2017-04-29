namespace BP.Logger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate05 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SMTPSendDate", "SMTPAccounts_ID", "dbo.SMTPAccount");
            DropForeignKey("dbo.SMTPMessage", "SMTPAccount_ID", "dbo.SMTPAccount");
            DropForeignKey("dbo.SMTPAttachment", "SMTPMessages_ID", "dbo.SMTPMessage");
            DropForeignKey("dbo.SMTPMessageToAddressTable", "ToSMTPMessageId", "dbo.SMTPMessage");
            DropForeignKey("dbo.SMTPMessageToAddressTable", "SMTPMessageToId", "dbo.SMTPTo");
            DropIndex("dbo.SMTPSendDate", new[] { "SMTPAccounts_ID" });
            DropIndex("dbo.SMTPMessage", new[] { "SMTPAccount_ID" });
            DropIndex("dbo.SMTPAttachment", new[] { "SMTPMessages_ID" });
            DropIndex("dbo.SMTPMessageToAddressTable", new[] { "ToSMTPMessageId" });
            DropIndex("dbo.SMTPMessageToAddressTable", new[] { "SMTPMessageToId" });
            DropTable("dbo.SMTPAccount");
            DropTable("dbo.SMTPSendDate");
            DropTable("dbo.SMTPMessage");
            DropTable("dbo.SMTPAttachment");
            DropTable("dbo.SMTPTo");
            DropTable("dbo.SMTPMessageToAddressTable");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SMTPMessageToAddressTable",
                c => new
                    {
                        ToSMTPMessageId = c.Int(nullable: false),
                        SMTPMessageToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToSMTPMessageId, t.SMTPMessageToId });
            
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
                "dbo.SMTPAttachment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        SMTPMessages_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SMTPMessage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Message = c.String(),
                        SendDate = c.DateTime(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        Importance = c.Int(nullable: false),
                        SMTPAccount_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SMTPSendDate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SendDate = c.DateTime(nullable: false),
                        SendCount = c.Int(nullable: false),
                        AddressCount = c.Int(nullable: false),
                        SMTPAccounts_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SMTPAccount",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        AppSettings = c.String(),
                        DailyLimits = c.Int(nullable: false),
                        AddressLimits = c.Int(nullable: false),
                        PerMessageAddressLimits = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.SMTPMessageToAddressTable", "SMTPMessageToId");
            CreateIndex("dbo.SMTPMessageToAddressTable", "ToSMTPMessageId");
            CreateIndex("dbo.SMTPAttachment", "SMTPMessages_ID");
            CreateIndex("dbo.SMTPMessage", "SMTPAccount_ID");
            CreateIndex("dbo.SMTPSendDate", "SMTPAccounts_ID");
            AddForeignKey("dbo.SMTPMessageToAddressTable", "SMTPMessageToId", "dbo.SMTPTo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SMTPMessageToAddressTable", "ToSMTPMessageId", "dbo.SMTPMessage", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SMTPAttachment", "SMTPMessages_ID", "dbo.SMTPMessage", "ID");
            AddForeignKey("dbo.SMTPMessage", "SMTPAccount_ID", "dbo.SMTPAccount", "ID");
            AddForeignKey("dbo.SMTPSendDate", "SMTPAccounts_ID", "dbo.SMTPAccount", "ID");
        }
    }
}
