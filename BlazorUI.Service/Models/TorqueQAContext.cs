using BlazorUI.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Service.Models
{
    public partial class TorqueQAContext : DbContext, ILegacyEventContext
    {
        private readonly string _connection;

        public TorqueQAContext(string connection)
        {
            _connection = connection;
        }

        public TorqueQAContext(DbContextOptions<TorqueQAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TotemV1Event> Event { get; set; }

        public async Task<List<TotemV1Event>> GetEvents(int count = 0, int checkpoint = 0)
        {
            using (var context = new TorqueQAContext(_connection))
            {
                return await context.Event.Where(e => e.Position < 10).ToListAsync();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(_connection))
            {
                throw new NullReferenceException($"You need to run the following comand: {Environment.NewLine}" +
                    $"dotnet user-secrets set \"LegacyEvents:ConnectionString\" \"your-string-here\" {Environment.NewLine}" +
                    "Otherwise check the dependency injection in ApplicationServiceExtensions.");
            }
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move 9it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings. 
#warning LOL shut up microsoft. -j
#warning Somebody tell them it's murder https://www.youtube.com/watch?v=CGOt8dZRsHk -a
                optionsBuilder.UseSqlServer(_connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TotemV1Event>(entity =>
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
