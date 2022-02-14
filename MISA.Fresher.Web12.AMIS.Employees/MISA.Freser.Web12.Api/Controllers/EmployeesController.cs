using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Api.Controllers;
using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using System.Globalization;

namespace MISA.Fresher.Web12.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : MISABaseController<Employee>
    {
        #region Dependency Injection

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeServices _employeeServices;

        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeServices employeeServices) : base(employeeRepository, employeeServices)
        {
            _employeeRepository = employeeRepository;
            _employeeServices = employeeServices;
        }

        #endregion

        #region Support Export Excel

        // List of headers
        private readonly string[] theads = {"STT", "Mã nhân viên", "Tên nhân viên", "Giới tính", "Ngày sinh", "Số CMND", 
            "Chức danh", "Tên đơn vị", "Số tài khoản", "Tên ngân hàng", "Chi nhánh TK ngân hàng"};

        /// <summary>
        /// @author: Vũ Quang Phong (12/02/2022)
        /// @desc: Styling border of grid
        /// </summary>
        /// <param name="titleTable"></param>
        private static void StyleBorder(IXLRange titleTable)
        {
            titleTable.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            titleTable.Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
            titleTable.Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
            titleTable.Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
        }

        /// <summary>
        /// @author: Vũ Quang Phong (12/02/2022)
        /// @desc: Styling title of grid
        /// </summary>
        /// <param name="titleTable"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontName"></param>
        private static void StyleTitle(IXLRange titleTable, int fontSize, string fontName)
        {
            titleTable.Style.Font.Bold = true;
            titleTable.Style.Font.FontSize = fontSize;
            titleTable.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            titleTable.Style.Font.SetFontName(fontName);
        }

        /// <summary>
        /// @author: Vũ Quang Phong (12/02/2022)
        /// @desc: Set width of Columns
        /// </summary>
        /// <param name="worksheet"></param>
        private static void SetColumnWidth(IXLWorksheet worksheet)
        {
            worksheet.Column("A").Width = 4;
            worksheet.Column("B").Width = 15;
            worksheet.Column("C").Width = 28;
            worksheet.Column("D").Width = 9;
            worksheet.Column("E").Width = 13;
            worksheet.Column("F").Width = 19;
            worksheet.Column("G").Width = 25;
            worksheet.Column("H").Width = 25;
            worksheet.Column("I").Width = 19;
            worksheet.Column("J").Width = 25;
            worksheet.Column("K").Width = 31;
        }

        /// <summary>
        /// @author: Vũ Quang Phong (12/02/2022)
        /// @desc: Change DateTime --> dd/mm/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns>
        /// String ~ dd/mm/yyyy
        /// </returns>
        private static string FormatDate(DateTime date)
        {
            string dd = date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString();
            string mm = date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString();
            string yyyy = date.Year.ToString();
            return $"{dd}/{mm}/{yyyy}";
        }

        #endregion

        #region Main Controllers

        /// <summary>
        /// @method: GET /Employees/excel
        /// @desc: Export Employees Data into Excel file
        /// @author: Vũ Quang Phong (12/02/2022)
        /// </summary>
        /// <returns>
        /// The Excel file
        /// </returns>
        [HttpGet("excel")]
        public IActionResult ExportExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                // Create new sheet of the Excel file named "DANH SÁCH NHÂN VIÊN"
                var worksheet = workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");

                // Title styling
                var title = worksheet.Range("A1:K1");
                title.Value = "DANH SÁCH NHÂN VIÊN";
                title.Merge();
                StyleTitle(title, 16, "Arial");

                // Empty mid row
                worksheet.Range("A2:K2").Merge();

                // Headers grid styling
                var headersGrid = worksheet.Range("A3:K3");
                headersGrid.Style.Fill.BackgroundColor = XLColor.CoolGrey;
                StyleBorder(headersGrid);
                StyleTitle(headersGrid, 10, "Arial");
                foreach (var (header, i) in theads.Select((header, i) => (header, i)))
                {
                    worksheet.Cell(3, i + 1).Value = header;
                }

                // Put Employees data in the grid
                var employees = _employeeRepository.GetAllEmployees();
                int currentRow = 4;
                foreach (var (employee, index) in employees.Select((employee, index) => (employee, index)))
                {
                    string dateTemp = "";
                    if (employee.DateOfBirth != null) 
                    {
                        dateTemp = FormatDate((DateTime)employee.DateOfBirth);
                    }

                    worksheet.Cell(currentRow, 1).Value = index + 1;
                    worksheet.Cell(currentRow, 2).Value = employee.EmployeeCode;
                    worksheet.Cell(currentRow, 3).Value = employee.EmployeeName.ToUpper();
                    worksheet.Cell(currentRow, 4).Value = employee.Gender == Core.Enum.Gender.Male ? "Nam" : "Nữ";

                    worksheet.Cell(currentRow, 5).Value = dateTemp;

                    worksheet.Cell(currentRow, 6).Value = $"'{employee.IdentityNumber}";
                    worksheet.Cell(currentRow, 7).Value = employee.PositionEName;
                    worksheet.Cell(currentRow, 8).Value = employee.DepartmentName;
                    worksheet.Cell(currentRow, 9).Value = $"'{employee.BankAccountNumber}";
                    worksheet.Cell(currentRow, 10).Value = employee.BankName;
                    worksheet.Cell(currentRow, 11).Value = employee.BankBranchName;

                    currentRow++;
                }

                // Style range data
                var rangeData = worksheet.Range($"A4:K{currentRow - 1}");
                StyleBorder(rangeData); 
                rangeData.Style.Font.SetFontName("Times New Roman");

                // Set width of columns
                SetColumnWidth(worksheet);

                // Set align of data in A column (Sequence number)
                worksheet.Range($"A4:A{currentRow - 1}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                // Set align of data E column (Date of birth)
                worksheet.Range($"E4:E{currentRow - 1}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                using (var stream = new MemoryStream())
                {
                    // Save the file
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Return the file to client
                    return File(
                        content, 
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                        "Danh sách nhân viên.xlsx"
                    );

                }
            }

        }

        /// <summary>
        /// @method: GET /Employees/filter?employeeFilter=...
        /// @desc: Search for Employees by employeeFilter (code, name, phonenumber)
        /// Get Paging
        /// @author: Vũ Quang Phong (20/01/2022)
        /// @edited_by: Vũ Quang Phong (13/02/2022)
        /// </summary>
        /// <param name="employeeFilter"></param>
        /// <returns>
        /// An object contains the Number of Records & the Array of Employees
        /// </returns>
        [HttpGet("filter")]
        public IActionResult GetPaging(int? pageIndex, int? pageSize, string? employeeFilter)
        {
            try
            {
                var dataEmployees = _employeeServices.GetEmployeesPaging(pageIndex, pageSize, employeeFilter);

                return Ok(dataEmployees);
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }
        }

        #endregion Main Controllers
    }
}
