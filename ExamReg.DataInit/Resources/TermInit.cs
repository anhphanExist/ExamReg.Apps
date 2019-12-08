using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class TermInit : CommonInit
    {
        public List<string> TermCodes { get; private set; }
        public TermInit(ExamRegContext examRegContext) : base(examRegContext)
        {
        }

        public List<string> Init(List<string> semesterIds)
        {
            List<string> returnList = new List<string>();
            List<string> termList = new List<string>
            {
                "Cấu trúc dữ liệu và giải thuật",
                "Xác suất thống kê",
                "Toán rời rạc",
                "Tối ưu hoá",
                "Giải tích 1",
                "Giải tích 2",
                "Đại số",
                "Lập trình hướng đối tượng",
                "Tin học cơ sở 4",
                "Phát triển ứng dụng web",
                "Cơ sở dữ liệu",
                "Mạng máy tính",
                "Phương pháp tính",
                "Công nghệ phần mềm",
                "Tín hiệu và hệ thống",
                "Kiến trúc máy tính",
                "Tin học cơ sở 1",
                "Vật lý đại cương 1",
                "Vật lý đại cương 2",
                "Những nguyên lý cơ bản của chủ nghĩa Mác",
                "Tư tưởng Hồ Chí Minh",
                "Đường lối cách mạng Đảng Cộng sản Việt Nam",
                "Lập trình nâng cao",
                "Kinh tế vi mô",
                "Kinh tế vĩ mô",
                "Đất nước học Nhật Bản"
            };

            for (int i = 0; i < semesterIds.Count; i++)
            {
                for (int j = 0; j < termList.Count; j++)
                {
                    examRegContext.Term.Add(new TermDAO
                    {
                        Id = CreateGuid(string.Format(termList[j] + ":" + semesterIds[i])),
                        SubjectName = termList[j],
                        SemesterId = CreateGuid(semesterIds[i])
                    });
                    returnList.Add(string.Format(termList[j] + ":" + semesterIds[i]));
                }
            }
            TermCodes.AddRange(returnList);
            return returnList;

        }
    }
}
