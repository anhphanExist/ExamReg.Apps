using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Common
{
    public class StudentRequirement : IAuthorizationRequirement { }
    public class AdminRequirement : IAuthorizationRequirement { }
}
