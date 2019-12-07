using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class StudentTermInit
    {
        private ExamRegContext examRegContext;

        public StudentTermInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
