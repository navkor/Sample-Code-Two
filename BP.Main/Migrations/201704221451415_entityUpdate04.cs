namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entityUpdate04 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessAttribute",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                        BusinessName = c.String(),
                        BusinessType_ID = c.Int(),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .ForeignKey("dbo.BusinessType", t => t.BusinessType_ID)
                .Index(t => t.AccountID)
                .Index(t => t.BusinessType_ID);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NickName = c.String(),
                        Longitude = c.String(),
                        Latitude = c.String(),
                        AddressType_ID = c.Int(),
                        BusinessAttribute_AccountID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AddressType", t => t.AddressType_ID)
                .ForeignKey("dbo.BusinessAttribute", t => t.BusinessAttribute_AccountID)
                .Index(t => t.AddressType_ID)
                .Index(t => t.BusinessAttribute_AccountID);
            
            CreateTable(
                "dbo.AddressCityAttribute",
                c => new
                    {
                        AddressID = c.Int(nullable: false),
                        City_ID = c.Int(),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.Address", t => t.AddressID)
                .ForeignKey("dbo.City", t => t.City_ID)
                .Index(t => t.AddressID)
                .Index(t => t.City_ID);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PostalCode_ID = c.Int(),
                        District_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PostalCode", t => t.PostalCode_ID)
                .ForeignKey("dbo.District", t => t.District_ID)
                .Index(t => t.PostalCode_ID)
                .Index(t => t.District_ID);
            
            CreateTable(
                "dbo.District",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Country_ID = c.Int(),
                        DistrictFormName_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Country", t => t.Country_ID)
                .ForeignKey("dbo.FormName", t => t.DistrictFormName_ID)
                .Index(t => t.Country_ID)
                .Index(t => t.DistrictFormName_ID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FormName",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PostalCode",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodeLine = c.String(),
                        PostalCodeFormName_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FormName", t => t.PostalCodeFormName_ID)
                .Index(t => t.PostalCodeFormName_ID);
            
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StreetAttribute",
                c => new
                    {
                        AddressID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.Address", t => t.AddressID)
                .Index(t => t.AddressID);
            
            CreateTable(
                "dbo.StringTable",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        Name = c.String(),
                        Title = c.String(),
                        StreetAttibute_AddressID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.StreetAttribute", t => t.StreetAttibute_AddressID)
                .Index(t => t.StreetAttibute_AddressID);
            
            CreateTable(
                "dbo.BusinessProgramAttribute",
                c => new
                    {
                        BusinessAttributeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BusinessAttributeID)
                .ForeignKey("dbo.BusinessAttribute", t => t.BusinessAttributeID)
                .Index(t => t.BusinessAttributeID);
            
            CreateTable(
                "dbo.BusinessType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserAttribute",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.UserProgramAttribute",
                c => new
                    {
                        UserAttributeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserAttributeID)
                .ForeignKey("dbo.UserAttribute", t => t.UserAttributeID)
                .Index(t => t.UserAttributeID);
            
            DropColumn("dbo.EntityAttribute", "AccountName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EntityAttribute", "AccountName", c => c.String());
            DropForeignKey("dbo.UserProgramAttribute", "UserAttributeID", "dbo.UserAttribute");
            DropForeignKey("dbo.UserAttribute", "AccountID", "dbo.Account");
            DropForeignKey("dbo.BusinessAttribute", "BusinessType_ID", "dbo.BusinessType");
            DropForeignKey("dbo.BusinessProgramAttribute", "BusinessAttributeID", "dbo.BusinessAttribute");
            DropForeignKey("dbo.StringTable", "StreetAttibute_AddressID", "dbo.StreetAttribute");
            DropForeignKey("dbo.StreetAttribute", "AddressID", "dbo.Address");
            DropForeignKey("dbo.Address", "BusinessAttribute_AccountID", "dbo.BusinessAttribute");
            DropForeignKey("dbo.Address", "AddressType_ID", "dbo.AddressType");
            DropForeignKey("dbo.AddressCityAttribute", "City_ID", "dbo.City");
            DropForeignKey("dbo.City", "District_ID", "dbo.District");
            DropForeignKey("dbo.PostalCode", "PostalCodeFormName_ID", "dbo.FormName");
            DropForeignKey("dbo.City", "PostalCode_ID", "dbo.PostalCode");
            DropForeignKey("dbo.District", "DistrictFormName_ID", "dbo.FormName");
            DropForeignKey("dbo.District", "Country_ID", "dbo.Country");
            DropForeignKey("dbo.AddressCityAttribute", "AddressID", "dbo.Address");
            DropForeignKey("dbo.BusinessAttribute", "AccountID", "dbo.Account");
            DropIndex("dbo.UserProgramAttribute", new[] { "UserAttributeID" });
            DropIndex("dbo.UserAttribute", new[] { "AccountID" });
            DropIndex("dbo.BusinessProgramAttribute", new[] { "BusinessAttributeID" });
            DropIndex("dbo.StringTable", new[] { "StreetAttibute_AddressID" });
            DropIndex("dbo.StreetAttribute", new[] { "AddressID" });
            DropIndex("dbo.PostalCode", new[] { "PostalCodeFormName_ID" });
            DropIndex("dbo.District", new[] { "DistrictFormName_ID" });
            DropIndex("dbo.District", new[] { "Country_ID" });
            DropIndex("dbo.City", new[] { "District_ID" });
            DropIndex("dbo.City", new[] { "PostalCode_ID" });
            DropIndex("dbo.AddressCityAttribute", new[] { "City_ID" });
            DropIndex("dbo.AddressCityAttribute", new[] { "AddressID" });
            DropIndex("dbo.Address", new[] { "BusinessAttribute_AccountID" });
            DropIndex("dbo.Address", new[] { "AddressType_ID" });
            DropIndex("dbo.BusinessAttribute", new[] { "BusinessType_ID" });
            DropIndex("dbo.BusinessAttribute", new[] { "AccountID" });
            DropTable("dbo.UserProgramAttribute");
            DropTable("dbo.UserAttribute");
            DropTable("dbo.BusinessType");
            DropTable("dbo.BusinessProgramAttribute");
            DropTable("dbo.StringTable");
            DropTable("dbo.StreetAttribute");
            DropTable("dbo.AddressType");
            DropTable("dbo.PostalCode");
            DropTable("dbo.FormName");
            DropTable("dbo.Country");
            DropTable("dbo.District");
            DropTable("dbo.City");
            DropTable("dbo.AddressCityAttribute");
            DropTable("dbo.Address");
            DropTable("dbo.BusinessAttribute");
        }
    }
}
