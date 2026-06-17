using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Supabase;
using TechMoveClient.Models;

/*
Author: Supabase Documentation
URL: https://supabase.com/docs/reference/csharp
Date: [n.d]
Date Accessed: 15 May 2026
*/
namespace TechMoveClient.Services
{
    public class SupabaseFileService
    {
        private readonly Supabase.Client _client;
        private readonly string _bucket;

        public SupabaseFileService(IOptions<SupabaseSettings> config)
        {
            var settings = config.Value;

            _bucket = settings.Bucket;

            _client = new Supabase.Client(settings.Url,settings.Key);

            _client.InitializeAsync().Wait();
        }

        /*
        Author: Supabase Documentation
        URL: https://supabase.com/docs/guides/storage
        Date: [n.d]
        Date Accessed: 15 May 2026
        */

        public async Task<string> UploadContractAsync(IFormFile file)
        {
            // Generate unique file name to avoid collisions in storage bucket
            var fileName = $"{Guid.NewGuid()}.pdf";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();

            // Upload file to Supabase storage bucket
            await _client.Storage
                .From(_bucket)
                .Upload(fileBytes, fileName);

            // Return publicly accessible URL for uploaded file
            return _client.Storage
                .From(_bucket)
                .GetPublicUrl(fileName);
        }

        /*
        Author: Supabase Documentation
        URL: https://supabase.com/docs/reference/javascript/storage-remove
        Date: [n.d]
        Date Accessed: 15 May 2026
        */

        public async Task DeleteContractAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return;

            try
            {
                // Extract file name from full public URL
                var fileName = Path.GetFileName(new Uri(fileUrl).AbsolutePath);

                // Remove file from Supabase storage bucket
                await _client.Storage
                    .From(_bucket)
                    .Remove(new List<string> { fileName });
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting file from Supabase storage: " + ex.Message); 
            }
        }
    }
}