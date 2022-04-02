namespace CuriousReadersService.Services.Image;

public interface IImageService
{
    Task<string> UploadFileBlobAsync(string blobContainerName, Stream content, string fileName, string? oldFileName);
}
