using Exam_Project.Api.Entities;
using Exam_Project.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Data
{
    public class ClientDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public ClientDbContext(DbContextOptions<ClientDbContext> options)
            : base(options)
        {
            this.Database.SetCommandTimeout(new TimeSpan(0, 0, 30, 0));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region ___TABLES_RELATIONSHIPS___
            #region USER TABLE
            modelBuilder.Entity<User>()
                .HasMany(m => m.Sessions)
                .WithOne(o => o.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .Property(s => s.DateCreated)
                .HasDefaultValueSql("getutcdate()");

            #endregion
            #region Session
            modelBuilder.Entity<Sessions>()
                .Property(s => s.DateCreated)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Sessions>()
                .HasOne(s => s.User).WithMany()
                .HasForeignKey(u => u.UserId);
            #endregion
            #region ORDER
            modelBuilder.Entity<Order>()
                    .HasOne(a => a.Customer).WithMany()
                    .HasForeignKey(s => s.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                    .Property(s => s.DateCreated)
                    .HasDefaultValueSql("getutcdate()");
            #endregion
            #endregion
        }
    }

}
