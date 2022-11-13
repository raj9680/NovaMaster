using Imm.DAL.Data.Table;
using Microsoft.EntityFrameworkCore;

namespace Imm.DAL.Data
{
    public class NovaDbContext:DbContext
    {
        public NovaDbContext(DbContextOptions<NovaDbContext> options)
            : base(options)
        { 
            /// Nothing to do now
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// Unique column
            modelBuilder.Entity<AspNetUsers>()
            .HasIndex(p => new { p.Email }).IsUnique();
            
            /// Seeding Country
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 1, CountryName = "India" }, 
                new Country { CountryId = 2, CountryName = "United States" });
            
            /// Seeding State
            modelBuilder.Entity<State>().HasData(
                new { StateId = 1, CountryId = 1, StateName = "Punjab" },
                new { StateId = 2, CountryId = 1, StateName = "Uttar Pradesh" },
                new { StateId = 3, CountryId = 2, StateName = "NY" });

            /// Seeding State
            modelBuilder.Entity<AspNetUsers>().HasData(
                new AspNetUsers { UserId=1, FullName="Raj Aryan", Email="admin@admin.com", Password="admin@admin.com", CnfEmail=false, IsActive=false });

            /// Seeding State
            modelBuilder.Entity<AspNetRoles>().HasData(
                new AspNetRoles { UserId = 1, Role = "admin" });

            /// Seeding City
            modelBuilder.Entity<City>().HasData(
            new { CityId=3, StateId = 1, CityName = "Amritsar" });
        }

        /// Registering tables
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<AspNetUsersInfo> AspNetUsersInfo { get; set; }
        public DbSet<AspNetStudentsInfo> AspNetStudentsInfo { get; set; }
        public DbSet<AspNetUsersDocs> AspNetUserDocs { get; set; }
        public DbSet<AspNetUsersManager> AspNetUsersManager { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
    }
}
