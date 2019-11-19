using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class StudentExamRoom : DataEntity
    {
        public Guid Id { get; set; }
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public int ComputerNumber { get; set; }
    }
    public class StudentExamRoomFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public IntFilter StudentNumber { get; set; }
        public StringFilter LastName { get; set; }
        public StringFilter GivenName { get; set; }
        public DateTimeFilter Birthday { get; set; }
        public short RoomNumber { get; set; }
        public StudentExamRoomFilter() : base()
        {

        }
    }
}
