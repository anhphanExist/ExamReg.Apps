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
        Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter);
        Task<bool> Create(ExamRoomExamPeriod ExamRoomExamPeriod);
        Task<bool> Delete(ExamRoomExamPeriod ExamRoomExamPeriod);
        Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);
    }
    public class ExamRoomExamPeriodRepository : IExamRoomExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        public ExamRoomExamPeriodRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<bool> Create(ExamRoomExamPeriod examRoomExamPeriod)
        {
            ExamRoomExamPeriodDAO examRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(examRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(examRoomExamPeriod.ExamPeriodId)))
                .FirstOrDefault();
            if (examRoomExamPeriodDAO == null)
            {
                examRoomExamPeriodDAO = new ExamRoomExamPeriodDAO()
                {
                    ExamRoomId = examRoomExamPeriod.ExamRoomId,
                    ExamPeriodId = examRoomExamPeriod.ExamPeriodId
                };

                await examRegContext.ExamRoomExamPeriod.AddAsync(examRoomExamPeriodDAO);
            }
            else
            {
                //
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(ExamRoomExamPeriod ExamRoomExamPeriod)
        {
            ExamRoomExamPeriodDAO ExamRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(ExamRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(ExamRoomExamPeriod.ExamPeriodId)))
                .AsNoTracking()
                .FirstOrDefault();

            examRegContext.ExamRoomExamPeriod.Remove(ExamRoomExamPeriodDAO);
            await examRegContext.SaveChangesAsync();
            return true;
            
        }

        public async Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter)
        {
            IQueryable<ExamRoomExamPeriodDAO> examRoomExamPeriodDAOs = examRegContext.ExamRoomExamPeriod;
            ExamRoomExamPeriodDAO examRoomExamPeriodDAO = DynamicFilter(examRoomExamPeriodDAOs, filter).FirstOrDefault();

            return new ExamRoomExamPeriod()
            {
                ExamRoomId = examRoomExamPeriodDAO.ExamRoomId,
                ExamPeriodId = examRoomExamPeriodDAO.ExamPeriodId,
                ExamProgramName = examRoomExamPeriodDAO.ExamPeriod.ExamProgram.Name,
                ExamDate = examRoomExamPeriodDAO.ExamPeriod.ExamDate,
                StartHour = examRoomExamPeriodDAO.ExamPeriod.StartHour,
                FinishHour = examRoomExamPeriodDAO.ExamPeriod.FinishHour,
                SubjectName = examRoomExamPeriodDAO.ExamPeriod.Term.SubjectName,
                ExamRoomNumber = examRoomExamPeriodDAO.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = examRoomExamPeriodDAO.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = examRoomExamPeriodDAO.ExamRoom.ComputerNumber
            };
        }

        public async Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return new List<ExamRoomExamPeriod>();

            IQueryable<ExamRoomExamPeriodDAO> query = examRegContext.ExamRoomExamPeriod.AsNoTracking();
            query = DynamicFilter(query, filter);

            List<ExamRoomExamPeriod> list = await query.Select(s => new ExamRoomExamPeriod()
            {
                ExamRoomId = s.ExamRoomId,
                ExamPeriodId = s.ExamPeriodId,
                ExamProgramName = s.ExamPeriod.ExamProgram.Name,
                ExamDate = s.ExamPeriod.ExamDate,
                StartHour = s.ExamPeriod.StartHour,
                FinishHour = s.ExamPeriod.FinishHour,
                SubjectName = s.ExamPeriod.Term.SubjectName,
                ExamRoomNumber = s.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = s.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = s.ExamRoom.ComputerNumber
            }).ToListAsync();
            return list;
        }

        private IQueryable<ExamRoomExamPeriodDAO> DynamicFilter(IQueryable<ExamRoomExamPeriodDAO> query, ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.ExamProgramName != null)
                query = query.Where(q => q.ExamPeriod.ExamProgram.Name, filter.ExamProgramName);
            if (filter.SubjectName != null)
                query = query.Where(q => q.ExamPeriod.Term.SubjectName, filter.SubjectName);
            if (filter.ExamDate != null)
                query = query.Where(q => q.ExamPeriod.ExamDate, filter.ExamDate);
            if (filter.StartHour != null)
                query = query.Where(q => q.ExamPeriod.StartHour, filter.ExamDate);
            if (filter.FinishHour != null)
                query = query.Where(q => q.ExamPeriod.FinishHour, filter.ExamDate);
            if (filter.ExamRoomNumber != null)
                query = query.Where(q => q.ExamRoom.RoomNumber, filter.ExamRoomNumber);
            if (filter.ExamRoomAmphitheaterName != null)
                query = query.Where(q => q.ExamRoom.AmphitheaterName, filter.ExamRoomAmphitheaterName);
            return query;
        }
    }
}
