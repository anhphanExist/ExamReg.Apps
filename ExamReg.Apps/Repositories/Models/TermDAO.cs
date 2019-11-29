using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class TermDAO
    {
        public TermDAO()
        {
            ExamPeriods = new HashSet<ExamPeriodDAO>();
            StudentTerms = new HashSet<StudentTermDAO>();
        }

        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public Guid SemesterId { get; set; }
        public long CX { get; set; }

        public virtual SemesterDAO Semester { get; set; }
        public virtual ICollection<ExamPeriodDAO> ExamPeriods { get; set; }
        public virtual ICollection<StudentTermDAO> StudentTerms { get; set; }
    }
}
