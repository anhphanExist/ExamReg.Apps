using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class StudentTermDAO
    {
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public long CX { get; set; }
        public bool IsQualified { get; set; }

        public virtual StudentDAO Student { get; set; }
        public virtual TermDAO Term { get; set; }
    }
}
