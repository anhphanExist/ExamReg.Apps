using ExamReg.Apps.Entities;
using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IStudentTermRepository
    {

    }
    public class StudentTermRepository : IStudentTermRepository
    {
        private ExamRegContext examRegContext;
        public StudentTermRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
    }
}
