using Gvz.Laboratory.PartyService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.PartyService
{
    public class GvzLaboratoryPartyServiceDbContext : DbContext
    {
        public DbSet<ManufacturerEntity> Manufacturers { get; set; }
        public DbSet<PartyEntity> Parties { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public GvzLaboratoryPartyServiceDbContext(DbContextOptions<GvzLaboratoryPartyServiceDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var user = new UserEntity
            {
                Id = Guid.Parse("CA6456C9-6062-481B-89B1-FF53E954A027"),
                Surname = "Admin",
                UserName = "Admin",
                Patronymic = "Admin",
            };

            modelBuilder.Entity<UserEntity>().HasData(user);
        }
    }
}
