using System;
using System.Linq;
using System.Windows;
using RestaurantHR_App.Data;
using RestaurantHR_App.Models;

namespace RestaurantHR_App.Views
{
    public partial class AddAttendanceWindow : Window
    {
        private readonly AppDbContext _context;

        public AddAttendanceWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadEmployees();
            DpDate.SelectedDate = DateTime.Today; // تعيين تاريخ اليوم كافتراضي
        }

        private void LoadEmployees()
        {
            // جلب الموظفين من قاعدة البيانات لعرضهم في القائمة المنسدلة
            CmbEmployees.ItemsSource = _context.Employees.Where(e => e.IsActive).ToList();
            if (CmbEmployees.Items.Count > 0)
                CmbEmployees.SelectedIndex = 0;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbEmployees.SelectedValue == null)
                {
                    MessageBox.Show("الرجاء اختيار الموظف أولاً.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // إنشاء سجل دوام جديد
                var newAttendance = new Attendance
                {
                    EmployeeId = (int)CmbEmployees.SelectedValue,
                    Date = DpDate.SelectedDate ?? DateTime.Today,
                    CheckInTime = TimeSpan.Parse(TxtCheckIn.Text),
                    CheckOutTime = TimeSpan.Parse(TxtCheckOut.Text),
                    OvertimeHours = 0, // يمكنك إضافة عملية حسابية هنا مستقبلاً
                    IsAbsent = false
                };

                // الحفظ الفعلي في قاعدة البيانات
                _context.Attendances.Add(newAttendance);
                _context.SaveChanges();

                MessageBox.Show("تم حفظ الدوام بنجاح!", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                
                this.DialogResult = true; // إخبار الشاشة الرئيسية بنجاح العملية لتحديث الجدول
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"تأكد من إدخال الوقت بصيغة صحيحة (مثال: 09:30)\nالخطأ: {ex.Message}", "خطأ في الإدخال", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
