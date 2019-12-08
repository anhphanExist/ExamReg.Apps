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
    public interface IExamPeriodRepository
    {
        Task<ExamPeriod> Get(Guid Id);
        Task<ExamPeriod> Get(ExamPeriodFilter filter);
        Task<bool> Create(ExamPeriod examPeriod);
        Task<bool> Update(ExamPeriod examPeriod);
        Task<bool> Delete(Guid Id);
        Task<int> Count(ExamPeriodFilter filter);
        Task<List<ExamPeriod>> List(ExamPeriodFilter filter);
    }
    public class ExamPeriodRepository : IExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public ExamPeriodRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(ExamPeriodFilter filter)
        {
            IQueryable<ExamPeriodDAO> examPeriodDAOs = examRegContext.ExamPeriod.AsNoTracking();
            examPeriodDAOs = DynamicFilter(examPeriodDAOs, filter);
            return await examPeriodDAOs.CountAsync();
        }

        public async Task<bool> Create(ExamPeriod examPeriod)
        {
            ExamPeriodDAO examPeriodDAO = new ExamPeriodDAO()
            {
                Id = examPeriod.Id,
                ExamDate = examPeriod.ExamDate,
                StartHour = examPeriod.StartHour,
                FinishHour = examPeriod.FinishHour,
                TermId = examPeriod.TermId,
                ExamProgramId = examPeriod.ExamProgramId
            };
            await examRegContext.ExamPeriod.AddAsync(examPeriodDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid Id)
        {
            await examRegContext.ExamRoomExamPeriod
            .Where(s => s.ExamPeriodId.Equals(Id))
            .DeleteFromQueryAsync();

            ExamPeriodDAO examPeriodDAO = examRegContext.ExamPeriod
                .Where(e => e.Id.Equals(Id))
                .FirstOrDefault();

            examRegContext.ExamPeriod.Remove(examPeriodDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<ExamPeriod> Get(Guid Id)
        {
            ExamPeriodDAO examPeriodDAO = examRegContext.ExamPeriod
                .AsNoTracking()
                .Where(t => t.Id.Equals(Id))
                .FirstOrDefault();
            if (examPeriodDAO == null)
                return null;
            return new ExamPeriod()
            {
                Id = examPeriodDAO.Id,
                ExamDate = examPeriodDAO.ExamDate,
                StartHour = examPeriodDAO.StartHour,
                FinishHour = examPeriodDAO.FinishHour,
                TermId = examPeriodDAO.TermId,
                SubjectName = examPeriodDAO.Term.SubjectName,
                ExamProgramId = examPeriodDAO.ExamProgramId,
                ExamProgramName = examPeriodDAO.ExamProgram.Name
            };
        }

        public async Task<ExamPeriod> Get(ExamPeriodFilter filter)
        {
            IQueryable<ExamPeriodDAO> examPeriodDAOs = examRegContext.ExamPeriod.AsNoTracking();
            ExamPeriodDAO examPeriodDAO = DynamicFilter(examPeriodDAOs, filter).FirstOrDefault();
            if (examPeriodDAO == null)
                return null;
            return new ExamPeriod()
            {
                Id = examPeriodDAO.Id,
                ExamDate = examPeriodDAO.ExamDate,
                StartHour = examPeriodDAO.StartHour,
                FinishHour = examPeriodDAO.FinishHour,
                TermId = examPeriodDAO.TermId,
                SubjectName = examPeriodDAO.Term.SubjectName,
                ExamProgramId = examPeriodDAO.ExamProgramId,
                ExamProgramName = examPeriodDAO.ExamProgram.Name
            };
        }

        public async Task<List<ExamPeriod>> List(ExamPeriodFilter filter)
        {
            if (filter == null)
                return new List<ExamPeriod>();
            IQueryable<ExamPeriodDAO> query = examRegContext.ExamPeriod.AsNoTracking();
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<ExamPeriod> list = await query.Select(e => new ExamPeriod()
            {
                Id = e.Id,
                ExamDate = e.ExamDate,
                StartHour = e.StartHour,
                FinishHour = e.FinishHour,
                TermId = e.TermId,
                SubjectName = e.Term.SubjectName,
                ExamProgramId = e.ExamProgramId,
                ExamProgramName = e.ExamProgram.Name
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(ExamPeriod examPeriod)
        {
            // Xoá ExamRegister của ExamRoomExamPeriod của ExamPeriod
            await examRegContext.ExamRoomExamPeriod
                .Where(e => e.ExamPeriodId == examPeriod.Id)
                .DeleteFromQueryAsync();
            await examRegContext.ExamRegister
                .Where(e => e.ExamPeriodId == examPeriod.Id)
                .DeleteFromQueryAsync();

            // Thêm lại ExamRoomExamPeriod
            await examRegContext.ExamRoomExamPeriod.BulkInsertAsync(examPeriod.ExamRooms.Select(e => new ExamRoomExamPeriodDAO
            {
                ExamPeriodId = examPeriod.Id,
                ExamRoomId = e.Id
            }).ToList());

            return true;
        }
        private IQueryable<ExamPeriodDAO> DynamicFilter(IQueryable<ExamPeriodDAO> query, ExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.StudentNumber != null)
                query = query.Where(q => q.ExamRoomExamPeriods.Select(e => e.ExamRegisters.Select(r => r.Student.StudentNumber)), filter.StudentNumber);
                // có thể dùng join vào với nhau để đạt performance cao hơn
            if (filter.ExamDate != null)
                query = query.Where(q => q.ExamDate, filter.ExamDate);
            if (filter.SubjectName != null)
                query = query.Where(q => q.Term.SubjectName, filter.SubjectName);
            if (filter.ExamProgramId != null)
                query = query.Where(q => q.ExamProgramId, filter.ExamProgramId);
            if (filter.ExamProgramName != null)
                query = query.Where(q => q.ExamProgram.Name, filter.ExamProgramName);
            return query;
        }

        private IQueryable<ExamPeriodDAO> DynamicOrder(IQueryable<ExamPeriodDAO> query, ExamPeriodFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ExamPeriodOrder.ExamDate:
                            query = query.OrderBy(q => q.ExamDate);
                            break;
                        case ExamPeriodOrder.SubjectName:
                            query = query.OrderBy(q => q.Term.SubjectName);
                            break;
                        case ExamPeriodOrder.ExamProgramName:
                            query = query.OrderBy(q => q.ExamProgram.Name);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ExamPeriodOrder.ExamDate:
                            query = query.OrderByDescending(q => q.ExamDate);
                            break;
                        case ExamPeriodOrder.SubjectName:
                            query = query.OrderByDescending(q => q.Term.SubjectName);
                            break;
                        case ExamPeriodOrder.ExamProgramName:
                            query = query.OrderByDescending(q => q.ExamProgram.Name);
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
