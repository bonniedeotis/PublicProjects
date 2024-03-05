namespace StudentAdmissions.Application.DTOs
{
    public class SectionInfo
    {
        public string InstructorAlias { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
