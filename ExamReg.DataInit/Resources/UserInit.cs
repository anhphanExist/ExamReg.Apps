using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class UserInit : CommonInit
    {
        public List<string> UserCodes { get; private set; }
        public UserInit(ExamRegContext examRegContext) : base(examRegContext)
        {
            UserCodes = new List<string>();
        }

        public List<string> Init(List<string> studentIds)
        {
            List<string> returnList = new List<string>
            {
                "admin"
            };
            returnList.AddRange(studentIds);
            examRegContext.User.Add(new UserDAO
            {
                Id = CreateGuid("admin"),
                Username = returnList[0],
                Password = returnList[0],
                IsAdmin = true
            });

            for (int i = 0; i < studentIds.Count; i++)
            {
                int username = 17020000 + i;
                examRegContext.User.Add(new UserDAO
                {
                    Id = CreateGuid(username.ToString()),
                    Username = username.ToString(),
                    Password = username.ToString(),
                    IsAdmin = false,
                    StudentId = CreateGuid(studentIds[i])
                });
            }

            UserCodes.AddRange(returnList);
            return returnList;
        }
    }
}
