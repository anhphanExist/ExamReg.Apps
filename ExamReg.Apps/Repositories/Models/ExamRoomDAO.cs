using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRoomDAO
    {
        public ExamRoomDAO()
        {
            ExamRoomExamPeriods = new HashSet<ExamRoomExamPeriodDAO>();
            StudentExamRooms = new HashSet<StudentExamRoomDAO>();
        }

        public Guid Id { get; set; }
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public int ComputerNumber { get; set; }
        public long CX { get; set; }

        public virtual ICollection<ExamRoomExamPeriodDAO> ExamRoomExamPeriods { get; set; }
        public virtual ICollection<StudentExamRoomDAO> StudentExamRooms { get; set; }
    }
}
