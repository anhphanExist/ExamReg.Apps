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
            ExamRegisterDAO examRegisterDAO = DynamicFilter(query, filter).FirstOrDefault();
            if (examRegisterDAO == null)
                return null;
            return new ExamRegister
            {
                StudentId = examRegisterDAO.StudentId,
                StudentNumber = examRegisterDAO.Student.StudentNumber,
                ExamPeriodId = examRegisterDAO.ExamPeriodId,
                ExamDate = examRegisterDAO.Exam.ExamPeriod.ExamDate,
                StartHour = examRegisterDAO.Exam.ExamPeriod.StartHour,
                FinishHour = examRegisterDAO.Exam.ExamPeriod.FinishHour,
                SubjectName = examRegisterDAO.Exam.ExamPeriod.Term.SubjectName,
                ExamProgramId = examRegisterDAO.Exam.ExamPeriod.ExamProgramId,
                ExamProgramName = examRegisterDAO.Exam.ExamPeriod.ExamProgram.Name,
                ExamRoomId = examRegisterDAO.ExamRoomId,
                ExamRoomNumber = examRegisterDAO.Exam.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = examRegisterDAO.Exam.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = examRegisterDAO.Exam.ExamRoom.ComputerNumber
            };
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
            ExamRegisterDAO examRegisterDAO = examRegContext.ExamRegister
                .Where(e => e.StudentId.Equals(studentId) && e.ExamPeriodId.Equals(examPeriodId))
                .FirstOrDefault();
            examRegContext.ExamRegister.Remove(examRegisterDAO);
            await examRegContext.SaveChangesAsync();
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
