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
            ExamRoomExamPeriodCodes = new List<string>();
        }

        public List<string> Init(List<string> examPeriodIds, List<string> examRoomIds)
        {
            List<string> returnList = new List<string>();

            int iter = 0;
            // mỗi examPeriod sẽ có 2 phòng thi
            foreach (string examPeriodId in examPeriodIds)
            {
                examRegContext.ExamRoomExamPeriod.Add(new ExamRoomExamPeriodDAO
                {
                    ExamPeriodId = CreateGuid(examPeriodId),
                    ExamRoomId = CreateGuid(examRoomIds[iter])
                });
                returnList.Add(string.Format(examPeriodId + "!" + examRoomIds[iter]));
                examRegContext.ExamRoomExamPeriod.Add(new ExamRoomExamPeriodDAO
                {
                    ExamPeriodId = CreateGuid(examPeriodId),
                    ExamRoomId = CreateGuid(examRoomIds[iter + 1])
                });
                returnList.Add(string.Format(examPeriodId + "!" + examRoomIds[iter]));


                iter = (iter + 2) % examRoomIds.Count;
            }

            ExamRoomExamPeriodCodes.AddRange(returnList);
            return returnList;
        }
    }
}
