using Core.Domain.Shared.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Domain.Shared.Contacts
{
    public interface IFileManagementRepository
    {
        Task<Response<string>> UploadFile(IFormFile file, string directory);
    }
}
