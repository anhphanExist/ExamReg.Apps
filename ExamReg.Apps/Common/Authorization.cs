using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExamReg.Apps.Common
{
    public class StudentRequirement : IAuthorizationRequirement { }
    public class AdminRequirement : IAuthorizationRequirement { }

    public class StudentHandler : AuthorizationHandler<StudentRequirement>
    {
        private ICurrentContext CurrentContext;
        public StudentHandler(ICurrentContext CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StudentRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            if (!context.User.HasClaim(c => c.Type == "IsAdmin"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            CurrentContext.IsAdmin = bool.TryParse(context.User.FindFirst(c => c.Type == "IsAdmin").Value, out bool b) ? b : false;
            if (CurrentContext.IsAdmin)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            CurrentContext.UserId = Guid.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out Guid u) ? u : Guid.Empty;
            CurrentContext.Username = context.User.FindFirstValue(ClaimTypes.Name);
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        private ICurrentContext CurrentContext;
        public AdminHandler(ICurrentContext CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            if (!context.User.HasClaim(c => c.Type == "IsAdmin"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            CurrentContext.IsAdmin = bool.TryParse(context.User.FindFirst(c => c.Type == "IsAdmin").Value, out bool b) ? b : false;
            if (!CurrentContext.IsAdmin)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            CurrentContext.UserId = Guid.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out Guid u) ? u : Guid.Empty;
            CurrentContext.Username = context.User.FindFirstValue(ClaimTypes.Name);
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
