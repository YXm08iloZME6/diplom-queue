using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IFileStorageService
{
    Task<string?> SaveFileAsync(IFormFile? file, string path);
    Task DeleteFileAsync(string? path);
}
