using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class User : DataEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public Guid? StudentId { get; set; }
    }
    public class UserFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public StringFilter Username { get; set; }
    }
}
