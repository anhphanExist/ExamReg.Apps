using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class ExamRoom : DataEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public int ComputerNumber { get; set; }
    }
    public class ExamRoomFilter : FilterEntity
    {
        public StringFilter AmphitheaterName { get; set; }
        public ShortFilter RoomNumber { get; set; }
        public IntFilter ComputerNumber { get; set; }
        public DateTime? ExamDate { get; set; }
        public DateTime? ExceptExamDate { get; set; }
        public short? ExceptStartHour { get; set; }
        public short? ExceptFinishHour { get; set; }
        public ExamRoomOrder OrderBy { get; set; }
        public ExamRoomFilter() : base()
        {

        }
    }
    public enum ExamRoomOrder
    {
        RoomNumber,
        ComputerNumber,
        AmphitheaterName
    }
}
