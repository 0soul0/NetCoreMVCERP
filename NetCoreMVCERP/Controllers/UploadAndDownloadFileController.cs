using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NetCoreMVCERP.Api.Io;
using NetCoreMVCERP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace NetCoreMVCERP.Controllers
{
    /// <summary>
    /// 上傳下載檔案控制器
    /// </summary>
    public class UploadAndDownloadFileController : Controller
    {
        private IHostEnvironment _env;
        private List<FileData> files;
        private HttpWebResponse response;

        public UploadAndDownloadFileController(IHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            files = new List<FileData>();

            DirSearch($"{_env.ContentRootPath}/wwwroot/files");

            return View(files);
        }

        /// <summary>
        /// select all of file under Directory that name is sDir
        /// </summary>
        /// <param name="sDir">name of Directory</param>
        public void DirSearch(string sDir)
        {
            try
            {
                //先找出所有目錄 
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    //先針對目前目路的檔案做處理 
                    foreach (string f in Directory.GetFiles(d))
                    {
                        var path = f.Replace("\\", "/");
                        files.Add(new FileData() { 
                            Route = path.Substring($"{_env.ContentRootPath}/wwwroot/".Length),
                            Type = Path.GetExtension(path)
                        });
                    }
                    //此目錄處理完再針對每個子目錄做處理 
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        /// <summary>
        /// upload file
        /// </summary>
        /// <param name="file">file</param>
        /// <returns></returns>
        public async Task<string> UploadFile(IFormFile file)
        {
            return await FileProcess.UploadFileAndCheckAsync(file, $"fileName{DateTime.Now.Millisecond}", _env, this, false);
        }

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<IActionResult> DownloadFile(string path) {
            return await FileProcess.DownloadFile(this,_env,path);
        }
    }
}
