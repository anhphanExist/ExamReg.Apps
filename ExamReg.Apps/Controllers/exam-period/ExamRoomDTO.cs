using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_period
{
    public class ExamRoomDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public int ComputerNumber { get; set; }
    }

    public class ExamRoomFilterDTO : FilterDTO
    {
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
    }
}