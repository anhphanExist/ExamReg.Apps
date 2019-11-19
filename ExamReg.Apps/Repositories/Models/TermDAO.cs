using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class TermDAO
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public Guid SemesterId { get; set; }
        public long CX { get; set; }
    }
}
