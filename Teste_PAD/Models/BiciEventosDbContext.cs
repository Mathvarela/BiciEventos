using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste_PAD.Models
{
    class BiciEventosDbContext : DbContext
    {
        public BiciEventosDbContext(DbContextOptions<BiciEventosDbContext> options):base(options)
        {
            
        }

        public BiciEventosDbContext()
        {
            
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Invite> Invites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=BiciEventos.db");
        }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasKey(u => u.Id);

        builder.Entity<Event>()
            .HasKey(e => e.Id);

        builder.Entity<Attendance>()
            .HasKey(a => new
            {
                a.EventId,
                a.UserId
            });

        builder.Entity<Invite>()
            .HasKey(i => new
            {
                i.EventId,
                i.InvitedId,
                i.InviterId
            });

        builder.Entity<Invite>()
            .HasOne(i => i.Invited)
            .WithMany(i => i.InvitesReceived)
            .HasForeignKey(i => i.InvitedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Invite>()
            .HasOne(i => i.Inviter)
            .WithMany(i => i.InvitesSent)
            .HasForeignKey(i => i.InviterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Invite>()
            .HasOne(i => i.Event)
            .WithMany(e => e.Invites)
            .HasForeignKey(i => i.EventId);

        builder.Entity<Event>()
            .HasOne(e => e.User)
            .WithMany(u => u.Events)
            .HasForeignKey(e => e.UserId);

        builder.Entity<Attendance>()
            .HasOne(a => a.Event)
            .WithMany(a => a.Attendances)
            .HasForeignKey(a => a.EventId);

        builder.Entity<Attendance>()
            .HasOne(a => a.User)
            .WithMany(a => a.Attendances)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    }
}
