using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IUserRepository
    {
        Task<int> Count(UserFilter filter);
        Task<User> Get(UserFilter filter);
        Task<List<User>> List(UserFilter filter);
        Task<bool> Create(User user);
        Task<bool> Update(User user);
        Task<bool> BulkInsert(List<User> users);
    }
    public class UserRepository : IUserRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public UserRepository(ExamRegContext examRegContext, ICurrentContext CurrentContext)
        {
            this.examRegContext = examRegContext;
            this.CurrentContext = CurrentContext;
        }

        public async Task<int> Count(UserFilter filter)
        {
            IQueryable<UserDAO> users = examRegContext.User.AsNoTracking();
            users = DynamicFilter(users, filter);
            return await users.CountAsync();
        }

        public async Task<bool> Create(User user)
        {
            examRegContext.User.Add(new UserDAO
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                IsAdmin = user.IsAdmin,
                StudentId = user.StudentId
            });
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> Get(UserFilter filter)
        {
            IQueryable<UserDAO> users = examRegContext.User.AsNoTracking();
            UserDAO userDAO = DynamicFilter(users, filter).FirstOrDefault();
            return new User
            {
                Id = userDAO.Id,
                Password = userDAO.Password,
                Username = userDAO.Username,
                IsAdmin = userDAO.IsAdmin,
                StudentId = userDAO.StudentId,
                StudentGivenName = userDAO.Student.GivenName,
                StudentLastName = userDAO.Student.LastName
            };
        }

        public async Task<List<User>> List(UserFilter filter)
        {
            IQueryable<UserDAO> users = examRegContext.User.AsNoTracking();
            users = DynamicFilter(users, filter);
            users = DynmaicOrder(users, filter);
            return await users.Select(u => new User
            {
                Id = u.Id,
                Username = u.Username,
                Password = u.Password,
                IsAdmin = u.IsAdmin,
                StudentId = u.StudentId,
                StudentGivenName = u.Student.GivenName,
                StudentLastName = u.Student.LastName
            }).ToListAsync();
        }

        public async Task<bool> Update(User user)
        {
            examRegContext.User
                .Where(u => u.Id.Equals(user.Id))
                .UpdateFromQuery(u => new UserDAO
                {
                    Password = user.Password
                });
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BulkInsert(List<User> users)
        {
            List<UserDAO> userDAOs = users.Select(u => new UserDAO()
            {
                Id = u.Id,
                Username = u.Username,
                Password = u.Password,
                IsAdmin = u.IsAdmin,
                StudentId = u.StudentId
            }).ToList();

            await examRegContext.User.BulkInsertAsync(userDAOs);
            return true;
        }

        private IQueryable<UserDAO> DynamicFilter(IQueryable<UserDAO> query, UserFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.Username != null)
                query = query.Where(u => u.Username.Equals(filter.Username));
            if (filter.Password != null)
                query = query.Where(u => u.Password.Equals(filter.Password));
            if (filter.StudentLastName != null)
                query = query.Where(u => u.Student.LastName.Equals(filter.StudentLastName));
            if (filter.StudentGivenName != null)
                query = query.Where(u => u.Student.GivenName.Equals(filter.StudentGivenName));
            return query;
        }

        private IQueryable<UserDAO> DynmaicOrder(IQueryable<UserDAO> users, UserFilter filter)
        {
            throw new NotImplementedException();
        }

    }
}
