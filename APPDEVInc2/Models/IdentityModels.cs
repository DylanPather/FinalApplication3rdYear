using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using APPDEVInc2.DataBaseModels;
namespace APPDEVInc2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ServiceTbl> ServiceTbls { get; set; }
        public DbSet<StockTbl> StockTbls { get; set; }
        public DbSet<TyresTbl> TyresTbls { get; set; }
        public DbSet<CustomerTbl> CustomerTbls { get; set; }
        public DbSet<OrdersTbl> OrdersTbls { get; set; }
        public DbSet<OrderDetailsTbl> OrderDetailsTbls { get; set; }
        public DbSet<ShippingTbl> ShippingTbls { get; set; }
        public DbSet<Drivers> Drivers { get; set; }
        public DbSet<PayFastShipping> PayFastShippings { get; set; }
        public DbSet<HistoryTbl> HistoryTbls { get; set; }
        public DbSet<QuoteCart> QuoteCarts { get; set; }
        public DbSet<QuotationTbl> QuotationTbls { get; set; }
        public DbSet<QuotationDetailTbl> QuotationDetailTbls { get; set; }


        public DbSet<VehicleTbl> VehicleTbls { get; set; }
        public DbSet<BookingTbl> BookingTbls { get; set; }
        public DbSet<CustomerVehicleTbl> CustomerVehicleTbls { get; set; }
        public DbSet<ScheduleTbl> ScheduleTbls { get; set; }
        public DbSet<MechanicTbl> MechanicTbls { get; set; }
        public DbSet<BayTbl> BayTbls { get; set; }
        public DbSet<ReportCart> ReportCarts { get; set; }
        public DbSet<ReportTbl> ReportTbls { get; set; }
        public DbSet<ReportDetailTbl> ReportDetailTbls { get; set; }
        public DbSet<InvoiceTbl> InvoiceTbls { get; set; }


        //Temp Table in DB
        public DbSet<ReportQuoteTempTbl> ReportQuoteTempTbls { get; set; }


        //Request Assistance Aspect
        public DbSet<RequestAssistanceTbl> RequestAssistanceTbls { get; set; }
        public DbSet<CalloutTbl> CalloutTbls { get; set; }
        public DbSet<ToolBoxTbl> ToolBoxTbls { get; set; }
        public DbSet<ToolsTbl> ToolsTbls { get; set; }
        public DbSet<ToolBoxToolDetail> ToolBoxToolDetails { get; set; }
        public DbSet<ToolsCheckOut> ToolsCheckOuts { get; set; }
        public DbSet<CalloutServices> CalloutServices { get; set; }
        public DbSet<CalloutReport> CalloutReports { get; set; }
        public DbSet<CalloutReportCart> CalloutReportCarts { get; set; }
        public DbSet<CalloutReportDetailTbl> CalloutReportDetailTbls { get; set; }
       
        public DbSet<InvoiceCalloutTbl> InvoiceCalloutTbls { get; set; }
        public DbSet<ToolsCheckInCart> ToolsCheckInCarts { get; set; }
        public DbSet<ToolBoxCheckInHistory> ToolBoxCheckInHistories { get; set; }

        //Cashup

        public DbSet<CashUpHistory> CashUpHistories { get; set; }

    }
}