using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class Semester
    {
        public Guid Id { get; set; }
        public short StartYear { get; set; }
        public short EndYear { get; set; }
        public bool IsFirstHalf { get; set; }
        public long CX { get; set; }
    }
}
