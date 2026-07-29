using System;
using System.Linq;
using System.Collections.ObjectModel;
using RestaurantHR_App.Data;
using RestaurantHR_App.Models;

namespace RestaurantHR_App.ViewModels
{
    public class DashboardViewModel
    {
        private readonly AppDbContext _context;

        // الخصائص المرتبطة بالواجهة
        public int TotalEmployees { get; set; }
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }
        public ObservableCollection<Attendance> FilteredAttendances { get; set; }

        public DashboardViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // إنشاء القاعدة إذا لم تكن موجودة
            FilteredAttendances = new ObservableCollection<Attendance>();
            LoadDailyStats();
        }

        // إحصائيات اليوم السريعة
        public void LoadDailyStats()
        {
            TotalEmployees = _context.Employees.Count(e => e.IsActive);
            var today = DateTime.Today;
            
            PresentToday = _context.Attendances.Count(a => a.Date == today && !a.IsAbsent);
            AbsentToday = _context.Attendances.Count(a => a.Date == today && a.IsAbsent);
        }

        // دالة الفلترة الذكية (حسب طلبك)
        public void FilterData(string filterType, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Attendances.AsQueryable();
            DateTime targetDate = DateTime.Today;

            switch (filterType)
            {
                case "Last7Days":
                    query = query.Where(a => a.Date >= targetDate.AddDays(-7) && a.Date <= targetDate);
                    break;
                case "Last30Days":
                    query = query.Where(a => a.Date >= targetDate.AddDays(-30) && a.Date <= targetDate);
                    break;
                case "Custom":
                    if (startDate.HasValue && endDate.HasValue)
                        query = query.Where(a => a.Date >= startDate.Value && a.Date <= endDate.Value);
                    break;
            }

            var results = query.ToList();
            FilteredAttendances.Clear();
            foreach (var item in results)
            {
                FilteredAttendances.Add(item);
            }
        }
    }
}
