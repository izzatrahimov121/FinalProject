using IshTap.Business.Exceptions;
using IshTap.Business.HelperServices.Interfaces;
using IshTap.Business.Utilities.Extensions;
using Microsoft.AspNetCore.Http;

namespace IshTap.Business.HelperServices.Implementations;

public class FileService : IFileService
{
    public async Task<string> CopyFileAsync(IFormFile file, string wwwroot, params string[] folders)
    {
        string filename = String.Empty;

        if (file != null)
        {
            if (!file.CheckFileFormat("image/"))
            {
                throw new IncorrectFileFormatException("Incorrect file format");
            }
            if (!file.CheckFileSize(100))
            {
                throw new IncorrectFileSizeException("Incorrect file size");
            }

            filename = Guid.NewGuid().ToString() + file.FileName;
            string resultPath = wwwroot;
            foreach (var folder in folders)
            {
                resultPath = Path.Combine(resultPath, folder);
            }
            resultPath = Path.Combine(resultPath, filename);
            using (FileStream stream = new FileStream(resultPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filename;
        }
        throw new Exception();
    }

    public async Task<string> CopyDocumentAsync(IFormFile file, string wwwroot, params string[] folders)
    {
        string filename = String.Empty;
        if (file != null)
        {
            if (file.CheckFileFormat("application/pdf") || file.CheckFileFormat("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            {
                if (!file.CheckFileSize(6020)) { throw new IncorrectFileSizeException("Incorrect file size"); }
                filename = Guid.NewGuid().ToString() + file.FileName;
                string resultPath = wwwroot;
                foreach (var folder in folders)
                {
                    resultPath = Path.Combine(resultPath, folder);
                }
                resultPath = Path.Combine(resultPath, filename);
                using (FileStream stream = new FileStream(resultPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return filename;
            }
            throw new IncorrectFileFormatException("Incorrect file format");
        }
        throw new Exception();
    }
}
