using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorUI.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorUI.Service.Models
{
    public partial class TorqueQAContext : DbContext, ILegacyEventContext
    {
        public TorqueQAContext()
        {
        }

        public TorqueQAContext(DbContextOptions<TorqueQAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Event { get; set; }

        public async Task<List<Event>> GetEvents()
        {
            using (var context = new TorqueQAContext())
            {
                return await Event.Where(e => e.Position < 10).ToListAsync();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings. 
#warning LOL shut up microsoft. -j
                optionsBuilder.UseSqlServer("Data Source=SQL-AAG-P2.ashburn.edealertools.com;Initial Catalog=Torque;User Id=TorqueSvc;Password=EZGg4+hA;");
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
