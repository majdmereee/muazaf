using System;
using System.Collections.Generic;

namespace RestaurantHR_App.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; } // المسمى الوظيفي
        public string Branch { get; set; } // الفرع
        public decimal BasicSalary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        
        // علاقة الموظف بسجل الحضور
        public ICollection<Attendance> Attendances { get; set; }
    }
}
