using System;

namespace RestaurantHR_App.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public decimal OvertimeHours { get; set; }
        public bool IsAbsent { get; set; }
    }
}
