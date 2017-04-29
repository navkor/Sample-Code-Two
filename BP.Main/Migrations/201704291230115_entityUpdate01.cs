namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoginId", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoginId", "EmailAddress");
        }
    }
}
