using Core.Domain.Shared.Contacts;
using Core.Domain.Shared.Wrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Repositories
{
    public class FileManagementRepository : IFileManagementRepository
    {
        private readonly IHostingEnvironment _webHostEnvironment;
        private readonly ILogger<FileManagementRepository> _logger;

        public FileManagementRepository(IHostingEnvironment webHostEnvironment, ILogger<FileManagementRepository> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<Response<string>> UploadFile(IFormFile file, string directory)
        {
            string filePath;
            try
            {
                Directory.CreateDirectory(directory);
                var fileName = file.FileName.Replace(" ", "_");
                fileName = fileName + Guid.NewGuid();
                fileName = fileName.Replace("/", "_");
                fileName = fileName.Replace(":", "_");
                filePath = directory + "/" + fileName;
                var filePathS = filePath.Replace("/", "\\");
                var physicalPath = _webHostEnvironment.WebRootPath + filePathS;
                await using var stream = new FileStream(physicalPath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to upload file!");
                return Response<string>.Fail("File upload failed");
            }
            return Response<string>.Success(filePath, "File has been uploaded successful");
        }

        public Response<bool> DeleteFile(string path)
        {
            string filePath;
            try
            {
                filePath = Path.Combine(_webHostEnvironment.WebRootPath, path);
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete file!");
                return Response<bool>.Fail("File delete failed");
            }
            return Response<bool>.Success(true, "File has been deleted successful");
        }
    }
}
