using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class Student : DataEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
    }
    public class StudentFilter : FilterEntity
    {
        public IntFilter StudentNumber { get; set; }
        public StringFilter LastName { get; set; }
        public StringFilter GivenName { get; set; }
        public DateTimeFilter Birthday { get; set; }
        public StudentOrder OrderBy { get; set; }
        public StudentFilter() : base()
        {

        }
    }
    public enum StudentOrder
    {
        StudentNumber,
        LastName,
        GivenName
    }
}
