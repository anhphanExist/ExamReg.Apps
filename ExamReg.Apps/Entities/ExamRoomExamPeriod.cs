using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class ExamRoomExamPeriod : DataEntity
    {
        public Guid ExamRoomId { get; set; }
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public Guid ExamPeriodId { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }

    }
    public class ExamRoomExamPeriodFilter : FilterEntity
    {
        public ShortFilter RoomNumber { get; set; }
        public StringFilter AmphitheaterName { get; set; }
        public DateTimeFilter ExamDate { get; set; }
        public ShortFilter StartHour { get; set; }
        public ShortFilter FinishHour { get; set; }
        public ExamRoomExamPeriodOrder OrderBy { get; set; }
        public ExamRoomExamPeriodFilter() : base() { }
    }
    public enum ExamRoomExamPeriodOrder
    {
        RoomNumber,
        AmphitheaterName,
        ExamDate,
        StartHour,
        FinishHour
    }
}
