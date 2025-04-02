using Magna.Dexsys.FileHandler.Models;
using Magna.Dexsys.FileHandler.Services;
using System.Diagnostics;

namespace Magna.Dexsys.FileHandler.Cli;

public class Program
{
    //I'd rather not hard code these values, call Constant class instead, allows to be retrievable across the application
    private const string _fileLocation = Common.Constants.FileLocation;
    private const string _searchValue = Common.Constants.SearchValue;

    public static void Main(string[] args)
    {
        int testCount = 0;
        int testLimit = 10;

        for (; testCount < testLimit; testCount++)
        {
            Test(_searchValue);
        }

        Console.ReadLine();
    }

    private async static void Test(string searchValue)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        FileSearchService searchService = new();
        await searchService.LocateFilesContainingSearchValue(_fileLocation, searchValue);
        stopwatch.Stop();
        Console.WriteLine($"Search Value : { _searchValue}");
        foreach (FileDetails item in searchService.FilesLocated)
        {
            Console.WriteLine($"File Name: {item.Name}\nFile Content: \n{item.Content}");
        }

        Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds}\n");
    }
}
