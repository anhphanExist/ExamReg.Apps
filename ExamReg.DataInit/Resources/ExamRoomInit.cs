using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamRoomInit
    {
        private ExamRegContext examRegContext;

        public ExamRoomInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
