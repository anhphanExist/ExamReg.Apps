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
    public interface IExamRoomExamPeriodRepository
    {
        Task<ExamRoomExamPeriod> Get(Guid Id);
    }
    public class ExamRoomExamPeriodRepository : IExamRoomExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        public ExamRoomExamPeriodRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<ExamRoomExamPeriod> Get(Guid Id)
        {
            return new ExamRoomExamPeriod()
                {

                };
        }
    }
}
