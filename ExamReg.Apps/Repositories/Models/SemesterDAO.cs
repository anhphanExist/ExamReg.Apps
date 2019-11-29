using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class SemesterDAO
    {
        public SemesterDAO()
        {
            ExamPrograms = new HashSet<ExamProgramDAO>();
            Terms = new HashSet<TermDAO>();
        }

        public Guid Id { get; set; }
        public short StartYear { get; set; }
        public short EndYear { get; set; }
        public bool IsFirstHalf { get; set; }
        public long CX { get; set; }

        public virtual ICollection<ExamProgramDAO> ExamPrograms { get; set; }
        public virtual ICollection<TermDAO> Terms { get; set; }
    }
}
