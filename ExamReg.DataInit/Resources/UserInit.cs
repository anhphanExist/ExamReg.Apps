using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class UserInit
    {
        private ExamRegContext examRegContext;

        public UserInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
