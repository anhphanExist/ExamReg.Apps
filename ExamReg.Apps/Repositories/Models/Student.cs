using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class Student
    {
        public Guid Id { get; set; }
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public long CX { get; set; }
    }
}
