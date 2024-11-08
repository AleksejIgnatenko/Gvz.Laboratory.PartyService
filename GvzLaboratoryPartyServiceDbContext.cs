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
            //configuration
        }
    }
}
