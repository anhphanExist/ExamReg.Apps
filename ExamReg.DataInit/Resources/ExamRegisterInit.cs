using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamRegisterInit : CommonInit
    {
        public List<string> ExamRegisterCodes { get; private set; }
        public ExamRegisterInit(ExamRegContext examRegContext) : base(examRegContext)
        {
            ExamRegisterCodes = new List<string>();
        }

        public List<string> Init(string studentId, string examPeriodId, string examRoomId)
        {
            throw new NotImplementedException();
        }
    }
}
