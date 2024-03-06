using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Data.Repositories.Test_Repositories
{
    public class TestMediaRepository : IMediaRespository
    {
        List<Media> medias = new List<Media> { 
            new Media { MediaID = 1, MediaTypeID = 2, IsArchived = false, Title = "Chinatown",
                CheckoutLogs = new List<CheckoutLog> { 
                    new CheckoutLog {
                        CheckoutDate = new DateTime(2024, 1, 24), 
                        DueDate = new DateTime(2024, 1, 30), 
                        ReturnDate = null }  }}, 
            new Media { MediaID = 2, MediaTypeID = 2, IsArchived = false, Title = "Swordfish",
                CheckoutLogs = new List<CheckoutLog> { 
                    new CheckoutLog {
                    CheckoutDate = new DateTime(2024, 1, 4),
                    DueDate = new DateTime(2024, 1, 11),
                    ReturnDate = null} }},
            new Media { MediaID = 3, MediaTypeID = 1, IsArchived = true, Title = "The Great Gatsby"},
            new Media { MediaID = 4, MediaTypeID = 3, IsArchived = false, Title = "Jagged Little Pill",
                CheckoutLogs = new List<CheckoutLog> {
                    new CheckoutLog {
                    CheckoutDate = new DateTime(2024, 1, 20),
                    DueDate = new DateTime(2024, 1, 27),
                    ReturnDate = new DateTime(2024, 1, 26)} }}
        };

        public int Add(Media media)
        {
            media.MediaID = medias.Last().MediaID + 1;
            medias.Add(media);

            return media.MediaID;
        }

        public void ArchiveMedia(Media media)
        {
            var item = medias.FirstOrDefault(m => m.MediaID == media.MediaID);
            item.IsArchived = true;
        }

        public void Edit(Media media)
        {
            var item = medias.FirstOrDefault(m => m.MediaID == media.MediaID);

            if (item != null)
            {
                item.Title = media.Title;
                item.MediaTypeID = media.MediaTypeID;
            }
        }

        public List<Media> GetAll()
        {
            return medias;
        }

        public List<Media> GetArchive()
        {
            return medias.Where(m => m.IsArchived == true).ToList();
        }

        public List<Media> GetByType(Media type)
        {
            return medias.Where(m => m.MediaTypeID == type.MediaTypeID).ToList();
        }

        public Media? GetItem(int id)
        {
            return medias.FirstOrDefault(m => m.MediaID == id);
        }

        public List<Media> GetNonArchivedMedia(Media type)
        {
            return medias.Where(m => m.IsArchived == false && m.MediaTypeID == type.MediaTypeID).ToList();
        }

        public bool IsCheckedOut(int id)
        {
            var logs = medias.FirstOrDefault(m => m.MediaID == id).CheckoutLogs;
            var result = logs.FirstOrDefault(l => l.ReturnDate == null);

            return result != null ? true : false;
        }

        public Dictionary<string, int> Top3CheckedOutItems()
        {
            throw new NotImplementedException();
        }
    }
}
