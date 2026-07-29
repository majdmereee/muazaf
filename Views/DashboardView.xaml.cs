using System;
using System.Windows;
using Microsoft.Win32;
using RestaurantHR_App.ViewModels;

namespace RestaurantHR_App.Views
{
    public partial class DashboardView : Window
    {
        private readonly DashboardViewModel _vm;

        public DashboardView()
        {
            InitializeComponent();
            _vm = new DashboardViewModel();
            DataContext = _vm;
        }

        private void FilterToday_Click(object sender, RoutedEventArgs e)
        {
            _vm.FilterData("Today");
        }

        private void Filter7Days_Click(object sender, RoutedEventArgs e)
        {
            _vm.FilterData("Last7Days");
        }

        private void Filter30Days_Click(object sender, RoutedEventArgs e)
        {
            _vm.FilterData("Last30Days");
        }

        private void FilterCustom_Click(object sender, RoutedEventArgs e)
        {
            _vm.FilterData("Custom", DpStart.SelectedDate, DpEnd.SelectedDate);
        }

        private void OpenAddAttendance_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("نافذة إضافة تسجيل دوام سريع.", "إضافة حضور جديد", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "تقرير_الدوام_المطعم.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                _vm.ExportToExcel(dialog.FileName);
                MessageBox.Show("تم تصدير التقرير بنجاح!", "تم التصدير", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
