using ExamReg.Apps.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ExamReg.DataInit
{
    public class CommonInit
    {
        protected ExamRegContext examRegContext;
        public CommonInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }

        public static Guid CreateGuid(string name)
        {
            MD5 md5 = MD5.Create();
            Byte[] myStringBytes = ASCIIEncoding.Default.GetBytes(name);
            Byte[] hash = md5.ComputeHash(myStringBytes);
            return new Guid(hash);
        }
    }
}
