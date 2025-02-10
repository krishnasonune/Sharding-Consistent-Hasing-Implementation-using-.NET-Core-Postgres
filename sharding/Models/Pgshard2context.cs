using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace sharding.Models;

public partial class Pgshard2Context : DbContext
{
    public Pgshard2Context(DbContextOptions<Pgshard2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<UrlShortner> UrlShortners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlShortner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("url_shortner_pkey");

            entity.ToTable("url_shortner");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.UrlId)
                .HasMaxLength(5)
                .HasColumnName("url_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
