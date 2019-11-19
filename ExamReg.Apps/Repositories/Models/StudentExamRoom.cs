using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class StudentExamRoom
    {
        public Guid StudentId { get; set; }
        public Guid ExamRoomId { get; set; }
        public long CX { get; set; }
    }
}
