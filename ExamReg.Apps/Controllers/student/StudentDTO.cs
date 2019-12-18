using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.student
{
    public class StudentDTO : DataDTO
    {
        public Guid Id { get; set; }
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public string Username { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
    }
}