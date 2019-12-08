using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamRoomExamPeriodInit : CommonInit
    {
        public List<string> ExamRoomExamPeriodCodes { get; private set; }
        public ExamRoomExamPeriodInit(ExamRegContext examRegContext) : base(examRegContext)
        {
        }

        public List<string> Init(string examPeriodId, string examRoomId)
        {
            throw new NotImplementedException();
        }
    }
}
