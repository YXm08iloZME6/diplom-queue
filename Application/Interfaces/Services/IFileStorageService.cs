using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IFileStorageService
{
    Task<string?> SaveFileAsync(IFormFile? file);
    Task DeleteFileAsync(string? path);
}
