using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.term
{
    public class TermDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
    }

    public class TermFilterDTO : FilterDTO
    {
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
    }
}