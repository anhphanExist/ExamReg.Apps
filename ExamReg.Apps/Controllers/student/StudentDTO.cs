using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.student
{
    public class StudentDTO : DataDTO
    {
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public string Username { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
    }
}