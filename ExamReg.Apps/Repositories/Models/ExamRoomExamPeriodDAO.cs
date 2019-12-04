using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRoomExamPeriodDAO
    {
        public ExamRoomExamPeriodDAO()
        {
            Students = new HashSet<StudentDAO>();
        }

        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }
        public long CX { get; set; }

        public virtual ExamPeriodDAO ExamPeriod { get; set; }
        public virtual ExamRoomDAO ExamRoom { get; set; }
        public virtual ICollection<StudentDAO> Students { get; set; }
    }
}
