using Microsoft.EntityFrameworkCore;
using BangazonAPI.Models;


//Written By: Pair Programmed as a team. 
namespace BangazonAPI.Data
{
    //APIContext class that inherits from the DbContext class that is a part of the Entity Framework
    public class BangazonAPIContext : DbContext
    {
        public BangazonAPIContext(DbContextOptions<BangazonAPIContext> options)
            : base(options)
        { }
        // Creates tables in the database; takes our models and turns them into tables in the database. Any models without a DbSet do not turn into tables in the database.
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<TrainingProgram> TrainingProgram { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Computer> Computer { get; set; }
        public DbSet<ProductOrder> ProductOrder {get; set;}


        //Sets the DateCreated on the <Customer> to todays date every time a new instance of Customer is added to the Context. 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // If a model has a date field that should be generated by the database
            modelBuilder.Entity<Customer>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d')");
            modelBuilder.Entity<Customer>()
                .Property(p => p.DateLastInteraction)
                .HasDefaultValueSql("strftime('%Y-%m-%d')");
            modelBuilder.Entity<Order>()
                .Property(c => c.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d')");
                
        }

    }
}