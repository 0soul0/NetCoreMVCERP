﻿using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using MvcReportViewer;
using NetCoreMVCERP.Api.Io;
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
        public IActionResult Print()
        {

            var dt = new DataTable();

            string mintype = "10";
            int extension = 0;

            //報表模板位置 NetCoreMVCERP\wwwroot\Reports\Report1.rdlc
            var path = $"{_webHostEnvironment.WebRootPath}\\reports\\Report11.rdlc";

            //匯入變數資料
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rp1", "公司");
            parameters.Add("rp2", "員工資料");
            parameters.Add("t1", "編號");
            parameters.Add("t2", "名稱");
            parameters.Add("t3", "年資");
            parameters.Add("t4", "薪水");
            AspNetCore.Reporting.LocalReport local = new AspNetCore.Reporting.LocalReport(path);

            local.AddDataSource("DataSet1", employees);


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var result = local.Execute(RenderType.Pdf, extension, parameters, mintype);

            //return result;
            return File(result.MainStream, "application/pdf");
        }

        public IActionResult Excel()
        {
            List<List<string[]>> title = new List<List<string[]>>();
            title.Add(new List<string[]>() { new string[2] {"XX公司","@"} });
            title.Add(new List<string[]>() { new string[2] { "員工", "@" } });
            title.Add(new List<string[]>() { 
                new string[2] { "部門", "@" },
            new string[2] { "人事部", "@" }});

            var dto=DataToExcel.DataToExcelDataType(employees, title, new string[]
            { nameof(Employee.Id),
                nameof(Employee.Name),
                nameof(Employee.Salary),
                nameof(Employee.Seniority),
                nameof(Employee.Currency)});

            return DataToExcel.ToExcel(this, dto, "XX公司-員工表", "員工表");
        }

    }
}
