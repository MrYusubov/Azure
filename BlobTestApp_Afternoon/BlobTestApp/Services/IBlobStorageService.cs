namespace BlobTestApp.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadAsync(string fileName);
        Task<List<string>> ListFilesAsync();
    }
}
