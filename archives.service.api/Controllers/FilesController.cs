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
    /// <summary>
    /// 文件上传下载接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class FilesController : Controller
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly string _localPath;
        private readonly string _gatewayFilePath;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStorageService"></param>
        /// <param name="configuration"></param>
        public FilesController(IFileStorageService fileStorageService, IConfiguration configuration)
        {
            _fileStorageService = fileStorageService;
            _localPath = configuration.GetValue<string>("FileLocalPath");
            _gatewayFilePath = configuration.GetValue<string>("GatewayFilePath");
        }

        /// <summary>
        /// 上传接口
        /// </summary>
        /// <param name="file">file name=file</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<AddFileResult>> AddFile(IFormFile file)
        {
            var response = new CommonResponse<AddFileResult>();

            try
            {
                var id = Guid.NewGuid().ToString("N");
                var suffix = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                var filePath = $"{_localPath}{id}.{suffix}";//注意formFile.FileName包含上传文件的文件路径，所以要进行Substring只取出最后的文件名

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var entity = new dal.Entity.FileStorage
                {
                    Id = id,
                    ContentType = file.ContentType,
                    CreateTime = DateTime.Now,
                    StoragePath = filePath,
                    StorageType = dal.Entity.FileStorageType.Local,
                    Size = file.Length,
                    AccessUrl = $"{_gatewayFilePath}?f={id}"
                };
                await _fileStorageService.AddFile(entity);
                response.Data = new AddFileResult
                {
                    Id = id,
                    AccessUrl = entity.AccessUrl,
                    Size = file.Length
                };
                response.Success = true;
            }
            catch
            {
                response.Message = "上传发生异常";
            }
            return response;

        }

        [HttpPost]
        public async Task<CommonResponse<List<AddFileResult>>> BatchAddFile()
        {
            var response = new CommonResponse<List<AddFileResult>>();

            try
            {
                var listFile = new List<dal.Entity.FileStorage>();
                foreach (var file in Request.Form.Files)
                {
                    var id = Guid.NewGuid().ToString("N");
                    var suffix = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                    var filePath = $"{_localPath}{id}.{suffix}";//注意formFile.FileName包含上传文件的文件路径，所以要进行Substring只取出最后的文件名

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    listFile.Add(new dal.Entity.FileStorage
                    {
                        Id = id,
                        ContentType = file.ContentType,
                        CreateTime = DateTime.Now,
                        StoragePath = filePath,
                        StorageType = dal.Entity.FileStorageType.Local,
                        Size = file.Length,
                        AccessUrl = $"{_gatewayFilePath}?f={id}"
                    });
                }
                var list = await _fileStorageService.AddRangeFile(listFile);
                response.Data = list.Select(c => new AddFileResult { Id = c}).ToList();
                response.Success = true;
            }
            catch
            {
                response.Message = "上传发生异常";
            }
            return response;
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [HttpGet]
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

        [HttpPost]
        public async Task<CommonResponse<List<string>>> ConfirmUpload([FromBody]ConfirmUploadRequest request)
        {
            var response = new CommonResponse<List<string>>();
            try
            {
                response.Data = await _fileStorageService.ConfirmUpload(request.FileIds);
                response.Success = !response.Data.Any();
            }
            catch(BizException ex)
            {
                response.Message = ex.Message;
            }
            catch
            {
                response.Message = "上传发生异常";
            }
            return response;
        }
    }
}
