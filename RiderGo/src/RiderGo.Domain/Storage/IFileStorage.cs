namespace RiderGo.Domain.Storage
{
    public interface IFileStorage
    {
        Task<string> UploadAsync(string fileName, byte[] fileBase64);
    }
}
