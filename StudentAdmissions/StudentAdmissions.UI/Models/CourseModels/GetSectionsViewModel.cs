using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.UI.Models.CourseModels
{
    public class GetSectionsViewModel
    {
        public Course Course { get; set; }
        public List<Section> Sections { get; set; }
        public string Message { get; set; } = "";
    }
}
