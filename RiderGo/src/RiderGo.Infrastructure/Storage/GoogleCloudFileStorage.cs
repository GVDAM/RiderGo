using Microsoft.Extensions.Logging;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using RiderGo.Domain.Storage;

namespace RiderGo.Infrastructure.Storage
{
    public class GoogleCloudFileStorage : IFileStorage
    {
        private readonly ILogger<GoogleCloudFileStorage> _logger;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private readonly string _uri;

        public GoogleCloudFileStorage(string bucketName, GoogleCredential credential, string uri, ILogger<GoogleCloudFileStorage> logger)
        {
            _storageClient = StorageClient.Create(credential);
            _bucketName = bucketName;
            _logger = logger;
            _uri = uri;
        }


        public async Task<string> UploadAsync(string fileName, byte[] fileBase64)
        {
            _logger.LogInformation("Iniciando upload do arquivo para o Google Cloud Storage. Bucket: {bucket}, FileName: {fileName}", _bucketName, fileName);
            using var fileStream = new MemoryStream(fileBase64);

            var response = await _storageClient.UploadObjectAsync(
                _bucketName, 
                fileName, 
                null, 
                fileStream);

            _logger.LogInformation("Upload do arquivo concluído com sucesso. Bucket: {bucket}, FileName: {fileName}", _bucketName, fileName);
            return $"{_uri}/{_bucketName}/{response.Name}";
        }
    }
}
