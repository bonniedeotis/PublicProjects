﻿namespace StudentAdmissions.Application.Entities
{
    public class Weakness
    {
        public int WeaknessID { get; set; }
        public int WeaknessTypeID { get; set; }
        public string WeaknessName { get; set; }
        public string WeaknessDescription { get; set; }

        public WeaknessType WeaknessType { get; set; }
        public List<StudentWeakness> StudentWeaknesses { get; set; } = [];
    }
}
