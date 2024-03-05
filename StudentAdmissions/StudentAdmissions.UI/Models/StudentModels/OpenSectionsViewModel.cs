using StudentAdmissions.Application.Entities;

namespace StudentAdmissions.UI.Models.StudentModels
{
    public class OpenSectionsViewModel
    {
        public List<Section> OpenSections { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
    }
}
