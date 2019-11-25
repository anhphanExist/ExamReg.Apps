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
        //Task<bool> Create(ExamRoomExamPeriod ExamRoomExamPeriod);
        //Task<bool> Update(ExamRoomExamPeriod ExamRoomExamPeriod);
        //Task<bool> Delete(ExamRoomExamPeriod ExamRoomExamPeriod);
        //Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);
        Task<List<ExamRoomExamPeriod>> List();
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
            try
            {
                ExamRoomExamPeriodDAO ExamRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                    .Where(s => (s.ExamRoomId.Equals(ExamRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(ExamRoomExamPeriod.ExamPeriodId)))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.ExamRoomExamPeriod.Remove(ExamRoomExamPeriodDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter)
        {
            IQueryable<ExamRoomExamPeriodDAO> ExamRoomExamPeriodDAOs = examRegContext.ExamRoomExamPeriod;
            ExamRoomExamPeriodDAO ExamRoomExamPeriodDAO = DynamicFilter(ExamRoomExamPeriodDAOs, filter).FirstOrDefault();

            return new ExamRoomExamPeriod()
            {
                ExamRoomId = ExamRoomExamPeriodDAO.ExamRoomId,
                ExamPeriodId = ExamRoomExamPeriodDAO.ExamPeriodId
            };
        }
        public async Task<List<ExamRoomExamPeriod>> List()
        {
            List<ExamRoomExamPeriod> list = await examRegContext.ExamRoomExamPeriod.Select(s => new ExamRoomExamPeriod()
            {
                ExamRoomId = s.ExamRoomId,
                ExamPeriodId = s.ExamPeriodId
            }).ToListAsync();
            return list;
        }

        public async Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            if (filter == null) return new List<ExamRoomExamPeriod>();

            IQueryable<ExamRoomExamPeriodDAO> query = examRegContext.ExamRoomExamPeriod;
            query = DynamicFilter(query, filter);

            List<ExamRoomExamPeriod> list = await query.Select(s => new ExamRoomExamPeriod()
            {
                ExamRoomId = s.ExamRoomId,
                ExamPeriodId = s.ExamPeriodId

            }).ToListAsync();
            return list;

        }

        public async Task<bool> Update(ExamRoomExamPeriod ExamRoomExamPeriod)
        {
            await examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(ExamRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(ExamRoomExamPeriod.ExamPeriodId)))
                .UpdateFromQueryAsync(s => new ExamRoomExamPeriodDAO()
                {
                    //
                });

            await examRegContext.SaveChangesAsync();
            return true;
        }
        private IQueryable<ExamRoomExamPeriodDAO> DynamicFilter(IQueryable<ExamRoomExamPeriodDAO> query, ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            query = query.Where(q => q.ExamRoomId, filter.ExamRoomId);
            query = query.Where(q => q.ExamPeriodId, filter.ExamPeriodId);

            return query;
        }
    }
}
