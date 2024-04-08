using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using LibraryManagement.Core.Interfaces.Services;

namespace LibraryManagement.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRespository _mediaRepository;

        public MediaService(IMediaRespository mediaRespository)
        {
            _mediaRepository = mediaRespository;
        }

        public Result<int> AddMedia(Media media)
        {
            try
            {
                var itemID = _mediaRepository.Add(media);
                return ResultFactory.Success(itemID);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<int>(ex.Message);
            }
        }

        public Result ArchiveMedia(int id)
        {
            try
            {
                var item = _mediaRepository.GetItem(id);
                if (item == null)
                {
                    return ResultFactory.Fail("Item does not exist!");
                }
                if (item.IsArchived == true)
                {
                    return ResultFactory.Fail("Item is already archived. Try another one.");
                }
                if (_mediaRepository.IsCheckedOut(id))
                {
                    return ResultFactory.Fail("Item is currently checked out and can't be archived.");
                }
                else
                {
                    _mediaRepository.ArchiveMedia(item);
                    return ResultFactory.Success();
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }

        public Result EditMedia(Media media)
        {
            try
            {
                _mediaRepository.Edit(media);
                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }

        public Result<List<Media>> GetAllMedia()
        {
            try
            {
                var media = _mediaRepository.GetAll();
                if (media.Count == 0)
                {
                    return ResultFactory.Fail<List<Media>>("Error getting the list of media.");
                }
                return ResultFactory.Success(media);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Media>>(ex.Message);
            }
        }

        public Result<List<Media>> GetArchive()
        {
            try
            {
                var media = _mediaRepository.GetArchive();

                if (media == null)
                {
                    return ResultFactory.Fail<List<Media>>("No media is archived currently.");
                }
                return ResultFactory.Success(media);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Media>>(ex.Message);
            }
        }

        public Result<List<Media>> GetNonArchivedMedia(Media type)
        {
            try
            {
                var media = _mediaRepository.GetNonArchivedMedia(type);
                if (media.Count == 0)
                {
                    return ResultFactory.Fail<List<Media>>("No available media exists currently.");
                }
                return ResultFactory.Success(media);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Media>>(ex.Message);
            }
        }

        public Result<Media> GetMediaByID(int id)
        {
            try
            {
                var media = _mediaRepository.GetItem(id);
                if (media == null)
                {
                    return ResultFactory.Fail<Media>($"Item with ID {id} does not exist.");
                }
                return ResultFactory.Success(media);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Media>(ex.Message);
            }
        }

        public Result<List<Media>> GetMediaByType(Media type)
        {
            try
            {
                var media = _mediaRepository.GetByType(type);
                if (media.Count() == 0)
                {
                    return ResultFactory.Fail<List<Media>>($"Media type with id {type.MediaTypeID} does not exist.");
                }
                return ResultFactory.Success(media);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Media>>(ex.Message);
            }
        }

        public Result<Dictionary<string, int>> Top3CheckedOutItems()
        {
            try
            {
                var items = _mediaRepository.Top3CheckedOutItems();
                return ResultFactory.Success(items);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Dictionary<string, int>>(ex.Message);
            }
        }
    }
}
