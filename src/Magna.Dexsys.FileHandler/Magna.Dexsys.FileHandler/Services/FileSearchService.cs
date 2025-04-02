using Magna.Dexsys.FileHandler.Interface;
using Magna.Dexsys.FileHandler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Magna.Dexsys.FileHandler.Services;
public class FileSearchService : IFileSearchService
{
    public IReadOnlyList<FileDetails> FilesLocated => _filesLocated.AsReadOnly();
    private readonly List<FileDetails> _filesLocated = [];

    public FileSearchService()
    {
    }

    /// <summary>
    /// Populate the instance's FilesLocated member with a collection of
    /// files which contain the partialContent value anywhere in the 
    /// file.
    /// </summary>
    /// <param name="directory">Directory containing files to search</param>
    /// <param name="searchValue">Data to search for in files</param>
    /// <returns>Return the number of files located</returns>
    /// 
    public async Task<int> LocateFilesContainingSearchValue(string directory, string searchValue)
    {
        string[] fileEntries = Directory.GetFiles(directory);
        _filesLocated.Clear();

        var tasks = fileEntries.Select(async file =>
        {
            try
            {
              string fileContent = await System.IO.File.ReadAllTextAsync(file, Encoding.Unicode);
              
                if (fileContent.Contains(searchValue))
                {
                    var fileInfo = new FileInfo(file);
                    var fileDetails = new FileDetails(
                        fileInfo.FullName,
                        fileInfo.Name,
                        fileContent.Length,
                        fileContent
                    );

                    // Lock to prevent concurrent access - SIMILAR TO RACE CONDITION WE SPOKE ABOUT, SEEING THAT WE ARE DOING TASK ASYNCHRONOUSLY
                    lock (_filesLocated)
                    {
                        _filesLocated.Add(fileDetails);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }).ToList();

        await Task.WhenAll(tasks);
        return _filesLocated.Count;
    }

}
