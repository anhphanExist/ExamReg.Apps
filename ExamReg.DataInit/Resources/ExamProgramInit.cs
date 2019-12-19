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

            // Thêm riêng lẻ 1 examProgram thành IsCurrent = true
            examRegContext.ExamProgram.Add(new ExamProgramDAO
            {
                Id = CreateGuid(string.Format(examProgramList[0] + ":" + semesterIds[0])),
                Name = string.Format(examProgramList[0] + ":" + semesterIds[0]),
                SemesterId = CreateGuid(semesterIds[0]),
                IsCurrent = true
            });
            returnList.Add(string.Format(examProgramList[0] + ":" + semesterIds[0]));

            // Thêm sao cho đống còn lại IsCurrent = false
            for (int j = 1; j < semesterIds.Count; j++)
            {
                examRegContext.ExamProgram.Add(new ExamProgramDAO
                {
                    Id = CreateGuid(string.Format(examProgramList[0] + ":" + semesterIds[j])),
                    Name = string.Format(examProgramList[0] + ":" + semesterIds[j]),
                    SemesterId = CreateGuid(semesterIds[0]),
                    IsCurrent = false
                });
                returnList.Add(string.Format(examProgramList[0] + ":" + semesterIds[j]));
            }

            for (int i = 1; i < examProgramList.Count; i++)
            {
                for (int j = 0; j < semesterIds.Count; j++)
                {
                    examRegContext.ExamProgram.Add(new ExamProgramDAO
                    {
                        Id = CreateGuid(string.Format(examProgramList[i] + ":" + semesterIds[j])),
                        Name = string.Format(examProgramList[i] + ":" + semesterIds[j]),
                        SemesterId = CreateGuid(semesterIds[j]),
                        IsCurrent = false
                    });
                    returnList.Add(string.Format(examProgramList[i] + ":" + semesterIds[j]));
                }
            }
            ExamProgramCodes.AddRange(returnList);
            return returnList;
        }
    }
}
