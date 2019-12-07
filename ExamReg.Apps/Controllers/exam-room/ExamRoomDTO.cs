using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.exam_room
{
    public class ExamRoomDTO : DataDTO
    {
        public Guid Id { get; set; }
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public int ComputerNumber { get; set; }
        public string Code { get; set; }
    }
}