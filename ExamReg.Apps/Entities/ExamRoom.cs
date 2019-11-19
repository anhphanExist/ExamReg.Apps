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
        public short RoomNumber { get; set; }
        public string AmphitheaterName { get; set; }
        public int ComputerNumber { get; set; }
    }
    public class ExamRoomFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public List<Guid> Ids { get; set; }
        public short RoomNumber { get; set; }
        public StringFilter AmphitheaterName { get; set; }
        public IntFilter ComputerNumber { get; set; }
        public ExamRoomFilter() : base()
        {

        }
    }
    public enum ExamRoomOrder
    {
        Id,
        ComputerNumber
    }
}
