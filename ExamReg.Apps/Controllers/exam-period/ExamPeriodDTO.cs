using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_period
{
    public class ExamPeriodDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public Guid ExamProgramId { get; set; }
        public string ExamProgramName { get; set; }
        public List<ExamRoomDTO> ExamRooms { get; set; }
    }
}