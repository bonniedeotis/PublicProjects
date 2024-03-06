using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Data.Repositories.EF_Repositories
{
    public class EFMediaRepository : IMediaRespository
    {
        private LibraryContext _dbContext;

        public EFMediaRepository(string connectionString)
        {
            _dbContext = new LibraryContext(connectionString);
        }

        public int Add(Media media)
        {
            _dbContext.Media.Add(media);
            _dbContext.SaveChanges();

            return media.MediaID;
        }

        public void ArchiveMedia(Media media)
        {
            var item = _dbContext.Media.FirstOrDefault(m => m.MediaID == media.MediaID);

            media.IsArchived = true;

            _dbContext.SaveChanges();
        }

        public void Edit(Media media)
        {
            var item = _dbContext.Media.FirstOrDefault(m => m.MediaID == media.MediaID);

            if (item != null)
            {
                item.Title = media.Title;
                item.MediaTypeID = media.MediaTypeID;

                _dbContext.SaveChanges();
            }
        }

        public List<Media> GetNonArchivedMedia(Media type)
        {
            return _dbContext.Media.Where(m => m.IsArchived == false && m.MediaTypeID == type.MediaTypeID).ToList();
        }

        public List<Media> GetAll()
        {
            return _dbContext.Media.Include(m => m.MediaType).ToList();
        }

        public List<Media> GetArchive()
        {
            return _dbContext.Media
                .Where(m => m.IsArchived == true)
                .OrderBy(m => m.MediaTypeID)
                .ThenBy(m => m.Title)
                .Include(m => m.MediaType)
                .ToList();
        }

        public List<Media> GetByType(Media type)
        {
            return _dbContext.Media.Where(m => m.MediaTypeID == type.MediaTypeID)
                .Include(m => m.MediaType)
                .ToList();
        }

        public Media? GetItem(int id)
        {
            return _dbContext.Media.FirstOrDefault(m => m.MediaID == id);
        }

        public Dictionary<string, int> Top3CheckedOutItems()
        {
            return _dbContext.CheckoutLog.GroupBy(cl => cl.Media.Title)
                .Select(m => new
                {
                    title = m.Key,
                    Count = m.Count()
                })
                .OrderByDescending(m => m.Count)
                .Take(3)
                .ToDictionary(m => m.title, m => m.Count);
        }

        public bool IsCheckedOut(int id)
        {
            var result = _dbContext.CheckoutLog.FirstOrDefault(cl => cl.MediaID == id && cl.ReturnDate == null);
            return result != null ? true : false;
        }
    }
}
