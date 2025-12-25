using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Network.API.Infrastructure;
using Network.Core.Helpers;
using Network.Core.Interfaces;
using Network.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Service.Sys_File
{
    public class Service:RepositoryBase<Model.Sys_File>, Sys_File.IService
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserProvider _userProvider;
        public Service(DomainDbContext dbContext, IDateTimeProvider dateTimeProvider, IUserProvider userService):base(dbContext, dateTimeProvider, userService)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _userProvider = userService;
        }
        public async Task<List<FileResult>> Upload(List<IFormFile> files, string objectId, string objectType, string savedPath)
        {
            List<Model.Sys_File> saveFiles = new List<Model.Sys_File>();            
            foreach (var file in files)
            {                
                string FileName = file.ContentDisposition.Split("\"")[3];
                string Extension = Path.GetExtension(FileName);
                string path = Path.Combine(savedPath, FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                Model.Sys_File saveFile = new Model.Sys_File();
                saveFile.Name = FileName;
                saveFile.Extension = Extension;
                saveFile.Path = string.IsNullOrEmpty(path) ? path : path.Replace("\\", "/");
                saveFile.ObjectId = objectId;
                saveFile.ObjectType = objectType;
                saveFile.CreatedDateTime = _dateTimeProvider.OffsetNow;
                saveFile.CreatedBy = _userProvider.UserName;
                saveFiles.Add(saveFile);
            }
            await _dbContext.Sys_Files.AddRangeAsync(saveFiles);
            await UnitOfWork.SaveAsync();
            List<FileResult> filesresult = new List<FileResult>();            
            ObjectHelpers.Mapping<Model.Sys_File, FileResult>(saveFiles, filesresult);
            return filesresult;
        }
        public async Task<List<FileResult>> GetByObjectId(string objectId, string objectType)
        {
            List<Model.Sys_File> files = await _dbContext.Sys_Files.Where(o => o.ObjectId == objectId && o.ObjectType == objectType).ToListAsync();
            List<FileResult> fileResults = new List<FileResult>();
            FileResult fileResult = null;
            foreach (var file in files)
            {
                fileResult = new FileResult();
                ObjectHelpers.Mapping<Model.Sys_File, FileResult>(file, fileResult);
                fileResults.Add(fileResult);
            }
            return fileResults;
        }
    }
}
