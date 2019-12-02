using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class ExamRegisterRoute : Root
    {
        private const string Default = Base + "/exam-register-result";
    }

    [Authorize(Policy = "CanRegisterExam")]
    public class ExamRegisterController : ApiController
    {
        public ExamRegisterController(ICurrentContext CurrentContext) : base(CurrentContext)
        {

        }
    }
}