using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorUI.Service.Models
{
    public partial class TorqueQAContext : DbContext
    {
        private string _connectionString { get; set; }

        public TorqueQAContext(ApplicationOptions options)
        {
            _connectionString = options.DealerOnConnectionString;
        }

        public TorqueQAContext(DbContextOptions<TorqueQAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Event { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Gonna regret this later when a credential sneaks into one of our commits. ☉ ‿ ⚆
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings. 
#warning LOL shut up microsoft. -j
#warning https://www.youtube.com/watch?v=lSipXuqHt40 it's murder -a
                optionsBuilder.UseSqlServer("we all know what this is...");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Position);

                entity.ToTable("Event", "inventory");

                entity.Property(e => e.Json)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CauseNavigation)
                    .WithMany(p => p.InverseCauseNavigation)
                    .HasForeignKey(d => d.Cause);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
