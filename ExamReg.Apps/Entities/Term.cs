using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class Term : DataEntity
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
        public List<ExamPeriod> ExamPeriods { get; set; }
        public List<Student> QualifiedStudents { get; set; }
    }
    public class TermFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public IntFilter StudentNumber { get; set; }
        public StringFilter SubjectName { get; set; }
        public GuidFilter SemesterId { get; set; }
        public StringFilter SemesterCode { get; set; }
        public TermOrder OrderBy { get; set;  }
        public TermFilter() : base() { }
    }
    public enum TermOrder
    {
        SubjectName,
        SemesterCode
    }
}
