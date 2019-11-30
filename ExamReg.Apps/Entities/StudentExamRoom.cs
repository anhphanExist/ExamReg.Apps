using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class StudentExamRoom : DataEntity
    {
        public Guid StudentId { get; set; }
        public Guid ExamRoomId { get; set; }
    }
    public class StudentExamRoomFilter : FilterEntity
    {
        public GuidFilter StudentId { get; set; }
        public GuidFilter ExamRoomId { get; set; }
        public StudentExamRoomFilter() : base()
        {

        }
    }
}
