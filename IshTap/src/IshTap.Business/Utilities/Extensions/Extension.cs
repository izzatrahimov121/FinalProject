using Microsoft.AspNetCore.Http;

namespace IshTap.Business.Utilities.Extensions;

public static class Extension
{
    public static bool CheckFileFormat(this IFormFile file, string format)
    {
        return file.ContentType.Contains(format);
    }
    public static bool CheckFileSize(this IFormFile file, int size)
    {
        return file.Length / 1024 < size;
    }
}
