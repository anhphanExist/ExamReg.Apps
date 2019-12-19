using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamReg.Apps.Repositories
{
    public interface IExamRegisterRepository
    {
        Task<int> Count(ExamRegisterFilter filter);
        Task<ExamRegister> Get(ExamRegisterFilter filter);
        Task<bool> Create(ExamRegister examRegister);
        Task<bool> Delete(Guid studentId, Guid examPeriodId);
    }
    public class ExamRegisterRepository : IExamRegisterRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext currentContext;

        public ExamRegisterRepository(ExamRegContext examRegContext, ICurrentContext currentContext)
        {
            this.examRegContext = examRegContext;
            this.currentContext = currentContext;
        }

        public async Task<int> Count(ExamRegisterFilter filter)
        {
            IQueryable<ExamRegisterDAO> query = examRegContext.ExamRegister.AsNoTracking();
            query = DynamicFilter(query, filter);
            return await query.CountAsync();
        }
        public async Task<ExamRegister> Get(ExamRegisterFilter filter)
        {
            IQueryable<ExamRegisterDAO> query = examRegContext.ExamRegister.AsNoTracking();
            query = DynamicFilter(query, filter);

            List<ExamRegister> list = await query.Select(e => new ExamRegister()
            {
                StudentId = e.StudentId,
                StudentNumber = e.Student.StudentNumber,
                ExamPeriodId = e.ExamPeriodId,
                ExamDate = e.Exam.ExamPeriod.ExamDate,
                StartHour = e.Exam.ExamPeriod.StartHour,
                FinishHour = e.Exam.ExamPeriod.FinishHour,
                SubjectName = e.Exam.ExamPeriod.Term.SubjectName,
                ExamProgramId = e.Exam.ExamPeriod.ExamProgramId,
                ExamProgramName = e.Exam.ExamPeriod.ExamProgram.Name,
                ExamRoomId = e.ExamRoomId,
                ExamRoomNumber = e.Exam.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = e.Exam.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = e.Exam.ExamRoom.ComputerNumber
            }).ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<bool> Create(ExamRegister examRegister)
        {
            ExamRegisterDAO newExamRegisterDAO = new ExamRegisterDAO
            {
                ExamPeriodId = examRegister.ExamPeriodId,
                StudentId = examRegister.StudentId,
                ExamRoomId = examRegister.ExamRoomId
            };
            examRegContext.ExamRegister.Add(newExamRegisterDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid studentId, Guid examPeriodId)
        {
            await examRegContext.ExamRegister.Where(e => e.StudentId.Equals(studentId) && e.ExamPeriodId.Equals(examPeriodId)).DeleteFromQueryAsync();
            return true;
        }

        private IQueryable<ExamRegisterDAO> DynamicFilter(IQueryable<ExamRegisterDAO> query, ExamRegisterFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            if (filter.StudentId != null)
                query = query.Where(q => q.StudentId, filter.StudentId);
            if (filter.TermId != null)
                query = query.Where(q => q.Exam.ExamPeriod.TermId, filter.TermId);
            if (filter.ExamPeriodId != null)
                query = query.Where(q => q.Exam.ExamPeriodId, filter.ExamPeriodId);
            return query;
        }
    }
}
