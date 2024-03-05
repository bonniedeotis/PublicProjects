using StudentAdmissions.Application.DTOs;
using StudentAdmissions.Application.Entities;
using StudentAdmissions.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentAdmissions.Application.Implentations
{
    public class InstructorService : IInstructorService
    {
        private AcademyContext _context;

        public InstructorService(DbContextOptions options)
        {
            _context = new AcademyContext(options);
        }

        public Result<List<Instructor>> GetInstructors()
        {
            try
            {
                var instructors = _context.Instructor.ToList();

                if (instructors.Count == 0)
                {
                    return ResultFactory.Fail<List<Instructor>>("Error getting instructor list");
                }

                return ResultFactory.Success(instructors);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Instructor>>(ex.Message);
            }

        }
    }
}
