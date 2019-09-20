using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using archives.service.biz.web;
using archives.service.biz.exp;
using NPOI.SS.UserModel;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.gateway.Controllers
{
    [Authorize]
    public class DAController : BaseController
    {
        [Authorize]
        public IActionResult Manage()
        {
            return View(getUser());
        }

        public IActionResult Upload()
        {
            return View();
        }

        static readonly string FiledirectoryPath = AppContext.BaseDirectory + "/files/";

        /// <summary>
        /// 1. 先批量上传文件
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> UploadFile()
        {
            var result = new CommonResponse<List<string>>() { Data = new List<string>() };
            try
            {
                foreach (IFormFile item in Request.Form.Files)
                {
                    if (!Directory.Exists(FiledirectoryPath))
                    {
                        Directory.CreateDirectory(FiledirectoryPath);
                    }
                    var id = Guid.NewGuid().ToString();
                    var suffix = item.FileName.Substring(item.FileName.LastIndexOf(".") + 1);
                    string filePath = FiledirectoryPath + Guid.NewGuid().ToString() + "."+ suffix;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    result.Data.Add(id);//将上传文件名返回，前端再根据这个提交之后读取数据。
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = "上传发生异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 2. 然后批量读取文件内容，提交到数据库
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ConfirmUpload([FromBody]List<string> fileList)
        {
            var result = new CommonResponse<List<string>>();
            try
            {
                for (int i = 0; i < fileList.Count; i++)
                {
                    var fileName = FiledirectoryPath + fileList[i];
                    if (!System.IO.File.Exists(fileName))
                    {
                        throw new BizException($"第{i + 1}个上传文件不存在，请确认");
                    }
                    //根据指定路径读取文件
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        //根据文件流创建excel数据结构
                        IWorkbook workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        if (sheet == null)
                            throw new BizException($"第{i + 1}个上传文件不不存在sheet，请确认");

                        for (int j = 1; j <= sheet.LastRowNum; ++j)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row == null) continue; //没有数据的行默认是null　　　　　　　

                            //for (int z = row.FirstCellNum; z < row.LastCellNum; ++z)
                            //{
                            //    if (row.GetCell(z) != null) //同理，没有数据的单元格都默认是null
                            //    {
                            //        //获取数据
                            //    }
                            //}
                        }
                    }
                }
                
            }
            catch(BizException ex)
            {
                result.Message = ex.Message;
            }
            catch(Exception ex)
            {
                result.Message = "确认提交发生异常";
            }
            return Json(result);
        }
    }
}
