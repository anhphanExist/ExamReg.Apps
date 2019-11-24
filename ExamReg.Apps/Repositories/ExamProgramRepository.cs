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
    public interface IExamProgramRepository
    {
        Task<ExamProgram> Get(Guid Id);
        Task<ExamProgram> Get(ExamProgramFilter filter);
        Task<bool> Create(ExamProgram examProgram);
        Task<bool> Update(ExamProgram examProgram);
        Task<bool> Delete(ExamProgram examProgram);
        Task<int> Count(ExamProgramFilter filter);
        Task<List<ExamProgram>> List(ExamProgramFilter filter);
    }
    public class ExamProgramRepository : IExamProgramRepository
    {
        private ExamRegContext examRegContext;
        public ExamProgramRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<int> Count(ExamProgramFilter filter)
        {
            IQueryable<ExamProgramDAO> examProgramDAOs = examRegContext.ExamProgram;
            examProgramDAOs = DynamicFilter(examProgramDAOs, filter);
            return await examProgramDAOs.CountAsync();
        }

        public async Task<bool> Create(ExamProgram examProgram)
        {
            ExamProgramDAO examProgramDAO = examRegContext.ExamProgram.Where(e => e.Id.Equals(examProgram.Id)).FirstOrDefault();
            if(examProgramDAO == null)
            {
                examProgramDAO = new ExamProgramDAO()
                {
                    Id = examProgram.Id,
                    Name = examProgram.Name,
                    SemesterId = examProgram.SemesterId
                };
                await examRegContext.ExamProgram.AddAsync(examProgramDAO);
            }
            else
            {
                examProgramDAO.Id = examProgram.Id;
                examProgramDAO.Name = examProgram.Name;
                examProgramDAO.SemesterId = examProgram.SemesterId;
            };

            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(ExamProgram examProgram)
        {
            try
            {
                await examRegContext.ExamPeriod
                .Where(t => t.ExamProgramId.Equals(examProgram.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                ExamProgramDAO examProgramDAO = examRegContext.ExamProgram
                    .Where(e => e.Id.Equals(examProgram.Id))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.ExamProgram.Remove(examProgramDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ExamProgram> Get(Guid Id)
        {
            ExamProgramDAO examProgramDAO = examRegContext.ExamProgram
                .Where(t => t.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new ExamProgram()
            {
                Id = examProgramDAO.Id,
                Name = examProgramDAO.Name,
                SemesterId = examProgramDAO.SemesterId
            };
        }

        public async Task<ExamProgram> Get(ExamProgramFilter filter)
        {
            IQueryable<ExamProgramDAO> query = examRegContext.ExamProgram;
            ExamProgramDAO examProgramDAO = DynamicFilter(query, filter).FirstOrDefault();

            return new ExamProgram()
            {
                Id = examProgramDAO.Id,
                Name = examProgramDAO.Name,
                SemesterId = examProgramDAO.SemesterId
            };
        }

        public async Task<List<ExamProgram>> List(ExamProgramFilter filter)
        {
            if (filter == null) return new List<ExamProgram>();
            IQueryable<ExamProgramDAO> query = examRegContext.ExamProgram;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<ExamProgram> list = await query.Select(e => new ExamProgram()
            {
                Id = e.Id,
                Name = e.Name,
                SemesterId = e.SemesterId
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(ExamProgram examProgram)
        {
            await examRegContext.ExamProgram.Where(t => t.Id.Equals(examProgram.Id)).UpdateFromQueryAsync(t => new ExamProgramDAO
            {
                Id = examProgram.Id,
                Name = examProgram.Name,
                SemesterId = examProgram.SemesterId
            });

            await examRegContext.SaveChangesAsync();
            return true;
        }
        private IQueryable<ExamProgramDAO> DynamicFilter(IQueryable<ExamProgramDAO> query, ExamProgramFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            query = query.Where(q => q.SemesterId, filter.SemesterId);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.Name != null)
                query = query.Where(q => q.Name, filter.Name);

            return query;
        }

        private IQueryable<ExamProgramDAO> DynamicOrder(IQueryable<ExamProgramDAO> query, ExamProgramFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ExamProgramOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ExamProgramOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ExamProgramOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ExamProgramOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
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
