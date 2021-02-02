using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetCoreMVCERP.Models;
using NetCoreMVCERP.Models.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCERP.Controllers
{
    public class ReportController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        IEnumerable<Employee> employees; //查詢員工資料
        public ReportController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            employees = EmployeeData.Employees;
        }

        public IActionResult Index()
        {
            return View(employees);
        }

        /// <summary>
        /// 列印報表
        /// </summary>
        /// <returns></returns>
        public IActionResult Print() {

            var dt = new DataTable();

            string mintype = "10";
            int extension = 0;

            //報表模板位置 NetCoreMVCERP\wwwroot\Reports\Report1.rdlc
            var path = $"{_webHostEnvironment.WebRootPath}\\reports\\Report1.rdlc";

            //匯入變數資料
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rp1", "Employee Data");
            parameters.Add("rp2", "0");
            LocalReport local = new LocalReport(path);
            local.AddDataSource("DataSet1", employees);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = local.Execute(RenderType.Pdf, extension, parameters, mintype);
            
            //return result;
            return File(result.MainStream, "application/pdf");
        }
    }
}
