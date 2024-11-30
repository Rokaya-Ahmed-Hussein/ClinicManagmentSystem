using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data 
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your DbSets here
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define  foreign key relationships
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments)
                .OnDelete(DeleteBehavior.Restrict); 

           
            modelBuilder.Entity<Staff>()
                .Property(s => s.Salary)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DoctorSchedule>()
                .Property(ds => ds.Price)
                .HasColumnType("decimal(18,2)");
          
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.ApplicationUser)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.ApplicationUser)
                .WithOne()
                .HasForeignKey<Patient>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.ApplicationUser)
                .WithOne()
                .HasForeignKey<Admin>(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Clinic>()
                .HasOne(c => c.ApplicationUser)
                .WithOne()
                .HasForeignKey<Clinic>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction); 
        }

    }
}
