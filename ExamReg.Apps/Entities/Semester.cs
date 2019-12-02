using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class Semester : DataEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public short StartYear { get; set; }
        public short EndYear { get; set; }
        public bool IsFirstHalf { get; set; }
    }
    public class SemesterFilter : FilterEntity
    {
        public StringFilter Code { get; set; }
        public bool? IsFirstHalf { get; set; }
        public SemesterOrder OrderBy { get; set; }
        public SemesterFilter() : base()
        {

        }
    }

    public enum SemesterOrder
    {
        Code,
        IsFirstHalf
    }
}
