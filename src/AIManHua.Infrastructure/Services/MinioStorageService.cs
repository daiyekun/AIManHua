using Minio;

namespace AIManHua.Infrastructure.Services;

public class MinioStorageService
{
    private readonly IMinioClient _client;

    public MinioStorageService(IMinioClient client)
    {
        _client = client;
    }
}
