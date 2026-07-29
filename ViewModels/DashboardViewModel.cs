using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestaurantHR_App.Data;
using RestaurantHR_App.Models;
using RestaurantHR_App.Services;

namespace RestaurantHR_App.ViewModels
{
    public class DashboardViewModel
    {
        private readonly AppDbContext _context;
        private readonly ExportService _exportService;

        public int TotalEmployees { get; set; }
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }
        public ObservableCollection<Attendance> FilteredAttendances { get; set; }

        public DashboardViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
            _exportService = new ExportService();
            FilteredAttendances = new ObservableCollection<Attendance>();

            SeedInitialData();
            LoadStats();
            FilterData("Today"); // الفلتر الافتراضي عند فتح البرنامج
        }

        private void SeedInitialData()
        {
            if (!_context.Employees.Any())
            {
                var emp1 = new Employee { Name = "أحمد محمود", Position = "شيف رئيسي", Branch = "الفرع الرئيسي", BasicSalary = 1200 };
                var emp2 = new Employee { Name = "سارّة خالد", Position = "مديرة صالة", Branch = "الفرع الرئيسي", BasicSalary = 1000 };
                _context.Employees.AddRange(emp1, emp2);
                _context.SaveChanges();

                var today = DateTime.Today;
                _context.Attendances.AddRange(
                    new Attendance { EmployeeId = emp1.Id, Date = today, CheckInTime = new TimeSpan(9, 0, 0), CheckOutTime = new TimeSpan(17, 0, 0), OvertimeHours = 1 },
                    new Attendance { EmployeeId = emp2.Id, Date = today, CheckInTime = new TimeSpan(9, 15, 0), CheckOutTime = new TimeSpan(17, 0, 0), OvertimeHours = 0 },
                    new Attendance { EmployeeId = emp1.Id, Date = today.AddDays(-1), CheckInTime = new TimeSpan(9, 0, 0), CheckOutTime = new TimeSpan(17, 0, 0), OvertimeHours = 0 }
                );
                _context.SaveChanges();
            }
        }

        public void LoadStats()
        {
            TotalEmployees = _context.Employees.Count(e => e.IsActive);
            var today = DateTime.Today;
            PresentToday = _context.Attendances.Count(a => a.Date == today && !a.IsAbsent);
            AbsentToday = _context.Attendances.Count(a => a.Date == today && a.IsAbsent);
        }

        public void FilterData(string filterType, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Attendances.Include(a => a.Employee).AsQueryable();
            DateTime today = DateTime.Today;

            if (filterType == "Today")
                query = query.Where(a => a.Date == today);
            else if (filterType == "Last7Days")
                query = query.Where(a => a.Date >= today.AddDays(-7));
            else if (filterType == "Last30Days")
                query = query.Where(a => a.Date >= today.AddDays(-30));
            else if (filterType == "Custom" && startDate.HasValue && endDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value && a.Date <= endDate.Value);

            FilteredAttendances.Clear();
            foreach (var item in query.ToList())
            {
                FilteredAttendances.Add(item);
            }
        }

        public void ExportToExcel(string path)
        {
            _exportService.ExportAttendancesToExcel(FilteredAttendances, path);
        }
    }
}
