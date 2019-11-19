using ExamReg.Apps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid Id);
        Task<User> Get(UserFilter filter);
    }
    public class UserRepository
    {
    }
}
