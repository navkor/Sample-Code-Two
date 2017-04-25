namespace BP.Logger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SMTPAccount", "AddressLimits", c => c.Int(nullable: false));
            AddColumn("dbo.SMTPAccount", "PerMessageAddressLimits", c => c.Int(nullable: false));
            AddColumn("dbo.SMTPSendDate", "AddressCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SMTPSendDate", "AddressCount");
            DropColumn("dbo.SMTPAccount", "PerMessageAddressLimits");
            DropColumn("dbo.SMTPAccount", "AddressLimits");
        }
    }
}
