using Magna.Dexsys.FileHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magna.Dexsys.FileHandler.Interface
{
    public interface IFileSearchService
    {
        Task<int> LocateFilesContainingSearchValue(string directory, string searchValue);
        IReadOnlyList<FileDetails> FilesLocated { get; }
    }
}
