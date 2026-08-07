using HelpDesk.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>().HasData(
                new Ticket
                {
                    Id = 1,
                    Title = "Software License Request",
                    Description = "Requesting license key for Visual Studio Enterprise edition",
                    Priority = "High",
                    Status = "Open",
                    RaisedBy = "John Doe",
                    CreatedDate = new DateTime(2026, 8, 1, 10, 0, 0)
                },
                new Ticket
                {
                    Id = 2,
                    Title = "VPN Connectivity Issue",
                    Description = "Unable to connect to company VPN network from home",
                    Priority = "Medium",
                    Status = "In Progress",
                    RaisedBy = "Jane Smith",
                    CreatedDate = new DateTime(2026, 8, 2, 11, 30, 0)
                },
                new Ticket
                {
                    Id = 3,
                    Title = "Monitor Replacement",
                    Description = "Flickering display on primary monitor",
                    Priority = "Low",
                    Status = "Closed",
                    RaisedBy = "Alice Johnson",
                    CreatedDate = new DateTime(2026, 8, 3, 14, 15, 0)
                }
            );
        }
    }
}
