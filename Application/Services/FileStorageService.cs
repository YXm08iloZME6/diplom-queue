using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class FileStorageService : IFileStorageService
{
    public async Task<string?> SaveFileAsync(IFormFile? file, string path)
    {
        if (file == null || file.Length == 0)
            return null;

        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", path);
        Directory.CreateDirectory(folder);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(folder, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{path}/{fileName}";
    }

    public async Task DeleteFileAsync(string? filePath)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        await Task.CompletedTask;
    }
}
