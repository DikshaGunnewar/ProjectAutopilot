namespace RepositoryLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalRepositoryTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccessToken = c.String(),
                        AccessTokenSecret = c.String(),
                        UpdateDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Expires_in = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Refresh_token = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuperTargetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccSettingId = c.Int(nullable: false),
                        UserName = c.String(),
                        SMId = c.String(),
                        Followers = c.Int(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccSettings", t => t.AccSettingId, cascadeDelete: true)
                .Index(t => t.AccSettingId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccSettingId = c.Int(nullable: false),
                        TagName = c.String(),
                        Location = c.String(),
                        IsBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccSettings", t => t.AccSettingId, cascadeDelete: true)
                .Index(t => t.AccSettingId);
            
            CreateTable(
                "dbo.UserManagements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccSettingId = c.Int(nullable: false),
                        userId = c.String(),
                        Email = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccSettings", t => t.AccSettingId, cascadeDelete: true)
                .Index(t => t.AccSettingId);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(),
                        socialId = c.Int(nullable: false),
                        SMId = c.String(),
                        PostId = c.String(),
                        OriginalPostId = c.String(),
                        Username = c.String(),
                        Activity = c.String(),
                        UserType = c.String(),
                        ActivityDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusinessCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Conversions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SMId = c.String(),
                        Username = c.String(),
                        socialId = c.Int(nullable: false),
                        Tag = c.String(),
                        ConvertDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FollowersGraphs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SocialId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Followers = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InstagramLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SocialId = c.Int(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                        LangCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SubscriptionPlanId = c.String(),
                        Amount = c.String(),
                        TransactionId = c.String(),
                        PaymentMethod = c.String(),
                        InvoiceId = c.String(),
                        Currency = c.String(),
                        Status = c.String(),
                        SocialId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocialMedias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Provider = c.String(),
                        Status = c.Boolean(nullable: false),
                        Followers = c.Int(nullable: false),
                        SMId = c.String(),
                        UserName = c.String(),
                        ProfilepicUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsInvalid = c.Boolean(nullable: false),
                        AccessDetails_Id = c.Int(),
                        AccSettings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessDetails", t => t.AccessDetails_Id)
                .ForeignKey("dbo.AccSettings", t => t.AccSettings_Id)
                .Index(t => t.AccessDetails_Id)
                .Index(t => t.AccSettings_Id);
            
            CreateTable(
                "dbo.SubscriptionsPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Price = c.Double(nullable: false),
                        BillingFrequency = c.String(),
                        NoOfAccounts = c.String(),
                        LimitedTagService = c.Boolean(nullable: false),
                        LowSpeedOfInteraction = c.Boolean(nullable: false),
                        AllowSuperTargeting = c.Boolean(nullable: false),
                        AllowNegativeTags = c.Boolean(nullable: false),
                        TagLimit = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsTrail = c.Boolean(nullable: false),
                        TrailDays = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAccountSubscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        socialIds = c.String(),
                        ExpiresOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PlanId = c.Int(nullable: false),
                        IsTrail = c.Boolean(nullable: false),
                        IsExpire = c.Boolean(nullable: false),
                        OrderId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserBillingAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Name = c.String(),
                        Company = c.String(),
                        Address = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        TaxId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialMedias", "AccSettings_Id", "dbo.AccSettings");
            DropForeignKey("dbo.SocialMedias", "AccessDetails_Id", "dbo.AccessDetails");
            DropForeignKey("dbo.UserManagements", "AccSettingId", "dbo.AccSettings");
            DropForeignKey("dbo.Tags", "AccSettingId", "dbo.AccSettings");
            DropForeignKey("dbo.SuperTargetUsers", "AccSettingId", "dbo.AccSettings");
            DropIndex("dbo.SocialMedias", new[] { "AccSettings_Id" });
            DropIndex("dbo.SocialMedias", new[] { "AccessDetails_Id" });
            DropIndex("dbo.UserManagements", new[] { "AccSettingId" });
            DropIndex("dbo.Tags", new[] { "AccSettingId" });
            DropIndex("dbo.SuperTargetUsers", new[] { "AccSettingId" });
            DropTable("dbo.UserBillingAddresses");
            DropTable("dbo.UserAccountSubscriptions");
            DropTable("dbo.SubscriptionsPlans");
            DropTable("dbo.SocialMedias");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Languages");
            DropTable("dbo.InstagramLocations");
            DropTable("dbo.FollowersGraphs");
            DropTable("dbo.Conversions");
            DropTable("dbo.BusinessCategories");
            DropTable("dbo.Activities");
            DropTable("dbo.UserManagements");
            DropTable("dbo.Tags");
            DropTable("dbo.SuperTargetUsers");
            DropTable("dbo.AccSettings");
            DropTable("dbo.AccessDetails");
        }
    }
}
