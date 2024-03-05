﻿using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAdmissions.Application.Entities
{
    public class Course
    {
        public int CourseID { get; set; }
        public int SubjectID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public decimal Credits { get; set; }

        public Subject Subject { get; set; }
        public List<Section> Sections { get; set; }
    }
}
