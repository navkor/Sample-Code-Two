namespace BP.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountDate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateLine = c.DateTimeOffset(nullable: false, precision: 7),
                        AccountDateType_ID = c.Int(),
                        EntityAttribute_AccountID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AccountDateType", t => t.AccountDateType_ID)
                .ForeignKey("dbo.EntityAttribute", t => t.EntityAttribute_AccountID)
                .Index(t => t.AccountDateType_ID)
                .Index(t => t.EntityAttribute_AccountID);
            
            CreateTable(
                "dbo.AccountDateType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EntityAttribute",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                        AccountType_ID = c.Int(),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .ForeignKey("dbo.AccountType", t => t.AccountType_ID)
                .Index(t => t.AccountID)
                .Index(t => t.AccountType_ID);
            
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EntityPreferenceAttribute",
                c => new
                    {
                        EntityAttributeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EntityAttributeID)
                .ForeignKey("dbo.EntityAttribute", t => t.EntityAttributeID)
                .Index(t => t.EntityAttributeID);
            
            CreateTable(
                "dbo.KeyWord",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.LoginAttribute",
                c => new
                    {
                        AccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.LoginId",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        LoginIdType_ID = c.Int(),
                        LoginAttribute_AccountID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LoginIdType", t => t.LoginIdType_ID)
                .ForeignKey("dbo.LoginAttribute", t => t.LoginAttribute_AccountID)
                .Index(t => t.LoginIdType_ID)
                .Index(t => t.LoginAttribute_AccountID);
            
            CreateTable(
                "dbo.LoginIdType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LoginPreferenceAttribute",
                c => new
                    {
                        LoginIdID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoginIdID)
                .ForeignKey("dbo.LoginId", t => t.LoginIdID)
                .Index(t => t.LoginIdID);
            
            CreateTable(
                "dbo.NewsLetter",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProfileAttribute",
                c => new
                    {
                        LoginIdID = c.Int(nullable: false),
                        Bio = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Hobbies = c.String(),
                        Goals = c.String(),
                        Gender_ID = c.Int(),
                    })
                .PrimaryKey(t => t.LoginIdID)
                .ForeignKey("dbo.Gender", t => t.Gender_ID)
                .ForeignKey("dbo.LoginId", t => t.LoginIdID)
                .Index(t => t.LoginIdID)
                .Index(t => t.Gender_ID);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NameAttribute",
                c => new
                    {
                        ProfileAttributeID = c.Int(nullable: false),
                        NameFormat_ID = c.Int(),
                        Title_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ProfileAttributeID)
                .ForeignKey("dbo.NameFormat", t => t.NameFormat_ID)
                .ForeignKey("dbo.ProfileAttribute", t => t.ProfileAttributeID)
                .ForeignKey("dbo.Title", t => t.Title_ID)
                .Index(t => t.ProfileAttributeID)
                .Index(t => t.NameFormat_ID)
                .Index(t => t.Title_ID);
            
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
                "dbo.UserNameType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.Title",
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
            
            CreateTable(
                "dbo.AccountType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AvoidAccountPreferenceTable",
                c => new
                    {
                        EntityAccountId = c.Int(nullable: false),
                        AccountEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityAccountId, t.AccountEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.EntityAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.AccountEntityId, cascadeDelete: true)
                .Index(t => t.EntityAccountId)
                .Index(t => t.AccountEntityId);
            
            CreateTable(
                "dbo.AvoidKeyWordPreferenceTable",
                c => new
                    {
                        EntityKeyWordId = c.Int(nullable: false),
                        KeyWordEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityKeyWordId, t.KeyWordEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.EntityKeyWordId, cascadeDelete: true)
                .ForeignKey("dbo.KeyWord", t => t.KeyWordEntityId, cascadeDelete: true)
                .Index(t => t.EntityKeyWordId)
                .Index(t => t.KeyWordEntityId);
            
            CreateTable(
                "dbo.PreferredAccountPreferenceTable",
                c => new
                    {
                        PreferredAccountId = c.Int(nullable: false),
                        AccountEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PreferredAccountId, t.AccountEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.PreferredAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.AccountEntityId, cascadeDelete: true)
                .Index(t => t.PreferredAccountId)
                .Index(t => t.AccountEntityId);
            
            CreateTable(
                "dbo.PreferredKeyWordPreferenceTable",
                c => new
                    {
                        EntityKeywordId = c.Int(nullable: false),
                        KeyWordEntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EntityKeywordId, t.KeyWordEntityId })
                .ForeignKey("dbo.EntityPreferenceAttribute", t => t.EntityKeywordId, cascadeDelete: true)
                .ForeignKey("dbo.KeyWord", t => t.KeyWordEntityId, cascadeDelete: true)
                .Index(t => t.EntityKeywordId)
                .Index(t => t.KeyWordEntityId);
            
            CreateTable(
                "dbo.PreferenceNewsLettersTable",
                c => new
                    {
                        NewsLettersPreferenceIds = c.Int(nullable: false),
                        PreferenceNewsLettersIds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewsLettersPreferenceIds, t.PreferenceNewsLettersIds })
                .ForeignKey("dbo.LoginPreferenceAttribute", t => t.NewsLettersPreferenceIds, cascadeDelete: true)
                .ForeignKey("dbo.NewsLetter", t => t.PreferenceNewsLettersIds, cascadeDelete: true)
                .Index(t => t.NewsLettersPreferenceIds)
                .Index(t => t.PreferenceNewsLettersIds);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EntityAttribute", "AccountType_ID", "dbo.AccountType");
            DropForeignKey("dbo.AccountDate", "EntityAttribute_AccountID", "dbo.EntityAttribute");
            DropForeignKey("dbo.EntityAttribute", "AccountID", "dbo.Account");
            DropForeignKey("dbo.UserProgramAttribute", "UserAttributeID", "dbo.UserAttribute");
            DropForeignKey("dbo.UserAttribute", "AccountID", "dbo.Account");
            DropForeignKey("dbo.LoginId", "LoginAttribute_AccountID", "dbo.LoginAttribute");
            DropForeignKey("dbo.NameAttribute", "Title_ID", "dbo.Title");
            DropForeignKey("dbo.NameAttribute", "ProfileAttributeID", "dbo.ProfileAttribute");
            DropForeignKey("dbo.NameAttribute", "NameFormat_ID", "dbo.NameFormat");
            DropForeignKey("dbo.NameFormatMap", "NameFormat_ID", "dbo.NameFormat");
            DropForeignKey("dbo.UserName", "UserNameType_ID", "dbo.UserNameType");
            DropForeignKey("dbo.UserName", "NameAttribute_ProfileAttributeID", "dbo.NameAttribute");
            DropForeignKey("dbo.NameFormatMap", "UserNameType_ID", "dbo.UserNameType");
            DropForeignKey("dbo.ProfileAttribute", "LoginIdID", "dbo.LoginId");
            DropForeignKey("dbo.ProfileAttribute", "Gender_ID", "dbo.Gender");
            DropForeignKey("dbo.PreferenceNewsLettersTable", "PreferenceNewsLettersIds", "dbo.NewsLetter");
            DropForeignKey("dbo.PreferenceNewsLettersTable", "NewsLettersPreferenceIds", "dbo.LoginPreferenceAttribute");
            DropForeignKey("dbo.LoginPreferenceAttribute", "LoginIdID", "dbo.LoginId");
            DropForeignKey("dbo.LoginId", "LoginIdType_ID", "dbo.LoginIdType");
            DropForeignKey("dbo.LoginAttribute", "AccountID", "dbo.Account");
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
            DropForeignKey("dbo.PreferredKeyWordPreferenceTable", "KeyWordEntityId", "dbo.KeyWord");
            DropForeignKey("dbo.PreferredKeyWordPreferenceTable", "EntityKeywordId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.PreferredAccountPreferenceTable", "AccountEntityId", "dbo.Account");
            DropForeignKey("dbo.PreferredAccountPreferenceTable", "PreferredAccountId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.EntityPreferenceAttribute", "EntityAttributeID", "dbo.EntityAttribute");
            DropForeignKey("dbo.AvoidKeyWordPreferenceTable", "KeyWordEntityId", "dbo.KeyWord");
            DropForeignKey("dbo.AvoidKeyWordPreferenceTable", "EntityKeyWordId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.AvoidAccountPreferenceTable", "AccountEntityId", "dbo.Account");
            DropForeignKey("dbo.AvoidAccountPreferenceTable", "EntityAccountId", "dbo.EntityPreferenceAttribute");
            DropForeignKey("dbo.AccountDate", "AccountDateType_ID", "dbo.AccountDateType");
            DropIndex("dbo.PreferenceNewsLettersTable", new[] { "PreferenceNewsLettersIds" });
            DropIndex("dbo.PreferenceNewsLettersTable", new[] { "NewsLettersPreferenceIds" });
            DropIndex("dbo.PreferredKeyWordPreferenceTable", new[] { "KeyWordEntityId" });
            DropIndex("dbo.PreferredKeyWordPreferenceTable", new[] { "EntityKeywordId" });
            DropIndex("dbo.PreferredAccountPreferenceTable", new[] { "AccountEntityId" });
            DropIndex("dbo.PreferredAccountPreferenceTable", new[] { "PreferredAccountId" });
            DropIndex("dbo.AvoidKeyWordPreferenceTable", new[] { "KeyWordEntityId" });
            DropIndex("dbo.AvoidKeyWordPreferenceTable", new[] { "EntityKeyWordId" });
            DropIndex("dbo.AvoidAccountPreferenceTable", new[] { "AccountEntityId" });
            DropIndex("dbo.AvoidAccountPreferenceTable", new[] { "EntityAccountId" });
            DropIndex("dbo.UserProgramAttribute", new[] { "UserAttributeID" });
            DropIndex("dbo.UserAttribute", new[] { "AccountID" });
            DropIndex("dbo.UserName", new[] { "UserNameType_ID" });
            DropIndex("dbo.UserName", new[] { "NameAttribute_ProfileAttributeID" });
            DropIndex("dbo.NameFormatMap", new[] { "NameFormat_ID" });
            DropIndex("dbo.NameFormatMap", new[] { "UserNameType_ID" });
            DropIndex("dbo.NameAttribute", new[] { "Title_ID" });
            DropIndex("dbo.NameAttribute", new[] { "NameFormat_ID" });
            DropIndex("dbo.NameAttribute", new[] { "ProfileAttributeID" });
            DropIndex("dbo.ProfileAttribute", new[] { "Gender_ID" });
            DropIndex("dbo.ProfileAttribute", new[] { "LoginIdID" });
            DropIndex("dbo.LoginPreferenceAttribute", new[] { "LoginIdID" });
            DropIndex("dbo.LoginId", new[] { "LoginAttribute_AccountID" });
            DropIndex("dbo.LoginId", new[] { "LoginIdType_ID" });
            DropIndex("dbo.LoginAttribute", new[] { "AccountID" });
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
            DropIndex("dbo.EntityPreferenceAttribute", new[] { "EntityAttributeID" });
            DropIndex("dbo.EntityAttribute", new[] { "AccountType_ID" });
            DropIndex("dbo.EntityAttribute", new[] { "AccountID" });
            DropIndex("dbo.AccountDate", new[] { "EntityAttribute_AccountID" });
            DropIndex("dbo.AccountDate", new[] { "AccountDateType_ID" });
            DropTable("dbo.PreferenceNewsLettersTable");
            DropTable("dbo.PreferredKeyWordPreferenceTable");
            DropTable("dbo.PreferredAccountPreferenceTable");
            DropTable("dbo.AvoidKeyWordPreferenceTable");
            DropTable("dbo.AvoidAccountPreferenceTable");
            DropTable("dbo.AccountType");
            DropTable("dbo.UserProgramAttribute");
            DropTable("dbo.UserAttribute");
            DropTable("dbo.Title");
            DropTable("dbo.UserName");
            DropTable("dbo.UserNameType");
            DropTable("dbo.NameFormatMap");
            DropTable("dbo.NameFormat");
            DropTable("dbo.NameAttribute");
            DropTable("dbo.Gender");
            DropTable("dbo.ProfileAttribute");
            DropTable("dbo.NewsLetter");
            DropTable("dbo.LoginPreferenceAttribute");
            DropTable("dbo.LoginIdType");
            DropTable("dbo.LoginId");
            DropTable("dbo.LoginAttribute");
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
            DropTable("dbo.KeyWord");
            DropTable("dbo.EntityPreferenceAttribute");
            DropTable("dbo.Account");
            DropTable("dbo.EntityAttribute");
            DropTable("dbo.AccountDateType");
            DropTable("dbo.AccountDate");
        }
    }
}
