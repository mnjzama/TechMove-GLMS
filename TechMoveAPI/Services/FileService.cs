using Microsoft.AspNetCore.Http;

namespace TechMoveAPI.Services
{
    public class FileService
    {
        private readonly string _uploadRoot;

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.getcurrentdirectory
        Date: [n.d]
        Date Accessed: 19 April 2026
        */
        public FileService()
        {
            // Set the root directory for file uploads to a "wwwroot/uploads/contracts" folder within the application
            _uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/contracts");
        }

        /*
        Author: Code Maze
        URL: https://code-maze.com/aspnetcore-validate-uploaded-file/#:~:text=Here%20are%20some%20examples%20of%20file%2Dtype%20validation:,the%20same%20name%20exists%20in%20the%20system.      
        Date: 10 February 2024
        Date Accessed: 19 April 2026
        */
        public bool IsValidPdf(IFormFile file)
        {
            // Validate that the uploaded file is a non-empty PDF by checking its extension and content type
            if (file == null || file.Length == 0)
                return false;

            // Check file extension and MIME type for PDF
            var extension = Path.GetExtension(file.FileName).ToLower();

            // A valid PDF file must have a .pdf extension and the correct MIME type
            return extension == ".pdf" && file.ContentType == "application/pdf";
        }

        /*
        Author: Yogeshkumar Hadiya
        URL: https://www.c-sharpcorner.com/article/upload-single-or-multiple-files-in-asp-net-core-using-iformfile2/
        Date: 14 February 2022
        Date Accessed: 19 April 2026
        */
        public string SaveFile(IFormFile file)
        {
            // Ensure the upload directory exists before saving the file
            if (!Directory.Exists(_uploadRoot))
                Directory.CreateDirectory(_uploadRoot);

            // Generate a unique file name using a GUID to prevent collisions and preserve the original file extension
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            // Save the file to the specified path and return the relative URL for accessing the uploaded file
            var fullPath = Path.Combine(_uploadRoot, fileName);

            // Save the file to the server
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Return the relative path to the saved file for use in the application
            return "/uploads/contracts/" + fileName;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.delete?view=net-10.0
        Date: [n.d]
        Date Accessed: 19 April 2026
        */
        public void DeleteFile(string relativePath)
        {
            // Delete the file at the specified relative path if it exists
            if (string.IsNullOrEmpty(relativePath))
                return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot",relativePath.TrimStart('/'));

            // Check if the file exists before attempting to delete it to avoid exceptions
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else
            {
                // File does not exist at the specified path, no action required
                // This prevents unnecessary exceptions or console logging in production
            }
        }
    }
}