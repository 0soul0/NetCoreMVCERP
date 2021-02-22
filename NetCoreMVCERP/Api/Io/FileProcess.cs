using ClosedXML.Excel;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreMVCERP.Api.Io
{   

    /// <summary>
    /// 處理檔案(上傳驗證等等)
    /// </summary>
    public class FileProcess
    {
        //public static string DATA_TYPE_NUMBER = "0";
        //public static string DATA_TYPE_TEXT = "@";
      
        //最大上傳大小2M
        private static long FILE_SIZE_LIMEIT = 1024 * 1024 * 2;
        //資料夾檔案最大數量
        private static int DIRECTORY_SIZE_LIMEIT = 30000;
        //存放檔案資料名稱
        private static string DIRECTORY_NAME = "files";

        /// <summary>
        /// 驗證檔案副檔名合法
        /// </summary>
        /// <param name="uploadedFileName">上傳檔案名稱</param>
        /// <returns>true:成功 false:失敗</returns>
        public static bool IsFileExtensionValidation(string uploadedFileName) {
            var ext = Path.GetExtension(uploadedFileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !GetMimeTypes().ContainsKey(ext))
            {
                // The extension is invalid ... discontinue processing the file
                return false;
            }
            return true;
        }

        /// <summary>
        /// 驗證上傳資料是否大於2M
        /// </summary>
        /// <param name="fileSize">資料大小</param>
        /// <returns>true:成功 false:失敗</returns>
        public static bool IsSizeValidation(long fileSize) {
            if (fileSize > FILE_SIZE_LIMEIT)
            {
                // The file is too large ... discontinue processing the file
                return false;
            }
            return true;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="file">檔案</param>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="env">路徑資料</param>
        /// <param name="controller">控制器</param>
        /// <param name="CompancyGuid">公司guid</param>
        /// <param name="isCheck">是否要檢查檔案是否合法</param>
        /// <returns>ServerName=CConstants.File.FILE_EXTENSION_IllEGAL:驗證檔案副檔名不合法 
        /// ServerName=CConstants.File.FILE_SIZE_TOO_BIG:驗證上傳資料大於2M 
        /// ServerName=CConstants.SUCCESS:成功</returns>
        public static async Task<string> UploadFileAndCheckAsync( IFormFile file,string fileName, IHostEnvironment env,Controller controller,bool isCheck) {

            //檢查輸入值
            //設定路徑
            //判斷現有資料表是否超過3萬筆
            //驗證檔案副檔名合法
            //驗證上傳資料是否大於2M
            //檢查資料夾是否存在 不存在新建一個
            //上傳資料庫資料
            //上傳檔案

            //檢查輸入值
            if (file == null || String.IsNullOrEmpty(fileName) || env == null || controller == null) return null;

            int fileAmount;//檔案數量
            int flowNumber = 1;//資料夾流水號

            //設定路徑
            //上傳當檔案路徑

            string routeLocal = env.ContentRootPath + "/wwwroot/";
            string routePrev = DIRECTORY_NAME + "/" + controller.RouteData.Values["controller"] + "/";
            string routeDate = DateTime.Now.Year.ToString();
            string route = routeLocal+ routePrev + routeDate+"/"+ flowNumber;

            //遍立檔案 計算一個資料夾是否超過N筆
            while (true) {
                if (!Directory.Exists(route)) {
                    break;
                }

                fileAmount = Directory.GetFiles(route).Length;//檔案數量
                //判斷現有資料表是否超過3萬筆
                if (fileAmount < DIRECTORY_SIZE_LIMEIT)
                {
                    break;
                }
                //檢查下一個檔案
                flowNumber++;
                route = routeLocal+routePrev + routeDate + "/" + flowNumber;
            }
            
            //檢查檔案是否合法
            if (isCheck) {
                //驗證檔案副檔名合法
                if (!IsFileExtensionValidation(file.FileName)) return "Data extension is illegal";

                //驗證上傳資料是否大於2M
                if (!IsSizeValidation(file.Length)) return "Data size is over than 2M";
            }

            //檢查資料夾是否存在 不存在新建一個
            if (!Directory.Exists(route))
            {
                //新增資料夾
                Directory.CreateDirectory(route);
            }

            ////上傳資料庫資料

            //GlaAccountDocumentAttachment attachment = new GlaAccountDocumentAttachment() {
            //    //ServerName = fileName,
            //    Route = routePrev + routeDate + "/" + flowNumber + "/" + Path.GetFileName(fileName)
            //};

            using (var fileStrem = new FileStream(Path.Combine(route, $"{Path.GetFileNameWithoutExtension(fileName)}{Path.GetExtension(file.FileName)}"), FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStrem);
            }

            return "success";
        }

        /// 下載檔案
        /// </summary>
        /// <param name="fileUrl">檔案url</param>
        /// <returns></returns>
        public static async Task<IActionResult> DownloadFile(Controller controller,IHostEnvironment env,string fileUrl)
        {
            //方案一  方法返回值：IActionResult,FileResult,FileStreamResult
            var path = env.ContentRootPath + "/wwwroot/" + fileUrl;
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return controller.File(memory, GetMimeTypes()[ext], Path.GetFileName(path));

        }

        /// <summary>
        ///  根據副檔名取得檔案類型
        /// </summary>
        /// <returns>檔案類型</returns>
        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt","text/plain" },
                {".pdf","application/pdf" },
                {".doc","application/vnd.ms-word" },
                {".docx","application/vnd.ms-word" },
                {".xls","application/vnd.ms-excel" },
                {".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                {".png","image/png" },
                {".jpg","image/jpg" },
                {".jpeg","image/jpeg" },
                {".gif","image/gif" },
                {".csv","text/csv" },
            };
        }

    }
}
