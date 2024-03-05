using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class StudentSectionService : IStudentSectionService
    {
        private AcademyContext _context;

        public StudentSectionService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result<StudentSection> AddStudentToSection(StudentSection model)
        {
            try
            {
                var alreadyEnrolled = _context.StudentSection.FirstOrDefault(s => s.SectionID == model.SectionID && s.StudentID == model.StudentID);

                if (alreadyEnrolled != null)
                {
                    return ResultFactory.Fail<StudentSection>($"Student ID {model.StudentID} is already enrolled in section {model.SectionID}");
                }
                _context.StudentSection.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentSection>(ex.Message);
            }

            return ResultFactory.Success(model);
        }

        public Result<SectionInfo> GetSection(int sectionId)
        {
            try
            {
                var info = new SectionInfo();

                var section = _context.Section
                    .Include(s => s.Instructor)
                    .FirstOrDefault(s => s.SectionID == sectionId);

                info.InstructorAlias = section.Instructor.Alias;
                info.StartTime = section.StartTime;
                info.EndTime = section.EndTime;
                info.StartDate = section.StartDate;
                info.EndDate = section.EndDate;

                return ResultFactory.Success(info);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<SectionInfo>(ex.Message);
            }
        }

        public Result<List<StudentSection>> GetStudentSections(int studentId)
        {
            var list = new List<StudentSection>();

            try
            {
                list = _context.StudentSection
                    .Where(s => s.StudentID == studentId)
                    .Include(s => s.Student)
                    .Include(s => s.Section.Instructor)
                    .Include(s => s.Section)
                        .ThenInclude(s => s.Course)
                        .ThenInclude(c => c.Subject)
                    .ToList();

                return ResultFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<StudentSection>>(ex.Message);
            }
        }

        public Result<StudentsInSection> GetStudentsInSection(int sectionId)
        {
            var list = new StudentsInSection();

            try
            {
                var studentSection = _context.StudentSection
                .Where(s => s.SectionID == sectionId)
                .Include(s => s.Student)
                .ToList();

                var section = _context.Section
                    .Include(s => s.Course)
                        .ThenInclude(c => c.Subject)
                    .Include(s => s.Instructor)
                    .FirstOrDefault(s => s.SectionID == sectionId);

                list.Students = studentSection;
                list.CourseName = section.Course.CourseName;
                list.CourseID = section.CourseID;
                list.Section = section;
                list.SubjectName = section.Course.Subject.SubjectName;
                list.InstructorAlias = section.Instructor.Alias;

                if (section.EndDate >= DateOnly.FromDateTime(DateTime.Today))
                {
                    list.EnrollmentStatus = Enrollment.Open;
                }
                else
                {
                    list.EnrollmentStatus = Enrollment.Closed;
                }

                return ResultFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<StudentsInSection>(ex.Message);
            }


        }
    }
}
