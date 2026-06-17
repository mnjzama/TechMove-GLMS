using Xunit;
using TechMoveAPI.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

/*
Author: Ivan Stulskij
URL: https://medium.com/@ivanstulskij/unit-testing-in-c-using-xun-585f4ed9fec7
Date: 11 November 2023
Date Accessed: 21 April 2026

Author: Microsoft Learn
URL: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-xunit
Date: [n.d]
Date Accessed: 21 April 2026

Author: Shubhadeep Chattopadhyay
URL: https://medium.com/@shubhadeepchat/unit-testing-in-net-core-6-0-web-api-using-xunit-83a5eb30d79b
Date: 18 July 2022
Date Accessed: 21 April 2026

Author: Gustavo Lage
URL: https://medium.com/@gustavolage/using-xunit-net-moq-and-inmemorydatabase-for-more-efficient-unit-testing-9763949dec2f
Date: 12 May 2020
Date Accessed: 21 April 2026

Author: Jeremy Wells
URL: https://medium.com/swlh/testing-an-asp-net-core-service-with-xunit-f18225d9b22a
Date: 3 August 2020
Date Accessed: 21 April 2026

Author: Arpit Shrivastava
URL: https://www.c-sharpcorner.com/article/in-memory-databases-unit-testing-with-c-sharp-efcore-and-xunit/
Date: [n.d]
Date Accessed: 21 April 2026

Author: Gibin Francis
URL: https://gibinfrancis.medium.com/unit-test-quick-book-c-net-core-moq-xunit-f89370af84b8
Date: 3 February 2023
Date Accessed: 21 April 2026

Author: Robert
URL: https://www.darchuk.net/2019/03/29/asp-net-core-unit-testing-a-file-upload/
Date: 29 March 2019
Date Accessed: 21 April 2026
*/

namespace ProjectTests.Services
{
    public class FileServiceTests
    {
        private IFormFile CreateMockFile(string fileName, string contentType)
        {
            var content = "dummy file content";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        [Fact]
        public void IsValidPdf_ShouldReturnTrue_ForValidPdf()
        {
            // Arrange
            var service = new FileService();
            var file = CreateMockFile("test.pdf", "application/pdf");

            // Act
            var result = service.IsValidPdf(file);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidPdf_ShouldReturnFalse_ForWrongExtension()
        {
            // Arrange
            var service = new FileService();
            var file = CreateMockFile("test.exe", "application/pdf");

            // Act
            var result = service.IsValidPdf(file);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPdf_ShouldReturnFalse_ForNullFile()
        {
            // Arrange
            var service = new FileService();

            // Act
            var result = service.IsValidPdf(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPdf_ShouldReturnFalse_ForWrongContentType()
        {
            // Arrange
            var service = new FileService();
            var file = CreateMockFile("test.pdf", "application/octet-stream");

            // Act
            var result = service.IsValidPdf(file);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SaveFile_ShouldReturnValidUploadPath()
        {
            // Arrange
            var service = new FileService();
            var file = CreateMockFile("contract.pdf", "application/pdf");

            // Act
            var result = service.SaveFile(file);

            // Assert
            Assert.StartsWith("/uploads/contracts/", result);
            Assert.EndsWith(".pdf", result);
        }

        [Fact]
        public void SaveFile_ShouldGenerateUniqueFileNames()
        {
            // Arrange
            var service = new FileService();
            var file = CreateMockFile("contract.pdf", "application/pdf");

            // Act
            var result1 = service.SaveFile(file);
            var result2 = service.SaveFile(file);

            // Assert
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public void SaveFile_ShouldThrowException_WhenFileIsNull()
        {
            // Arrange
            var service = new FileService();

            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                service.SaveFile(null);
            });
        }

        [Fact]
        public void FileService_ShouldRejectInvalidFileBeforeSaving()
        {
            // Arrange
            var service = new FileService();
            var file = CreateMockFile("virus.exe", "application/octet-stream");

            // Act
            var isValid = service.IsValidPdf(file);

            // Assert
            Assert.False(isValid);
        }
    }
}