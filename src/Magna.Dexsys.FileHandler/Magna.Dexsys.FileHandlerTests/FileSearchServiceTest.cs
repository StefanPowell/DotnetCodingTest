using Magna.Dexsys.FileHandler;
using Magna.Dexsys.FileHandler.Services;
using System.Text;
using Xunit;

namespace Magna.Dexsys.FileHandlerTests
{
    public class FileSearchServiceTest
    {
        private readonly string _testDirectory = Path.Combine(Directory.GetCurrentDirectory(), FileHandler.Common.Constants.UnitTestFolderLocation);

        [Fact]
        public async Task LocateFilesContainingSearchValue_FilesFound()
        {
            // Arrange
            string searchValue = "Magna";
            string firstFile = Path.Combine(_testDirectory, "file1");
            string secondFile = Path.Combine(_testDirectory, "file2");
            string thirdFile = Path.Combine(_testDirectory, "file3");
            string fourthFile = Path.Combine(_testDirectory, "file4");

            await File.WriteAllTextAsync(firstFile, "Magna \n Test \n Apple", Encoding.Unicode);
            await File.WriteAllTextAsync(secondFile, "Angam \n Mag \n Test", Encoding.Unicode);
            await File.WriteAllTextAsync(thirdFile, "Yes \n Test \n Magn", Encoding.Unicode);
            await File.WriteAllTextAsync(fourthFile, "Angam \n Apple \n Magna", Encoding.Unicode);
            var searchService = new FileSearchService();

            // Act
            int foundCount = await searchService.LocateFilesContainingSearchValue(_testDirectory, searchValue);

            // Assert
            //omly file one and four should return values
            Assert.Equal(2, foundCount);
            Assert.Contains(searchService.FilesLocated, file => file.Name == "file1");
            Assert.Contains(searchService.FilesLocated, file => file.Name == "file4");

            //Removing files after usage, ensure no problems caused with future tests
            File.Delete(firstFile);
            File.Delete(secondFile);
            File.Delete(thirdFile);
            File.Delete(fourthFile);
        }
    }
}
