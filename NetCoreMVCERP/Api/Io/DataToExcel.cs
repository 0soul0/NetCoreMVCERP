using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCoreMVCERP.Api.Io
{
    public class DataToExcel
    {

        public const string TYPE_STRING = "string";
        public const string TYPE_DOUBLE = "double";
        public const string TYPE_INT32 = "int32";
        public const string TYPE_DATETIMEOFFSET = "datetimeoffset";
        public const string TYPE_DECIMAL = "decimal";
        public const string DATA_FORMAT_STRING = "@"; //字串
        /// <summary>
        ///  list 轉成excel
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="values">轉換資料</param>
        /// <param name="fileTitle">檔案標題</param>
        /// <param name="subTitle">副檔案標題</param>
        /// <returns></returns>
        public static IActionResult ToExcel(Controller controller, List<List<string[]>> values, string fileTitle, string subTitle)
        {
            fileTitle = String.IsNullOrEmpty(fileTitle) ? "file" : fileTitle;
            subTitle = String.IsNullOrEmpty(subTitle) ? "sub" : subTitle;

            //設定excel資料
            using (var workbook = new XLWorkbook())
            {
                //設定分頁標頭
                var worksheet = workbook.Worksheets.Add(String.IsNullOrEmpty(subTitle) ? "default" : subTitle);
                var row = 1;//設定第一行
                values.ForEach(lists =>
                {
                    int col = 1;//列
                    lists.ForEach(val =>
                    {

                        //設定資料格式
                        worksheet.Cell(row, col).Style.NumberFormat.Format = val[1];
                        //設定欄位資料
                        worksheet.Cell(row, col).SetValue(val[0]);

                        col++;
                    });
                    row++;
                });

                //輸出excel
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    //return content;
                    return controller.File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       $"{fileTitle}.xlsx"
                        );

                }
            }
        }


        /// <summary>
        /// 把model資料轉成excel需要的資料格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">model資料</param>
        /// <param name="titleList">excel標頭</param>
        /// <param name="showColumn">要顯示的欄位</param>
        /// <returns></returns>
        public static List<List<string[]>> DataToExcelDataType<T>(IEnumerable<T> data, List<List<string[]>> titleList, string[] showColumn)
        {

            List<List<string[]>> output = new List<List<string[]>>();

            //excel標題
            if (titleList != null)
            {
                output = titleList;
            }

            try
            {   //解析model資料並填成字串方面匯出excel
                Boolean titleCheck = true; //標題只添加一次
                foreach (T item in data)
                {

                    //table 標題
                    List<string[]> label = new List<string[]>();
                    //table第乙行
                    List<string[]> col = new List<string[]>();

                    //獲取model欄位
                    PropertyInfo[] listPI = item.GetType().GetProperties();

                    foreach (PropertyInfo pi in listPI)
                    {
                        string title = pi.Name;
                        if (!showColumn.Contains(pi.Name))
                        {
                            continue;
                        }
                        //標題只添加一次
                        if (titleCheck)
                        {
                            //添加欄位標題
                            //var display = pi.CustomAttributes.FirstOrDefault();
                            var attri = (DisplayAttribute)pi.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                            if (attri != null)
                            {
                                title = attri.Name;
                            }
                            label.Add(new string[] { title, DATA_FORMAT_STRING });
                        }

                        //數值集合
                        string[] valueSet = new string[2];
                        //欄位資料
                        var value = pi.GetValue(item, null);
                        if (value != null)
                        {
                            valueSet[0] = value.ToString();

                            string type = pi.PropertyType.Name.ToString().ToLower();
                            switch (type)
                            {
                                case TYPE_DATETIMEOFFSET://時間
                                    valueSet[1] = "yyyy-mm-dd";
                                    break;
                                case TYPE_DECIMAL: //貨幣
                                    valueSet[1] = "#,##0.00";
                                    break;
                                default: //預設字串
                                    valueSet[1] = DATA_FORMAT_STRING;
                                    break;

                            }
                        }
                        col.Add(valueSet);
                    }
                    if (titleCheck)
                    {
                        output.Add(label);
                    }
                    output.Add(col);
                    titleCheck = false;
                }
            }
            catch (Exception e)
            {
                return new List<List<string[]>>();
            }


            return output;
        }


    }
}
