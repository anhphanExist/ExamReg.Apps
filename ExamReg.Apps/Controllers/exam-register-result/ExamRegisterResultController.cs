using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.exam_register_result
{
    public class ExamRegisterResultRoute : Root
    {
        private const string Default = Base + "/exam-register-result";
    }

    [Authorize(Policy= "CanRegisterExam")]
    public class ExamRegisterResultController : ApiController
    {
        public ExamRegisterResultController(ICurrentContext CurrentContext) : base(CurrentContext)
        {

        }
    }
}