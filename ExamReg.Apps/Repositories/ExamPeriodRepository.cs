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
        Task<bool> Delete(ExamPeriod examPeriod);
        Task<int> Count(ExamPeriodFilter filter);
        Task<List<ExamPeriod>> List(ExamPeriodFilter filter);
    }
    public class ExamPeriodRepository : IExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        public ExamPeriodRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<int> Count(ExamPeriodFilter filter)
        {
            IQueryable<ExamPeriodDAO> examPeriodDAOs = examRegContext.ExamPeriod;
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

        public async Task<bool> Delete(ExamPeriod examPeriod)
        {
            try
            {
                await examRegContext.StudentExamPeriod
                .Where(s => s.ExamPeriodId.Equals(examPeriod.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                await examRegContext.ExamRoomExamPeriod
                .Where(s => s.ExamPeriodId.Equals(examPeriod.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                ExamPeriodDAO examPeriodDAO = examRegContext.ExamPeriod
                    .Where(e => e.Id.Equals(examPeriod.Id))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.ExamPeriod.Remove(examPeriodDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ExamPeriod> Get(Guid Id)
        {
            ExamPeriodDAO examPeriodDAO = examRegContext.ExamPeriod
                .Where(t => t.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new ExamPeriod()
            {
                Id = examPeriodDAO.Id,
                ExamDate = examPeriodDAO.ExamDate,
                StartHour = examPeriodDAO.StartHour,
                FinishHour = examPeriodDAO.FinishHour,
                TermId = examPeriodDAO.TermId,
                ExamProgramId = examPeriodDAO.ExamProgramId
            };
        }

        public async Task<ExamPeriod> Get(ExamPeriodFilter filter)
        {
            IQueryable<ExamPeriodDAO> examPeriodDAOs = examRegContext.ExamPeriod;
            ExamPeriodDAO examPeriodDAO = DynamicFilter(examPeriodDAOs, filter).FirstOrDefault();

            return new ExamPeriod()
            {
                Id = examPeriodDAO.Id,
                ExamDate = examPeriodDAO.ExamDate,
                StartHour = examPeriodDAO.StartHour,
                FinishHour = examPeriodDAO.FinishHour,
                TermId = examPeriodDAO.TermId,
                ExamProgramId = examPeriodDAO.ExamProgramId
            };
        }

        public async Task<List<ExamPeriod>> List(ExamPeriodFilter filter)
        {
            if (filter == null) return new List<ExamPeriod>();
            IQueryable<ExamPeriodDAO> query = examRegContext.ExamPeriod;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<ExamPeriod> list = await query.Select(e => new ExamPeriod()
            {
                Id = e.Id,
                ExamDate = e.ExamDate,
                StartHour = e.StartHour,
                FinishHour = e.FinishHour,
                TermId = e.TermId,
                ExamProgramId = e.ExamProgramId
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(ExamPeriod examPeriod)
        {
            await examRegContext.ExamPeriod.Where(t => t.Id.Equals(examPeriod.Id)).UpdateFromQueryAsync(t => new ExamPeriodDAO
            {
                ExamDate = examPeriod.ExamDate,
                StartHour = examPeriod.StartHour,
                FinishHour = examPeriod.FinishHour,
                TermId = examPeriod.TermId,
                ExamProgramId = examPeriod.ExamProgramId
            });

            await examRegContext.SaveChangesAsync();
            return true;
        }
        private IQueryable<ExamPeriodDAO> DynamicFilter(IQueryable<ExamPeriodDAO> query, ExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            query = query.Where(q => q.TermId, filter.TermId);
            query = query.Where(q => q.ExamProgramId, filter.ExamProgramId);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.ExamDate != null)
                query = query.Where(q => q.ExamDate, filter.ExamDate);

            return query;
        }

        private IQueryable<ExamPeriodDAO> DynamicOrder(IQueryable<ExamPeriodDAO> query, ExamPeriodFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ExamPeriodOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ExamPeriodOrder.ExamDate:
                            query = query.OrderBy(q => q.ExamDate);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ExamPeriodOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ExamPeriodOrder.ExamDate:
                            query = query.OrderByDescending(q => q.ExamDate);
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
