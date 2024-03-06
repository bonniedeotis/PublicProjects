using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibraryManagement.ConsoleUI.IO
{
    public class MediaWorkflows
    {
        public static void MediaMenuChoice(IMediaService service)
        {
            int choice = 0;

            while (choice != 7)
            {
                choice = Menus.MediaManagementMenu();

                switch (choice)
                {
                    case 1:
                        GetAllMedia(service);
                        break;
                    case 2:
                        AddMedia(service);
                        break;
                    case 3:
                        EditMedia(service);
                        break;
                    case 4:
                        ArchiveMedia(service);
                        break;
                    case 5:
                        GetArchivedMedia(service);
                        break;
                    case 6:
                        //view most popular report
                        GetTop3CheckedOutItems(service);
                        break;
                    case 7:
                        return;
                }
            }
        }

        public static void GetAllMedia(IMediaService service)
        {
            Console.Clear();
            Console.WriteLine("Media List");
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Type",-15} {"Archived?",-6}");
            Console.WriteLine(new string('=', 100));

            var result = service.GetAllMedia();

            if (result.Ok)
            {
                foreach (var m in result.Data)
                {
                    Console.WriteLine($"{m.MediaID,-5} {m.Title,-32} {m.MediaType.MediaTypeName,-15} {m.IsArchived,-6}");
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }

        public static void AddMedia(IMediaService service)
        {
            Console.Clear();
            Console.WriteLine("Add Media");
            Console.WriteLine("================");
            Console.WriteLine();

            var newMedia = new Media();

            newMedia.Title = Utilities.GetRequiredString("Enter Title: ");
            do
            {
                string type = Utilities.GetRequiredString("Select type of media: (B)ook, (D)VD, or Digital (M)edia: ").ToUpper(); // need to list out types
                switch (type)
                {
                    case "B":
                        newMedia.MediaTypeID = 1;
                        break;
                    case "D":
                        newMedia.MediaTypeID = 2;
                        break;
                    case "M":
                        newMedia.MediaTypeID = 3;
                        break;
                    default:
                        Console.WriteLine("That is not a valid media type.");
                        break;
                }
            } while (newMedia.MediaTypeID == 0);

            var result = service.AddMedia(newMedia);

            if (result.Ok)
            {
                Console.WriteLine($"Media created with id: {result.Data}");
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }

        public static void EditMedia(IMediaService service)
        {
            Console.Clear();
            Console.WriteLine("Add Media");
            Console.WriteLine("================");
            Console.WriteLine();

            DisplayAllMediaByType(service);
            
            do
            {
                var id = Utilities.GetPositiveInteger("\nSelect the ID of the item you want to edit: ");

                var item = service.GetMediaByID(id);
                if (item.Ok)
                {
                    do
                    {
                        var fieldToEdit = Utilities.GetRequiredString("Would you like to edit the (T)itle or (M)edia Type?: ").ToUpper();

                        if (fieldToEdit == "T")
                        {
                            item.Data.Title = Utilities.GetRequiredString("Enter new title: ");
                            break;
                        }
                        if (fieldToEdit == "M")
                        {  
                            Utilities.DisplayTypeOptions();
                            item.Data.MediaTypeID = Utilities.GetPositiveInteger("Enter new type: ");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Try again.");
                        }
                    } while (true);

                    var result = service.EditMedia(item.Data);

                    if (result.Ok)
                    {
                        Console.WriteLine($"\nItem with id: {id} was updated");
                    }
                    else
                    {
                        Console.WriteLine(result.Message);
                    }

                    Utilities.AnyKey();
                    return;
                }
                else
                {
                    Console.WriteLine(item.Message);
                }
            } while (true);
        }

        public static void GetMediaByType(IMediaService service, Media type)
        {
            Console.Clear();
            Console.WriteLine("Media List");
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Type",-15}");
            Console.WriteLine(new string('=', 100));

            var result = service.GetMediaByType(type);

            if (result.Ok)
            {
                foreach (var m in result.Data)
                {
                    if (m.IsArchived == false)
                    {
                        Console.WriteLine($"{m.MediaID,-5} {m.Title,-32} {m.MediaType.MediaTypeName,-15}");
                    }
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }

        public static void GetArchivedMedia(IMediaService service)
        {
            Console.Clear();
            Console.WriteLine("Archived Media List");
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Type",-15} {"Archived?",-6}");
            Console.WriteLine(new string('=', 100));

            var result = service.GetArchive();

            if (result.Ok)
            {
                foreach (var m in result.Data)
                {
                    Console.WriteLine($"{m.MediaID,-5} {m.Title,-32} {m.MediaType.MediaTypeName,-15} {m.IsArchived,-6}");
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }

        public static void ArchiveMedia(IMediaService service)
        {
            Console.Clear();
            GetNonArchivedMedia(service);

            do
            {
                var id = Utilities.GetPositiveInteger("\nEnter ID of item to archive: ");

                var result = service.ArchiveMedia(id);

                if (result.Ok)
                {
                    Console.WriteLine($"\nItem ID {id} was changed to archived.");
                    Utilities.AnyKey();
                }
                else
                {
                    Console.WriteLine(result.Message);
                    Utilities.AnyKey();
                }
                break;
            } while (true);

        }

        public static void DisplayAllMediaByType(IMediaService service)
        {
            bool validChoice = false;
            
            do
            {
                var choice = Utilities.GetRequiredString("Select type of media: (B)ook, (D)VD, or Digital (M)edia: ").ToUpper();
                var type = new Media();

                switch (choice)
                {
                    case "B":
                        validChoice = true;
                        type.MediaTypeID = 1;
                        GetMediaByType(service, type);
                        break;
                    case "D":
                        validChoice = true;
                        type.MediaTypeID = 2;
                        GetMediaByType(service, type);
                        break;
                    case "M":
                        validChoice = true;
                        type.MediaTypeID = 3;
                        GetMediaByType(service, type);
                        break;
                    default:
                        Console.WriteLine("Invalid media type. Try again.");
                        break;
                }
            } while (validChoice == false);
        }

        public static void GetNonArchivedMedia(IMediaService service)
        {
            bool validChoice = false;

            do
            {
                var choice = Utilities.GetRequiredString("Select type of media to archive: (B)ook, (D)VD, or Digital (M)edia: ").ToUpper();
                var type = new Media();

                switch (choice)
                {
                    case "B":
                        validChoice = true;
                        type.MediaTypeID = 1;
                        DisplayNonArchivedMedia(service, type);
                        break;
                    case "D":
                        validChoice = true;
                        type.MediaTypeID = 2;
                        DisplayNonArchivedMedia(service, type);
                        break;
                    case "M":
                        validChoice = true;
                        type.MediaTypeID = 3;
                        DisplayNonArchivedMedia(service, type);
                        break;
                    default:
                        Console.WriteLine("Invalid media type. Try again.");
                        break;
                }
            } while (validChoice == false);
        }

        public static void DisplayNonArchivedMedia(IMediaService service, Media type)
        {
            Console.Clear();
            Console.WriteLine("Non-Archived Media List");
            Console.WriteLine();
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Type",-15} {"Archived?",-6}");
            Console.WriteLine(new string('=', 100));

            var result = service.GetNonArchivedMedia(type);

            if (result.Ok)
            {
                foreach (var m in result.Data)
                {
                    if (m.IsArchived == false)
                    {
                        Console.WriteLine($"{m.MediaID,-5} {m.Title,-32} {m.MediaType.MediaTypeName,-15} {m.IsArchived,-6}");
                    }
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }

        public static void GetTop3CheckedOutItems(IMediaService service)
        {
            Console.Clear();
            Console.WriteLine("Top 3 Checked Out Items");
            Console.WriteLine();
            Console.WriteLine($"{"Title",-32} {"# of Checkouts",-5}");
            Console.WriteLine(new string('=', 60));

            var result = service.Top3CheckedOutItems();

            if (result.Ok)
            {
                foreach (var item in result.Data)
                {
                    Console.WriteLine($"{item.Key,-32} {item.Value,-5}");
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }
    }
}
