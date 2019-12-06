using ExamReg.Apps.Common;

namespace ExamReg.Apps.Controllers.semester
{
    public class SemesterDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public short StartYear { get; set; }
        public short EndYear { get; set; }
        public bool IsFirstHalf { get; set; }
    }
}