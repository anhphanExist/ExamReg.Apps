using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Controllers.exam_period
{
    public class ExamProgramDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class ExamProgramFilterDTO : FilterDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
        public bool IsCurrent { get; set; }
    }
}
