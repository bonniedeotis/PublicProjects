using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Application.Repositories
{
    public interface IMediaRespository
    {
        List<Media> GetAll();
        int Add(Media media);
        void Edit(Media media);
        List<Media> GetByType(Media type);
        Media? GetItem(int id);
        List<Media> GetArchive();
        void ArchiveMedia(Media media);
        List<Media> GetNonArchivedMedia(Media type);
        Dictionary<string, int> Top3CheckedOutItems();
        bool IsCheckedOut(int id);
    }
}
