using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

        public ReportController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列印報表
        /// </summary>
        /// <returns></returns>
        public IActionResult Print() {


            string mintype = "";
            int extension = 1;
            //查詢員工資料
            var data = EmployeeData.Employees;
            //報表模板位置 NetCoreMVCERP\wwwroot\Reports\Report1.rdlc
            var path = $"{_webHostEnvironment.WebRootPath}\\reports\\Report1.rdlc";
           
            //匯入變數資料
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rp1", "Employee Data");
           
            LocalReport local = new LocalReport(path);
            local.AddDataSource("DataSet1", data);

            var result = local.Execute(RenderType.Pdf, extension, parameters, mintype);
            return File(result.MainStream, "application/pdf");
        }
    }
}
