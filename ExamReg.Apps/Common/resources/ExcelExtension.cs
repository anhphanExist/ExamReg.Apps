using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Common
{
    public static class ExcelExtension
    {
        public static void GenerateWorksheet(this ExcelPackage excelPackage, string SheetName, List<string[]> headers, IEnumerable<object[]> data)
        {
            // thêm worksheet vào file excel
            var worksheet = excelPackage.Workbook.Worksheets.Add(SheetName);

            // đặt header
            SetHeader(worksheet, headers);

            // đặt data
            SetData(worksheet, headers, data);
        }
        private static void SetHeader(this ExcelWorksheet worksheet, List<string[]> headers)
        {
            // những dòng đầu tiên của sheet là của header
            int headerLine = headers.Count;

            // lấy độ dài của header
            string headerRange = $"A1:" + Char.ConvertFromUtf32(headers[0].Length + 64) + headerLine;

            // load header từ array vào cell
            worksheet.Cells[headerRange].LoadFromArrays(headers);

            // set font các thứ các thứ cho header
            worksheet.Cells[headerRange].Style.Font.Bold = true;
            worksheet.Cells[headerRange].Style.Font.Size = 12;
            worksheet.Cells[headerRange].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
            worksheet.Cells[headerRange].AutoFitColumns();
            worksheet.Cells[headerRange].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
        private static void SetData(this ExcelWorksheet worksheet, List<string[]> headers, IEnumerable<object[]> data)
        {
            // dòng đầu tiên của data là ngay sau header
            int startFromLine = headers.Count + 1;
            // lấy range của header
            var headerRange = $"A{startFromLine}:" + Char.ConvertFromUtf32(headers[0].Length + 64) + startFromLine;
            worksheet.Cells[headerRange].LoadFromArrays(data);
        }
    }
}
