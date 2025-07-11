using Microsoft.EntityFrameworkCore;
using PersonaKey.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.DataAccessLayer.Context
{
    public class PersonaKeyContext : DbContext
    {
        // Constructor — will get DbContextOptions from outside
        public PersonaKeyContext(DbContextOptions<PersonaKeyContext> options) : base(options)
        {
        }

        public DbSet<AccessLog> AccessLogs { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<RoleAccess> RoleAccesses { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Role ↔ RoleAccess (One-to-One ilişki)
            modelBuilder.Entity<Role>()
                .HasOne(r => r.RoleAccess)
                .WithOne(ra => ra.Role)
                .HasForeignKey<RoleAccess>(ra => ra.RoleId)
                .OnDelete(DeleteBehavior.Cascade); // Gerekirse değiştirilebilir
        }
    }
}
