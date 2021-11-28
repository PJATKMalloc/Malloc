using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Malloc.Data.AutocompleteModel
{
    public partial class nominatimContext : DbContext
    {
        public nominatimContext()
        {
        }

        public nominatimContext(DbContextOptions<nominatimContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Place> Places { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=ConnectionStrings:AutocompleteConn");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("hstore")
                .HasPostgresExtension("postgis");

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("place");

                entity.HasIndex(e => new { e.OsmId, e.OsmType, e.Class, e.Type }, "idx_place_osm_unique")
                    .IsUnique();

                entity.HasIndex(e => new { e.OsmType, e.OsmId }, "place_id_idx");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.AdminLevel).HasColumnName("admin_level");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasColumnName("class");

                entity.Property(e => e.Extratags).HasColumnName("extratags");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.OsmId).HasColumnName("osm_id");

                entity.Property(e => e.OsmType)
                    .HasMaxLength(1)
                    .HasColumnName("osm_type");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type");
            });

            modelBuilder.HasSequence("file");

            modelBuilder.HasSequence("seq_place").StartsAt(100000);

            modelBuilder.HasSequence("seq_word");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
