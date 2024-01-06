using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CISStock.ConsignmentNote;

namespace CISStock
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null;
        public DbSet<Role> Roles { get; set; } = null;
        public DbSet<Invoice> Invoices { get; set; } = null;
        public DbSet<Supplier> Suppliers { get; set; } = null;
        public DbSet<Customer> Customers { get; set; } = null;
        public DbSet<Product> Products { get; set; } = null;


        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost; port=5432; Database=Stock; UserName=postgres; Password=admin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User admin = new User();
            admin.UserID = 1;
            admin.FirstName = "Илья";
            admin.Password = "admin";
            admin.UserName = "admin";
            
            Role roleAdmin = new Role();
            Role roleUser = new Role();
            roleAdmin.RoleID = 1;
            roleAdmin.RoleName = "admin";
            roleUser.RoleID = 2;
            roleUser.RoleName = "user";
            admin.RoleId = 1;

            modelBuilder.Entity<User>().HasData(admin);
            modelBuilder.Entity<Role>().HasData(roleAdmin);
            modelBuilder.Entity<Role>().HasData(roleUser);


        }

        [Table("users")] // Пользователи
        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("user_id")]
            public int UserID { get; set; }

            [Column("username")]
            public string UserName { get; set; }

            [Column("password")]
            public string Password { get; set; }

            [Column("first_name")]
            public string FirstName { get; set; }

            [Column("last_name")]
            public string LastName { get; set; }

            [ForeignKey("role")]
            public int RoleId { get; set; }
            public virtual Role Role { get; set; }

        }

        [Table("roles")] // Роли
        public class Role
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("role_id")]
            public int RoleID { get; set; }

            [Column("rolename")]
            public string RoleName { get; set; }
        }
    }
}
