using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers
{
    public class Root
    {
        protected const string Base = "api/ExamReg/APPS";
    }

    [Authorize]
    public class ApiController : ControllerBase
    {
        protected ICurrentContext CurrentContext;
        public ApiController(ICurrentContext CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }
    }
}