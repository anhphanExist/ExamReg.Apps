using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class StudentInit : CommonInit
    {
        public List<string> StudentCodes { get; private set; }
        public StudentInit(ExamRegContext examRegContext) : base(examRegContext)
        {
            StudentCodes = new List<string>();
        }

        public List<string> Init()
        {
            List<string> returnList = new List<string>
            {
                "Phan Anh",
                "Lưu Lê Tuấn Đạt",
                "Vũ Thị Thanh Mai",
                "Nguyễn Văn Lâm",
                "Quế Ngọc Hải",
                "La Mạnh Hừu",
                "Vuỹ Bá Du",
                "Nguyễn Quang Hải",
                "Nguyễn Công Phượng",
                "Nguyễn Phan Thu Trang",
                "Mai Thu Phương",
                "Trần Thị Khánh Linh",
                "Nguyễn Thị Thuỳ",
                "Đoàn Văn Hậu",
                "Thái Huy Nhật Quang",
                "Lê Vũ Hà",
                "Bùi Tiến Dũng",
                "Nguyễn Thị Vân",
                "Lê Công Vinh",
                "Lê Trần Hải Tùng",
                "Lã Đức Việt",
                "Tôn Ngộ Không",
                "Phạm Nguyễn Thanh Phương",
                "Vũ Thuỳ Dương"
            };
            var rand = new Random();

            for (int i = 0; i < returnList.Count; i++)
            {
                // Tách họ tên vào 2 biến
                // email = ghép tên vào lowercase, thêm đuôi gmail.com vào đuôi email
                string[] nameParts = returnList[i].Split(" ");
                string givenName = "";
                string email = nameParts[0].ToLower();
                for (int j = 1; j < nameParts.Length; j++)
                {
                    email += nameParts[j].ToLower();
                    givenName += nameParts[j];
                    givenName += (j < (nameParts.Length - 1) ? " " : "");
                }
                email += "@gmail.com";
                email = email.ChangeToEnglishChar();

                examRegContext.Student.Add(new StudentDAO
                {
                    Id = CreateGuid(returnList[i]),
                    StudentNumber = 17020000 + i,
                    LastName = nameParts[0],
                    GivenName = givenName,
                    Birthday = new DateTime(1999, rand.Next(1, 13), rand.Next(1, 29)),
                    Email = email
                });
            }

            StudentCodes.AddRange(returnList);
            return returnList;
        }
    }
}
