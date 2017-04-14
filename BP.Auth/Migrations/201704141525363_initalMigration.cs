namespace BP.Accounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountAttribute",
                c => new
                    {
                        RegistrationID = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RegistrationType_ID = c.Int(),
                        UserRole_ID = c.Int(),
                    })
                .PrimaryKey(t => t.RegistrationID)
                .ForeignKey("dbo.Registration", t => t.RegistrationID)
                .ForeignKey("dbo.RegistrationType", t => t.RegistrationType_ID)
                .ForeignKey("dbo.Role", t => t.UserRole_ID)
                .Index(t => t.RegistrationID)
                .Index(t => t.RegistrationType_ID)
                .Index(t => t.UserRole_ID);
            
            CreateTable(
                "dbo.DateTable",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DateLine = c.DateTimeOffset(nullable: false, precision: 7),
                        DateType_ID = c.Int(),
                        EmailAddress_ID = c.Int(),
                        IdentityAttribute_RegistrationID = c.Int(),
                        LoginAttribute_RegistrationID = c.Int(),
                        TokenClaim_ID = c.Int(),
                        AccountAttribute_RegistrationID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProfileDateType", t => t.DateType_ID)
                .ForeignKey("dbo.EmailAddress", t => t.EmailAddress_ID)
                .ForeignKey("dbo.IdentityAttribute", t => t.IdentityAttribute_RegistrationID)
                .ForeignKey("dbo.LoginAttribute", t => t.LoginAttribute_RegistrationID)
                .ForeignKey("dbo.TokenClaim", t => t.TokenClaim_ID)
                .ForeignKey("dbo.AccountAttribute", t => t.AccountAttribute_RegistrationID)
                .Index(t => t.DateType_ID)
                .Index(t => t.EmailAddress_ID)
                .Index(t => t.IdentityAttribute_RegistrationID)
                .Index(t => t.LoginAttribute_RegistrationID)
                .Index(t => t.TokenClaim_ID)
                .Index(t => t.AccountAttribute_RegistrationID);
            
            CreateTable(
                "dbo.ProfileDateType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EmailAddress",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Default = c.Boolean(nullable: false),
                        Validated = c.Boolean(nullable: false),
                        ValidationString = c.String(),
                        ProfileAttribute_RegistrationID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProfileAttribute", t => t.ProfileAttribute_RegistrationID)
                .Index(t => t.ProfileAttribute_RegistrationID);
            
            CreateTable(
                "dbo.ProfileAttribute",
                c => new
                    {
                        RegistrationID = c.Int(nullable: false),
                        Biography = c.String(),
                    })
                .PrimaryKey(t => t.RegistrationID)
                .ForeignKey("dbo.Registration", t => t.RegistrationID)
                .Index(t => t.RegistrationID);
            
            CreateTable(
                "dbo.PhoneNumber",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        Extension = c.String(),
                        PhoneNumberType_ID = c.Int(),
                        ProfileAttribute_RegistrationID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PhoneNumberType", t => t.PhoneNumberType_ID)
                .ForeignKey("dbo.ProfileAttribute", t => t.ProfileAttribute_RegistrationID)
                .Index(t => t.PhoneNumberType_ID)
                .Index(t => t.ProfileAttribute_RegistrationID);
            
            CreateTable(
                "dbo.PhoneNumberType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Registration",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IdentityAttribute",
                c => new
                    {
                        RegistrationID = c.Int(nullable: false),
                        Password = c.String(),
                        Salt = c.String(),
                        Title_ID = c.Int(),
                    })
                .PrimaryKey(t => t.RegistrationID)
                .ForeignKey("dbo.Registration", t => t.RegistrationID)
                .ForeignKey("dbo.Title", t => t.Title_ID)
                .Index(t => t.RegistrationID)
                .Index(t => t.Title_ID);
            
            CreateTable(
                "dbo.ResetQuestion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        IdentityAttribute_RegistrationID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.IdentityAttribute", t => t.IdentityAttribute_RegistrationID)
                .Index(t => t.IdentityAttribute_RegistrationID);
            
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
                "dbo.UserName",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IdentityAttribute_RegistrationID = c.Int(),
                        NameType_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.IdentityAttribute", t => t.IdentityAttribute_RegistrationID)
                .ForeignKey("dbo.NameType", t => t.NameType_ID)
                .Index(t => t.IdentityAttribute_RegistrationID)
                .Index(t => t.NameType_ID);
            
            CreateTable(
                "dbo.NameType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LoginAttribute",
                c => new
                    {
                        RegistrationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RegistrationID)
                .ForeignKey("dbo.Registration", t => t.RegistrationID)
                .Index(t => t.RegistrationID);
            
            CreateTable(
                "dbo.LoginToken",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        TokenDate = c.DateTimeOffset(nullable: false, precision: 7),
                        LoginAttribute_RegistrationID = c.Int(),
                        TokenProvider_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LoginAttribute", t => t.LoginAttribute_RegistrationID)
                .ForeignKey("dbo.TokenProvider", t => t.TokenProvider_ID)
                .Index(t => t.LoginAttribute_RegistrationID)
                .Index(t => t.TokenProvider_ID);
            
            CreateTable(
                "dbo.TokenProvider",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Provider = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TokenClaim",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Claim = c.String(),
                        ClaimEntity_ID = c.Int(),
                        LoginAttribute_RegistrationID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ClaimEntity", t => t.ClaimEntity_ID)
                .ForeignKey("dbo.LoginAttribute", t => t.LoginAttribute_RegistrationID)
                .Index(t => t.ClaimEntity_ID)
                .Index(t => t.LoginAttribute_RegistrationID);
            
            CreateTable(
                "dbo.ClaimEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClaimName = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RegistrationType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        Name = c.String(),
                        RoleGroup_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RoleGroup", t => t.RoleGroup_ID)
                .Index(t => t.RoleGroup_ID);
            
            CreateTable(
                "dbo.RoleGroup",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Cookie",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Data = c.String(),
                        DateSet = c.DateTimeOffset(nullable: false, precision: 7),
                        Expiration = c.DateTimeOffset(nullable: false, precision: 7),
                        CookieType_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CookieType", t => t.CookieType_ID)
                .Index(t => t.CookieType_ID);
            
            CreateTable(
                "dbo.CookieType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Index = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cookie", "CookieType_ID", "dbo.CookieType");
            DropForeignKey("dbo.Role", "RoleGroup_ID", "dbo.RoleGroup");
            DropForeignKey("dbo.AccountAttribute", "UserRole_ID", "dbo.Role");
            DropForeignKey("dbo.AccountAttribute", "RegistrationType_ID", "dbo.RegistrationType");
            DropForeignKey("dbo.AccountAttribute", "RegistrationID", "dbo.Registration");
            DropForeignKey("dbo.DateTable", "AccountAttribute_RegistrationID", "dbo.AccountAttribute");
            DropForeignKey("dbo.ProfileAttribute", "RegistrationID", "dbo.Registration");
            DropForeignKey("dbo.TokenClaim", "LoginAttribute_RegistrationID", "dbo.LoginAttribute");
            DropForeignKey("dbo.TokenClaim", "ClaimEntity_ID", "dbo.ClaimEntity");
            DropForeignKey("dbo.DateTable", "TokenClaim_ID", "dbo.TokenClaim");
            DropForeignKey("dbo.LoginAttribute", "RegistrationID", "dbo.Registration");
            DropForeignKey("dbo.LoginToken", "TokenProvider_ID", "dbo.TokenProvider");
            DropForeignKey("dbo.LoginToken", "LoginAttribute_RegistrationID", "dbo.LoginAttribute");
            DropForeignKey("dbo.DateTable", "LoginAttribute_RegistrationID", "dbo.LoginAttribute");
            DropForeignKey("dbo.UserName", "NameType_ID", "dbo.NameType");
            DropForeignKey("dbo.UserName", "IdentityAttribute_RegistrationID", "dbo.IdentityAttribute");
            DropForeignKey("dbo.IdentityAttribute", "Title_ID", "dbo.Title");
            DropForeignKey("dbo.ResetQuestion", "IdentityAttribute_RegistrationID", "dbo.IdentityAttribute");
            DropForeignKey("dbo.IdentityAttribute", "RegistrationID", "dbo.Registration");
            DropForeignKey("dbo.DateTable", "IdentityAttribute_RegistrationID", "dbo.IdentityAttribute");
            DropForeignKey("dbo.PhoneNumber", "ProfileAttribute_RegistrationID", "dbo.ProfileAttribute");
            DropForeignKey("dbo.PhoneNumber", "PhoneNumberType_ID", "dbo.PhoneNumberType");
            DropForeignKey("dbo.EmailAddress", "ProfileAttribute_RegistrationID", "dbo.ProfileAttribute");
            DropForeignKey("dbo.DateTable", "EmailAddress_ID", "dbo.EmailAddress");
            DropForeignKey("dbo.DateTable", "DateType_ID", "dbo.ProfileDateType");
            DropIndex("dbo.Cookie", new[] { "CookieType_ID" });
            DropIndex("dbo.Role", new[] { "RoleGroup_ID" });
            DropIndex("dbo.TokenClaim", new[] { "LoginAttribute_RegistrationID" });
            DropIndex("dbo.TokenClaim", new[] { "ClaimEntity_ID" });
            DropIndex("dbo.LoginToken", new[] { "TokenProvider_ID" });
            DropIndex("dbo.LoginToken", new[] { "LoginAttribute_RegistrationID" });
            DropIndex("dbo.LoginAttribute", new[] { "RegistrationID" });
            DropIndex("dbo.UserName", new[] { "NameType_ID" });
            DropIndex("dbo.UserName", new[] { "IdentityAttribute_RegistrationID" });
            DropIndex("dbo.ResetQuestion", new[] { "IdentityAttribute_RegistrationID" });
            DropIndex("dbo.IdentityAttribute", new[] { "Title_ID" });
            DropIndex("dbo.IdentityAttribute", new[] { "RegistrationID" });
            DropIndex("dbo.PhoneNumber", new[] { "ProfileAttribute_RegistrationID" });
            DropIndex("dbo.PhoneNumber", new[] { "PhoneNumberType_ID" });
            DropIndex("dbo.ProfileAttribute", new[] { "RegistrationID" });
            DropIndex("dbo.EmailAddress", new[] { "ProfileAttribute_RegistrationID" });
            DropIndex("dbo.DateTable", new[] { "AccountAttribute_RegistrationID" });
            DropIndex("dbo.DateTable", new[] { "TokenClaim_ID" });
            DropIndex("dbo.DateTable", new[] { "LoginAttribute_RegistrationID" });
            DropIndex("dbo.DateTable", new[] { "IdentityAttribute_RegistrationID" });
            DropIndex("dbo.DateTable", new[] { "EmailAddress_ID" });
            DropIndex("dbo.DateTable", new[] { "DateType_ID" });
            DropIndex("dbo.AccountAttribute", new[] { "UserRole_ID" });
            DropIndex("dbo.AccountAttribute", new[] { "RegistrationType_ID" });
            DropIndex("dbo.AccountAttribute", new[] { "RegistrationID" });
            DropTable("dbo.CookieType");
            DropTable("dbo.Cookie");
            DropTable("dbo.RoleGroup");
            DropTable("dbo.Role");
            DropTable("dbo.RegistrationType");
            DropTable("dbo.ClaimEntity");
            DropTable("dbo.TokenClaim");
            DropTable("dbo.TokenProvider");
            DropTable("dbo.LoginToken");
            DropTable("dbo.LoginAttribute");
            DropTable("dbo.NameType");
            DropTable("dbo.UserName");
            DropTable("dbo.Title");
            DropTable("dbo.ResetQuestion");
            DropTable("dbo.IdentityAttribute");
            DropTable("dbo.Registration");
            DropTable("dbo.PhoneNumberType");
            DropTable("dbo.PhoneNumber");
            DropTable("dbo.ProfileAttribute");
            DropTable("dbo.EmailAddress");
            DropTable("dbo.ProfileDateType");
            DropTable("dbo.DateTable");
            DropTable("dbo.AccountAttribute");
        }
    }
}
