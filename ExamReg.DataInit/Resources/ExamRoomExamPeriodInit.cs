using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamRoomExamPeriodInit
    {
        private ExamRegContext examRegContext;

        public ExamRoomExamPeriodInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
