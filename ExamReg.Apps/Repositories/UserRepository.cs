﻿using ExamReg.Apps.Entities;
using ExamReg.Apps.Common;
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
        /*Task<User> Get(Guid Id);
        Task<User> Get(UserFilter filter);*/
    }
    public class UserRepository : IUserRepository
    {
        private ExamRegContext examRegContext;
        public UserRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
    }
}
