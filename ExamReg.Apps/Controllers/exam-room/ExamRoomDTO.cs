using ExamReg.Apps.Common;

namespace ExamReg.Apps.Controllers.exam_room
{
    public class ExamRoomDTO : DataDTO
    {
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public int ComputerNumber { get; set; }
    }
}