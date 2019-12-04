using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class StudentDAO
    {
        public StudentDAO()
        {
            StudentTerms = new HashSet<StudentTermDAO>();
            Users = new HashSet<UserDAO>();
        }

        public Guid Id { get; set; }
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public long CX { get; set; }
        public Guid? ExamPeriodId { get; set; }
        public Guid? ExamRoomId { get; set; }

        public virtual ExamRoomExamPeriodDAO Exam { get; set; }
        public virtual ICollection<StudentTermDAO> StudentTerms { get; set; }
        public virtual ICollection<UserDAO> Users { get; set; }
    }
}
