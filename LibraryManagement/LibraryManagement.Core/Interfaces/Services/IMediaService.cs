using LibraryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IMediaService
    {
        Result<List<Media>> GetAllMedia();
        Result<int> AddMedia(Media media);
        Result EditMedia(Media media);
        Result<List<Media>> GetMediaByType(Media type);
        Result<Media> GetMediaByID(int id);
        Result<List<Media>> GetArchive();
        Result ArchiveMedia(int id);

        Result<List<Media>> GetNonArchivedMedia(Media type);
        Result<Dictionary<string, int>> Top3CheckedOutItems();

    }
}
