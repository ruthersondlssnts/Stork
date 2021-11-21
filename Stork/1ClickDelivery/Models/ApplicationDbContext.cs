using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace _1ClickDelivery.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.RegisteredUser> RegisteredUsers { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.PickupAddress> PickupAddresses { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.ScheduledPickup> ScheduledPickups { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.Waybill> Waybills { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.Area> Areas { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.VBM> VBMs { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.ViewModels.FiveBookingPromoViewModel> FiveBookingPromoes { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.PickupDate> PickupDates { get; set; }

        public System.Data.Entity.DbSet<_1ClickDelivery.Models.Log> Logs { get; set; }

    }
}