﻿namespace StudentAPI.Model
{
    public class UpdateStudentDto
    {
        public string Name { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string FatherName { get; set; } = string.Empty;

        public string MotherName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
