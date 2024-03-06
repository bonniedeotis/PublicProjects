using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Data.Repositories.Test_Repositories;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class MediaServiceLayerTests
    {
        private IMediaService _service;

        private List<Media> getMediaItem1 = new List<Media> {
            new Media { 
                MediaID = 1, MediaTypeID = 1, IsArchived = false, Title = "The World of Coffee",
                CheckoutLogs = new List<CheckoutLog> {
                    new CheckoutLog {
                        CheckoutDate = new DateTime(2024, 1, 24),
                        DueDate = new DateTime(2024, 1, 30),
                        ReturnDate = null }  }} };
        private List<Media> getByType2 = new List<Media>
        {
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
                    ReturnDate = null} }}
        };

        private List<Media> nonArchivedMediaType3 = new List<Media> {
            new Media { MediaID = 4, MediaTypeID = 3, IsArchived = false, Title = "Jagged Little Pill",
                CheckoutLogs = new List<CheckoutLog> {
                    new CheckoutLog {
                    CheckoutDate = new DateTime(2024, 1, 20),
                    DueDate = new DateTime(2024, 1, 27),
                    ReturnDate = new DateTime(2024, 1, 26)} }}};

        [SetUp]
        public void Init()
        {
            _service = new MediaService(new TestMediaRepository());
        }

        [Test]
        public void ArchiveMedia_Success()
        {
            var test = _service.ArchiveMedia(4);

            var expected = ResultFactory.Success();

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void ArchiveMedia_Fail_AlreadyArchived()
        {
            var test = _service.ArchiveMedia(3);

            var expected = ResultFactory.Fail("Item is already archived. Try another one.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }

        [Test]
        public void ArchiveMedia_Fail_CurrentlyCheckedOut()
        {
            var test = _service.ArchiveMedia(1);

            var expected = ResultFactory.Fail("Item is currently checked out and can't be archived.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }

        [Test]
        public void ArchiveMedia_Fail_ItemDoesNotExist()
        {
            var test = _service.ArchiveMedia(5);

            var expected = ResultFactory.Fail("Item does not exist!");

            Assert.That(expected.Message, Is.EqualTo(test.Message));
            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void GetByType_Success()
        {
            var type = new Media { MediaTypeID = 2 };
            var test = _service.GetMediaByType(type);

            var expected = ResultFactory.Success(getByType2);

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Data, Is.EqualTo(test.Data));
        }

        [Test]
        public void GetByType_Fail()
        {
            int id = 4;
            var type = new Media { MediaTypeID = id };
            var test = _service.GetMediaByType(type);

            var expected = ResultFactory.Fail<List<Media>>($"Media type with id {id} does not exist.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }

        [Test]
        public void GetArchive_Success()
        {
            var list = new List<Media> { new Media { MediaID = 3, MediaTypeID = 1, IsArchived = true, Title = "The Great Gatsby" } };
            var test = _service.GetArchive();

            var expected = ResultFactory.Success<List<Media>>(list);

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Data, Is.EqualTo(test.Data));
        }

        [Test]
        public void GetMediaByID_Fail()
        {
            int id = 9;
            var test = _service.GetMediaByID(id);

            var expected = ResultFactory.Fail<Media>($"Item with ID {id} does not exist.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }

        [Test]
        public void GetMediaByID_Success()
        {
            int id = 1;
            var test = _service.GetMediaByID(id);

            var expected = ResultFactory.Success(getMediaItem1);

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }

        [Test]
        public void GetNonArchivedMedia_Success()
        {
            var mediaType = new Media { MediaTypeID = 3 };
            var test = _service.GetNonArchivedMedia(mediaType);

            var expected = ResultFactory.Success(nonArchivedMediaType3);

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Data, Is.EqualTo(test.Data));
        }

        [Test]
        public void GetNonArchivedMedia_Fail()
        {
            var mediaType = new Media { MediaTypeID = 1 };
            var test = _service.GetNonArchivedMedia(mediaType);

            var expected = ResultFactory.Fail<List<Media>>("No available media exists currently.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }
    }
}
