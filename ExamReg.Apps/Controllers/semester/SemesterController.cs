using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Services.MSemester;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.semester
{
    public class SemesterRoute : Root
    {
        private const string Default = Base + "/semester";
        public const string List = Default + "/list";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
    }

    [Authorize(Policy = "CanManage")]
    public class SemesterController : ApiController
    {
        private ISemesterService SemesterService;
        public SemesterController(ICurrentContext CurrentContext,
            ISemesterService SemesterService
            ) : base(CurrentContext)
        {
            this.SemesterService = SemesterService;
        }
    }
}