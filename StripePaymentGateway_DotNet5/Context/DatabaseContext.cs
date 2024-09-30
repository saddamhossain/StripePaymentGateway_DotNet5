using Microsoft.EntityFrameworkCore;
using StripePaymentGateway_DotNet5.Models;

namespace StripePaymentGateway_DotNet5.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {
        }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<User> Users { get; set; }

    }
}
