using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.student_profile
{
    public class StudentProfileRoute : Root
    {
        private const string Default = Base + "/student-profile";
        public const string GetInfoStudent = Default + "/get";
        public const string Update = Default + "/update";
    }

    [Authorize(Policy = "CanRegisterExam")]
    public class StudentProfileController : ApiController
    {
        public StudentProfileController(ICurrentContext CurrentContext) : base(CurrentContext)
        {

        }
    }
}