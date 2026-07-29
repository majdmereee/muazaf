using ClosedXML.Excel;
using RestaurantHR_App.Models;
using System.Collections.Generic;
using System.IO;

namespace RestaurantHR_App.Services
{
    public class ExportService
    {
        public void ExportAttendancesToExcel(IEnumerable<Attendance> attendances, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("تقرير الدوام");

                // إضافة العناوين (Headers)
                worksheet.Cell(1, 1).Value = "التاريخ";
                worksheet.Cell(1, 2).Value = "رقم الموظف";
                worksheet.Cell(1, 3).Value = "وقت الدخول";
                worksheet.Cell(1, 4).Value = "وقت الخروج";
                worksheet.Cell(1, 5).Value = "الساعات الإضافية";

                // تنسيق العناوين
                var headerRange = worksheet.Range("A1:E1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                // تعبئة البيانات
                int row = 2;
                foreach (var record in attendances)
                {
                    worksheet.Cell(row, 1).Value = record.Date.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 2).Value = record.EmployeeId;
                    worksheet.Cell(row, 3).Value = record.CheckInTime.ToString();
                    worksheet.Cell(row, 4).Value = record.CheckOutTime?.ToString() ?? "-";
                    worksheet.Cell(row, 5).Value = record.OvertimeHours;
                    row++;
                }

                // تعديل عرض الأعمدة تلقائياً لتناسب المحتوى
                worksheet.Columns().AdjustToContents();

                // حفظ الملف
                workbook.SaveAs(filePath);
            }
        }
    }
}
