using Microsoft.AspNetCore.Http;

namespace IshTap.Business.HelperServices.Interfaces;

public interface IFileService
{
    Task<string> CopyFileAsync(IFormFile file, string wwwroot, params string[] folders);
    Task<string> CopyDocumentAsync(IFormFile file, string wwwroot, params string[] folders);
}
