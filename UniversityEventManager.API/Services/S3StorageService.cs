using Amazon.S3;
using Amazon.S3.Model;

namespace UniversityEventManager.API.Services
{
    public class S3StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;

        public S3StorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _configuration = configuration;
        }

        public async Task<(string PosterUrl, string PosterS3Key)> UploadEventPosterAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException("Poster file is required.");
            }

            var allowedContentTypes = new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

            if (!allowedContentTypes.Contains(file.ContentType))
            {
                throw new InvalidOperationException("Only JPG, PNG, and WEBP images are allowed.");
            }

            var bucketName = _configuration["AWS:S3:BucketName"];
            var region = _configuration["AWS:S3:Region"] ?? "ap-southeast-1";

            if (string.IsNullOrWhiteSpace(bucketName))
            {
                throw new InvalidOperationException("S3 bucket name is missing.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var key = $"event-posters/{Guid.NewGuid()}{extension}";

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType
            };

            await _s3Client.PutObjectAsync(request);

            var encodedKey = string.Join("/", key.Split('/').Select(Uri.EscapeDataString));
            var posterUrl = $"https://{bucketName}.s3.{region}.amazonaws.com/{encodedKey}";

            return (posterUrl, key);
        }
    }
}