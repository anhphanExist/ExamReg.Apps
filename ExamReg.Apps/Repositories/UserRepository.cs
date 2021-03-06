﻿using ExamReg.Apps.Common;
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
            if (filter == null) return null;

            IQueryable<UserDAO> users = examRegContext.User.AsNoTracking();
            users = DynamicFilter(users, filter);

            List<User> list = await users.Select(u => new User
            {
                Id = u.Id,
                Password = u.Password,
                Username = u.Username,
                IsAdmin = u.IsAdmin,
                StudentId = u.StudentId,
                StudentGivenName = u.Student.GivenName,
                StudentLastName = u.Student.LastName
            }).ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<List<User>> List(UserFilter filter)
        {
            IQueryable<UserDAO> users = examRegContext.User.AsNoTracking();
            users = DynamicFilter(users, filter);
            users = DynamicOrder(users, filter);
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

        private IQueryable<UserDAO> DynamicOrder(IQueryable<UserDAO> query, UserFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case UserOrder.Username:
                            query = query.OrderBy(q => q.Username);
                            break;
                        case UserOrder.StudentLastName:
                            query = query.OrderBy(q => q.Student.LastName);
                            break;
                        case UserOrder.StudentGivenName:
                            query = query.OrderBy(q => q.Student.GivenName);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case UserOrder.Username:
                            query = query.OrderByDescending(q => q.Username);
                            break;
                        case UserOrder.StudentLastName:
                            query = query.OrderByDescending(q => q.Student.LastName);
                            break;
                        case UserOrder.StudentGivenName:
                            query = query.OrderByDescending(q => q.Student.GivenName);
                            break;
                        default:
                            query = query.OrderByDescending(q => q.CX);
                            break;
                    }
                    break;
                default:
                    query = query.OrderBy(q => q.CX);
                    break;
            }
            return query.Skip(filter.Skip).Take(filter.Take);
        }

    }
}
