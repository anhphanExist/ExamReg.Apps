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
        }

        public List<string> Init(List<string> semesterIds)
        {
            List<string> returnList = new List<string>();
            for (int i = 0; i < semesterIds.Count; i++)
            {
                returnList.Add(string.Format("Kỳ thi chính" + ":" + semesterIds[i]));
                returnList.Add(string.Format("Kỳ thi phụ" + ":" + semesterIds[i]));
            }

            for (int i = 0; i < returnList.Count; i++)
            {
                // lấy thông tin semesterCodes để gán cho ExamProgram
                string semesterId = returnList[i].Split(" ")[1];
                examRegContext.ExamProgram.Add(new ExamProgramDAO
                {
                    Id = CreateGuid(returnList[i]),
                    Name = returnList[i],
                    SemesterId = CreateGuid(semesterId)
                });
            }
            ExamProgramCodes.AddRange(returnList);
            return returnList;
        }
    }
}
