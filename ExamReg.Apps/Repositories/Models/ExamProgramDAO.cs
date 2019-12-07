using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamProgramDAO
    {
        public ExamProgramDAO()
        {
            ExamPeriods = new HashSet<ExamPeriodDAO>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SemesterId { get; set; }
        public long CX { get; set; }
        public bool IsCurrent { get; set; }

        public virtual SemesterDAO Semester { get; set; }
        public virtual ICollection<ExamPeriodDAO> ExamPeriods { get; set; }
    }
}
