namespace CuriousReadersService.Services.Image;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class ImageService : IImageService
{
    private readonly BlobServiceClient blobServiceClient;

    public ImageService(BlobServiceClient blobServiceClient)
    {
        this.blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileBlobAsync(string blobContainerName, Stream content, string fileName, string? oldFilename = "")
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

        var blobClient = containerClient.GetBlobClient(fileName);

        if (!string.IsNullOrEmpty(oldFilename))
        {
            await blobClient.DeleteIfExistsAsync();
        }

        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = fileName });
        return blobClient.Uri.ToString();
    }
}
