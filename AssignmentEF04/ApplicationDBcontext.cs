using AssignmentEF04.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentEF04
{
    public class ApplicationDBcontext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=BankDB;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Branch>()
                .HasKey(b => b.Code);

           
            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountNumber);

            
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Manager)
                .WithOne(m => m.Branch)
                .HasForeignKey<Manager>(m => m.BranchCode);

            
            modelBuilder.Entity<Branch>()
                .HasMany(b => b.Accounts)
                .WithOne(a => a.Branch)
                .HasForeignKey(a => a.BranchCode);

            
            modelBuilder.Entity<CustomerAccount>()
                .HasKey(ca => new { ca.CustomerId, ca.AccountNumber });

            modelBuilder.Entity<CustomerAccount>()
                .HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId);

            modelBuilder.Entity<CustomerAccount>()
                .HasOne(ca => ca.Account)
                .WithMany(a => a.CustomerAccounts)
                .HasForeignKey(ca => ca.AccountNumber);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountNumber);

   
            modelBuilder.Entity<Branch>().HasData(
                new Branch { Code = "B001", Name = "Cairo Branch", Address = "Cairo", PhoneNumber = "01000000000" }
            );

            modelBuilder.Entity<Manager>().HasData(
                new Manager { Id = 1, FullName = "Ahmed Ali", Email = "ahmed@test.com", PhoneNumber = "01111111111", HireDate = DateTime.Now, BranchCode = "B001" }
            );
        }

    }
}
