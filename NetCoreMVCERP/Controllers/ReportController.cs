using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using MvcReportViewer;
using NetCoreMVCERP.Models;
using NetCoreMVCERP.Models.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NetCoreMVCERP.Controllers
{
    public class ReportController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        IEnumerable<Employee> employees; //查詢員工資料
        IEnumerable<CList> list; //查詢員工資料
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
            var path = $"{_webHostEnvironment.WebRootPath}\\reports\\Report11.rdlc";

            //匯入變數資料
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("rp1", "Employee Data");
            //parameters.Add("rp2", "0");
            //parameters.Add("rp3", "False");

            //parameters.Add("rpL1", "aa");
            AspNetCore.Reporting.LocalReport local = new AspNetCore.Reporting.LocalReport(path);

         local.AddDataSource("DataSet1", employees);
            list = new List<CList>() { new CList() {Title="編號" } , new CList() { Title = "名稱" } , new CList() { Title = "薪水" }};
            local.AddDataSource("DataSet2", list);
            // local.AddDataSource

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = local.Execute(RenderType.Pdf, extension, parameters, mintype);
            
            //return result;
            return File(result.MainStream, "application/pdf");
        }

        public IActionResult Excel() {

            var dt = new DataTable();

            string mintype = "10";
            int extension = 0;

            //報表模板位置 NetCoreMVCERP\wwwroot\Reports\Report1.rdlc
            var path = $"{_webHostEnvironment.WebRootPath}\\reports\\Report11.rdlc";

            //匯入變數資料
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rp1", "Employee Data");
            parameters.Add("rp2", "0");
            parameters.Add("rp3", "False");
            AspNetCore.Reporting.LocalReport local = new AspNetCore.Reporting.LocalReport(path);
            local.AddDataSource("DataSet1", employees);
            // local.AddDataSource

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = local.Execute(RenderType.Excel, extension, parameters, mintype);

            //return result;
            //return File(result.MainStream, "application/vnd.ms-excel", "Export.xlsx");
            return File(result.MainStream, "application/msexcel", "Export.xlsx");
            //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        }



        //public IActionResult PrintNetCore() {
        //    LocalReport report = new LocalReport();
        //    report.LoadReportDefinition($"{_webHostEnvironment.WebRootPath}\\reports\\Report1.rdlc");
        //    report.DataSources.Add(new ReportDataSource("source", dataSource));
        //    report.SetParameters(new[] { new ReportParameter("Parameter1", "Parameter value") });
        //    byte[] pdf = report.Render("PDF");

        //}

        //public IActionResult PrintHtml() {

        //    //ReportViewer reportViewer = new ReportViewer();
        //    //reportViewer.ProcessingMode = ProcessingMode.Local;
        //    //reportViewer.LocalReport.ReportPath = $"{_webHostEnvironment.WebRootPath}\\reports\\Report1.rdlc";
        //    ////reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", employees));
        //    //return View(reportViewer);

        //    var model = GetReportViewerModel(Request);
        //    model.ReportPath = "/Folder/Report File Name";
        //    model.AddParameter("Parameter1", namedParameter1);
        //    model.AddParameter("Parameter2", namedParameter2);

        //    return View("ReportViewer", model);
        //}
    }
}
