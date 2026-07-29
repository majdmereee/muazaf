using Microsoft.EntityFrameworkCore;
using RestaurantHR_App.Models;

namespace RestaurantHR_App.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // إنشاء قاعدة بيانات SQLite في مجلد التطبيق
            optionsBuilder.UseSqlite("Data Source=RestaurantHR.db");
        }
    }
}
