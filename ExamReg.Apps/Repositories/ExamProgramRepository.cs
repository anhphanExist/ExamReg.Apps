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
        Task<bool> Delete(Guid Id);
        Task<int> Count(ExamProgramFilter filter);
        Task<List<ExamProgram>> List(ExamProgramFilter filter);
        Task<bool> Active(Guid Id);
    }
    public class ExamProgramRepository : IExamProgramRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public ExamProgramRepository(ExamRegContext examReg, ICurrentContext currentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = currentContext;
        }
        public async Task<int> Count(ExamProgramFilter filter)
        {
            IQueryable<ExamProgramDAO> examProgramDAOs = examRegContext.ExamProgram.AsNoTracking();
            examProgramDAOs = DynamicFilter(examProgramDAOs, filter);
            return await examProgramDAOs.CountAsync();
        }

        public async Task<bool> Create(ExamProgram examProgram)
        {
            ExamProgramDAO examProgramDAO = new ExamProgramDAO()
            {
                Id = examProgram.Id,
                Name = examProgram.Name,
                SemesterId = examProgram.SemesterId,
                IsCurrent = false
            };
            await examRegContext.ExamProgram.AddAsync(examProgramDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid Id)
        {
            await examRegContext.ExamPeriod
            .Where(t => t.ExamProgramId.Equals(Id))
            .DeleteFromQueryAsync();

            ExamProgramDAO examProgramDAO = examRegContext.ExamProgram
                .Where(e => e.Id.Equals(Id))
                .FirstOrDefault();

            examRegContext.ExamProgram.Remove(examProgramDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<ExamProgram> Get(Guid Id)
        {
            ExamProgramDAO examProgramDAO = examRegContext.ExamProgram
                .AsNoTracking()
                .Where(t => t.Id.Equals(Id))
                .FirstOrDefault();
            return new ExamProgram()
            {
                Id = examProgramDAO.Id,
                Name = examProgramDAO.Name,
                SemesterId = examProgramDAO.SemesterId,
                SemesterCode = string.Format(examProgramDAO.Semester.StartYear + "_" + examProgramDAO.Semester.EndYear + "_" + (examProgramDAO.Semester.IsFirstHalf ? 1 : 2)),
                IsCurrent = examProgramDAO.IsCurrent
            };
        }

        public async Task<ExamProgram> Get(ExamProgramFilter filter)
        {
            IQueryable<ExamProgramDAO> query = examRegContext.ExamProgram.AsNoTracking();
            ExamProgramDAO examProgramDAO = DynamicFilter(query, filter).FirstOrDefault();

            return new ExamProgram()
            {
                Id = examProgramDAO.Id,
                Name = examProgramDAO.Name,
                SemesterId = examProgramDAO.SemesterId,
                SemesterCode = string.Format(examProgramDAO.Semester.StartYear + "_" + examProgramDAO.Semester.EndYear + "_" + (examProgramDAO.Semester.IsFirstHalf ? 1 : 2)),
                IsCurrent = examProgramDAO.IsCurrent
            };
        }

        public async Task<List<ExamProgram>> List(ExamProgramFilter filter)
        {
            if (filter == null) return new List<ExamProgram>();
            IQueryable<ExamProgramDAO> query = examRegContext.ExamProgram.AsNoTracking();
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<ExamProgram> list = await query.Select(e => new ExamProgram()
            {
                Id = e.Id,
                Name = e.Name,
                SemesterId = e.SemesterId,
                SemesterCode = string.Format(e.Semester.StartYear + "_" + e.Semester.EndYear + "_" + (e.Semester.IsFirstHalf ? 1 : 2)),
                IsCurrent = e.IsCurrent
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(ExamProgram examProgram)
        {
            await examRegContext.ExamProgram.Where(t => t.Id.Equals(examProgram.Id)).UpdateFromQueryAsync(t => new ExamProgramDAO
            {
                Name = examProgram.Name,
                SemesterId = examProgram.SemesterId,
                IsCurrent = examProgram.IsCurrent
            });

            return true;
        }
        
        public async Task<bool> Active(Guid Id)
        {
            await examRegContext.ExamProgram
                .Where(e => e.IsCurrent.Equals(true))
                .UpdateFromQueryAsync(e => new ExamProgramDAO { IsCurrent = false });

            await examRegContext.ExamProgram
                .Where(e => e.Id.Equals(Id))
                .UpdateFromQueryAsync(e => new ExamProgramDAO { IsCurrent = true });

            return true;
        }
        private IQueryable<ExamProgramDAO> DynamicFilter(IQueryable<ExamProgramDAO> query, ExamProgramFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.Name != null)
                query = query.Where(q => q.Name, filter.Name);
            if (filter.SemesterCode != null)
            {
                string[] codeData = filter.SemesterCode.Equal.Split("_");
                query = query.Where(q => q.Semester.StartYear, new ShortFilter { Equal = short.Parse(codeData[0]) });
                query = query.Where(q => q.Semester.EndYear, new ShortFilter { Equal = short.Parse(codeData[1]) });
                query = query.Where(q => q.Semester.IsFirstHalf == (codeData[2] == "1" ? true : false));
            }
            if (filter.IsCurrent.HasValue)
                query = query.Where(q => q.IsCurrent.Equals(filter.IsCurrent.Value));
            return query;
        }

        private IQueryable<ExamProgramDAO> DynamicOrder(IQueryable<ExamProgramDAO> query, ExamProgramFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ExamProgramOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ExamProgramOrder.SemesterCode:
                            query = query.OrderBy(q => q.Semester.StartYear);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ExamProgramOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ExamProgramOrder.SemesterCode:
                            query = query.OrderByDescending(q => q.Semester.StartYear);
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
