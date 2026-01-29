using Microsoft.EntityFrameworkCore;
using PetQueue.Api.Models; 

namespace PetQueue.Api.Data
{
    public class PetQueueDbContext : DbContext
    {
        public PetQueueDbContext(DbContextOptions<PetQueueDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DogType> DogTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId); 
                entity.Property(u => u.Username).IsRequired().HasMaxLength(150);
                entity.HasIndex(u => u.Username).IsUnique();
            });

            // DogType Configuration
            modelBuilder.Entity<DogType>(entity =>
            {
                entity.HasKey(dt => dt.TypeId); 
                entity.Property(dt => dt.TypeName).IsRequired().HasMaxLength(50);
                entity.Property(dt => dt.Price).HasColumnType("decimal(10,2)");
            });

            // Appointment Configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.AppointmentId); 
                entity.Property(a => a.FinalPrice).HasColumnType("decimal(10,2)");
                entity.Property(a => a.ScheduledTime).IsRequired();

                // Relationships
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Appointments)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.DogType)
                    .WithMany()
                    .HasForeignKey(a => a.TypeId) 
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}