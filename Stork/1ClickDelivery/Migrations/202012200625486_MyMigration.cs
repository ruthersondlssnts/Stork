namespace _1ClickDelivery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                {
                    PKArea = c.Guid(nullable: false),
                    AreaName = c.String(),
                })
                .PrimaryKey(t => t.PKArea);

            CreateTable(
                "dbo.FiveBookingPromoViewModels",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SenderName = c.String(),
                    BookingCount = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Logs",
                c => new
                {
                    PKLog = c.Guid(nullable: false),
                    DateTimeCreated = c.DateTime(nullable: false),
                    Message = c.String(),
                    CreatedBy = c.String(),
                })
                .PrimaryKey(t => t.PKLog);

            CreateTable(
                "dbo.PickupAddresses",
                c => new
                {
                    PKPickupAddress = c.Guid(nullable: false),
                    SenderId = c.String(),
                    Street = c.String(),
                    Unit = c.String(),
                    Area = c.String(nullable: false),
                    VillageBarangaMunicipality = c.String(nullable: false),
                    ContactPerson = c.String(nullable: false),
                    ContactPersonNo = c.String(nullable: false),
                    DateTimeCreated = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.PKPickupAddress);

            CreateTable(
                "dbo.PickupDates",
                c => new
                {
                    PKPickupDate = c.Guid(nullable: false),
                    Date = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.PKPickupDate);

            CreateTable(
                "dbo.RegisteredUsers",
                c => new
                {
                    PKRegisteredUser = c.Guid(nullable: false),
                    Email = c.String(nullable: false),
                    FirstName = c.String(nullable: false),
                    LastName = c.String(nullable: false),
                    PhoneNo = c.String(nullable: false),
                    Password = c.String(nullable: false),
                    DateCreated = c.DateTime(nullable: false),
                    DateTimeCreated = c.DateTime(nullable: false),
                    DateTimeUpdated = c.DateTime(nullable: false),
                    LastLoginDateTime = c.DateTime(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.PKRegisteredUser);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.ScheduledPickups",
                c => new
                {
                    PKScheduledPickup = c.Guid(nullable: false),
                    SenderId = c.String(),
                    NoItem = c.Int(nullable: false),
                    ReferenceNo = c.Int(nullable: false, identity: true),
                    SenderName = c.String(),
                    PickupAddress = c.String(),
                    SpecialInstruction = c.String(),
                    PickupArea = c.String(),
                    Status = c.String(),
                    DateOfPickup = c.DateTime(nullable: false),
                    CreatedBy = c.String(),
                    DateTimeCreated = c.DateTime(nullable: false),
                    DateTimeUpdated = c.DateTime(),
                })
                .PrimaryKey(t => t.PKScheduledPickup);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    FirstName = c.String(),
                    LastName = c.String(),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.VBMs",
                c => new
                {
                    PKVBM = c.Guid(nullable: false),
                    PKArea = c.Guid(nullable: false),
                    VBMName = c.String(),
                })
                .PrimaryKey(t => t.PKVBM);

            CreateTable(
                "dbo.Waybills",
                c => new
                {
                    PKWayBill = c.Guid(nullable: false),
                    SenderId = c.String(),
                    WayBillNo = c.Int(nullable: false, identity: true),
                    ManualWayBillNo = c.String(),
                    SenderName = c.String(nullable: false),
                    PickupArea = c.String(nullable: false),
                    PickupAddress = c.String(nullable: false),
                    ItemDescription = c.String(),
                    SpecialInstruction = c.String(),
                    DestinationAddress = c.String(nullable: false),
                    ReceiverName = c.String(nullable: false),
                    DeliveryArea = c.String(nullable: false),
                    ReceiverPhoneNo = c.String(nullable: false, maxLength: 15),
                    Status = c.String(),
                    DateOfPickup = c.DateTime(nullable: false),
                    CreatedBy = c.String(),
                    DateTimeCreated = c.DateTime(nullable: false),
                    DateTimeUpdated = c.DateTime(),
                    DateDelivered = c.DateTime(),
                })
                .PrimaryKey(t => t.PKWayBill);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.Waybills");
            DropTable("dbo.VBMs");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ScheduledPickups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RegisteredUsers");
            DropTable("dbo.PickupDates");
            DropTable("dbo.PickupAddresses");
            DropTable("dbo.Logs");
            DropTable("dbo.FiveBookingPromoViewModels");
            DropTable("dbo.Areas");
        }
    }
}
