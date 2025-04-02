using Magna.Dexsys.FileHandler.Interface;
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

        //Create Interface for Searching Files - This will allows for mocking and Unit Testing Later
        IFileSearchService fileSearchService = new FileSearchService();

        for (; testCount < testLimit; testCount++)
        {
            Test(fileSearchService, _searchValue);
        }

        Console.ReadLine();
    }

    private async static void Test(IFileSearchService service, string searchValue)
    {
        //Using the async, not sure if Concurrent implememation is correct, or if this should have been done with a Parallelism approach
        //vs a Concurrent approach, went concurrent (believe to speed up process, possibly by using multiple threads in file access)
        //so do understand that there may be a better approach to speeding up the process i.e. Parallelism vs Concurrency

        Stopwatch stopwatch = Stopwatch.StartNew();
        await service.LocateFilesContainingSearchValue(_fileLocation, searchValue);
        stopwatch.Stop();


        Console.WriteLine($"Search Value : { _searchValue}");
        foreach (FileDetails item in service.FilesLocated)
        {
            Console.WriteLine($"File Name: {item.Name}\nFile Content: \n{item.Content}");
        }

        Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds}\n");
    }
}
