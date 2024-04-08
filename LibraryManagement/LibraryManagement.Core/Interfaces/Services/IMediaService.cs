using LibraryManagement.Core.Entities;

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
