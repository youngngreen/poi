using Microsoft.EntityFrameworkCore;
using PartnerPOI.API.Models;

namespace PartnerPOI.API.Data;

public class PartnerPOIContext : DbContext
{
    public PartnerPOIContext(DbContextOptions options)
        : base(options) { }

    public DbSet<PartnerType> PartnerType { get; set; }
    public DbSet<TbMInternalPointH> TbMInternalPointH { get; set; }
    public DbSet<TbMInternalPointD> TbMInternalPointD { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder
        //    .Entity<PartnerType>().HasKey(b => b.Id);

        modelBuilder.Entity<PartnerType>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
        });

        //modelBuilder.Entity<InternalPoint>().HasKey(b => b.internalPointSettingID);
        modelBuilder.Entity<TbMInternalPointH>(entity =>
        {
            entity.Property(e => e.InternalPointSettingID).HasColumnName("internalPointSettingID");
        });

        //modelBuilder.Entity<TbMInternalPointD>(entity =>
        //{
        //    entity.Property(e => e.InternalPointSettingID).HasColumnName("internalPointSettingID");
        //});

        modelBuilder
            .Entity<TbMInternalPointD>().HasKey(b => new { b.InternalPointSettingID, b.SeqNo});


    }
}