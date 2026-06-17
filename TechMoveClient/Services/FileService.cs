using Microsoft.AspNetCore.Http;

namespace TechMoveClient.Services
{
    public class FileService
    {
        /*
        Author: Code Maze
        URL: https://code-maze.com/aspnetcore-validate-uploaded-file/#:~:text=Here%20are%20some%20examples%20of%20file%2Dtype%20validation:,the%20same%20name%20exists%20in%20the%20system.      
        Date: 10 February 2024
        Date Accessed: 19 April 2026
        */
        public bool IsValidPdf(IFormFile file)
        {
            // Check if file is null or empty to prevent processing invalid uploads
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLower();

            // Return true only if file has .pdf extension and correct MIME type to ensure it's a valid PDF
            return extension == ".pdf" && file.ContentType == "application/pdf";
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.delete?view=net-10.0
        Date: [n.d]
        Date Accessed: 19 April 2026
        */
        public void DeleteFile(string relativePath)
        {
            // Check if relative path is null or empty to prevent accidental deletion of root directory
            if (string.IsNullOrEmpty(relativePath))
                return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot",relativePath.TrimStart('/'));

            // Ensure the file exists before attempting deletion to avoid exceptions
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}