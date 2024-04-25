using AngularCrud.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AngularCrud.data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Student { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=lbs; Integrated Security=True;TrustServerCertificate=True");

        }

    }

}
