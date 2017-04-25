namespace BP.Logger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SMTPAccount", "DailyLimits", c => c.Int(nullable: false));
            AddColumn("dbo.SMTPMessage", "Importance", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SMTPMessage", "Importance");
            DropColumn("dbo.SMTPAccount", "DailyLimits");
        }
    }
}
