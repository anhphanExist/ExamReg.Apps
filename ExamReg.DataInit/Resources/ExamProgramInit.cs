using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamProgramInit : CommonInit
    {
        public List<string> ExamProgramCodes { get; private set; }
        public ExamProgramInit(ExamRegContext examRegContext) : base(examRegContext)
        {
            ExamProgramCodes = new List<string>();
        }

        public List<string> Init(List<string> semesterIds)
        {
            List<string> returnList = new List<string>();
            List<string> examProgramList = new List<string>
            {
                "Kỳ thi chính",
                "Kỳ thi phụ"
            };

            for (int i = 0; i < examProgramList.Count; i++)
            {
                for (int j = 0; j < semesterIds.Count; j++)
                {
                    examRegContext.ExamProgram.Add(new ExamProgramDAO
                    {
                        Id = CreateGuid(string.Format(examProgramList[i] + ":" + semesterIds[j])),
                        Name = string.Format(examProgramList[i] + ":" + semesterIds[j]),
                        SemesterId = CreateGuid(semesterIds[j])
                    });
                    returnList.Add(string.Format(examProgramList[i] + ":" + semesterIds[j]));
                }
            }
            ExamProgramCodes.AddRange(returnList);
            return returnList;
        }
    }
}
