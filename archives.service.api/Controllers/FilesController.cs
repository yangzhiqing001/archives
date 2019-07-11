using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.exp;
using archives.service.biz.ifs;
using archives.service.biz.web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.service.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FilesController : Controller
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly string _localPath;
        private readonly string _gatewayFilePath;
        public FilesController(IFileStorageService fileStorageService, IConfiguration configuration)
        {
            _fileStorageService = fileStorageService;
            _localPath = configuration.GetValue<string>("FileLocalPath");
            _gatewayFilePath = configuration.GetValue<string>("GatewayFilePath");
        }

        [HttpPost]
        public async Task<CommonResponse<AddFileResult>> AddFile(IFormFile formFile)
        {
            var response = new CommonResponse<AddFileResult>();

            try
            {
                var id = Guid.NewGuid().ToString("N");
                var suffix = formFile.FileName.Substring(formFile.FileName.LastIndexOf(".") + 1);
                var filePath = $"{_localPath}{id}.{suffix}";//注意formFile.FileName包含上传文件的文件路径，所以要进行Substring只取出最后的文件名

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                var entity = new dal.Entity.FileStorage
                {
                    Id = id,
                    ContentType = formFile.ContentType,
                    CreateTime = DateTime.Now,
                    StoragePath = filePath,
                    StorageType = dal.Entity.FileStorageType.Local,
                    Size = formFile.Length,
                    AccessUrl = $"{_gatewayFilePath}?f={id}"
                };
                await _fileStorageService.AddFile(entity);
                response.Data = new AddFileResult
                {
                    Id = id,
                    AccessUrl = entity.AccessUrl,
                    Size = formFile.Length
                };
                response.Success = true;
            }
            catch
            {
                response.Message = "上传发生异常";
            }
            return response;

        }

        public async Task<IActionResult> DownLoad(string f)
        {
            try
            {
                var fileStorage = await _fileStorageService.Get(f);
                var stream = System.IO.File.OpenRead(fileStorage.StoragePath);
                return File(stream, fileStorage.ContentType);
            }
            catch(BizException ex)
            {
                return Json(ex.Message);//两个catch先暂时这么写吧，
            }
            catch(Exception ex)
            {
                var s = ex.Message;
                return Json("系统异常"+ex.ToString());
            }

        }
    }
}
