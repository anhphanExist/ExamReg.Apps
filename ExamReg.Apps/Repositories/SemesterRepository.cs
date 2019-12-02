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
    public interface ISemesterRepository
    {
        Task<Semester> Get(Guid Id);
        Task<Semester> Get(SemesterFilter filter);
        Task<bool> Create(Semester semester);
        Task<bool> Update(Semester semester);
        Task<bool> Delete(Semester semester);
        Task<int> Count(SemesterFilter filter);
        Task<List<Semester>> List(SemesterFilter filter);
        Task<List<Semester>> BulkInsert(List<Semester> semesters);
    }
    public class SemesterRepository : ISemesterRepository
    {
        private ExamRegContext examRegContext;
        public SemesterRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<int> Count(SemesterFilter filter)
        {
            IQueryable<SemesterDAO> semesterDAOs = examRegContext.Semester;
            semesterDAOs = DynamicFilter(semesterDAOs, filter);

            return await semesterDAOs.CountAsync();
        }

        public async Task<bool> Create(Semester semester)
        {
            SemesterDAO semesterDAO = examRegContext.Semester.Where(s => s.Id.Equals(semester.Id)).FirstOrDefault();
            if(semesterDAO == null)
            {
                semesterDAO = new SemesterDAO()
                {
                    Id = semester.Id,
                    StartYear = semester.StartYear,
                    EndYear = semester.EndYear,
                    IsFirstHalf = semester.IsFirstHalf
                };

                await examRegContext.Semester.AddAsync(semesterDAO);
            }
            else
            {
                semesterDAO.StartYear = semester.StartYear;
                semesterDAO.EndYear = semester.EndYear;
                semesterDAO.IsFirstHalf = semester.IsFirstHalf;
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Semester semester)
        {
            try
            {
                await examRegContext.Term
                .Where(t => t.SemesterId.Equals(semester.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                await examRegContext.ExamProgram
                .Where(t => t.SemesterId.Equals(semester.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                SemesterDAO semesterDAO = examRegContext.Semester
                    .Where(s => s.Id.Equals(semester.Id))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.Semester.Remove(semesterDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Semester> Get(Guid Id)
        {
            SemesterDAO semesterDAO = examRegContext.Semester
                .Where(s => s.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();

            return new Semester()
            {
                Id = semesterDAO.Id,
                Code = string.Format(semesterDAO.StartYear + "_" + semesterDAO.EndYear + "_" + (semesterDAO.IsFirstHalf ? 1 : 2)),
                StartYear = semesterDAO.StartYear,
                EndYear = semesterDAO.EndYear,
                IsFirstHalf = semesterDAO.IsFirstHalf
            };
        }

        public async Task<Semester> Get(SemesterFilter filter)
        {
            IQueryable<SemesterDAO> query = examRegContext.Semester;
            SemesterDAO semesterDAO = DynamicFilter(query, filter).FirstOrDefault();

            return new Semester()
            {
                Id = semesterDAO.Id,
                Code = string.Format(semesterDAO.StartYear + "_" + semesterDAO.EndYear + "_" + (semesterDAO.IsFirstHalf ? 1 : 2)),
                StartYear = semesterDAO.StartYear,
                EndYear = semesterDAO.EndYear,
                IsFirstHalf = semesterDAO.IsFirstHalf
            };
        }

        public async Task<List<Semester>> List(SemesterFilter filter)
        {
            if (filter == null) return new List<Semester>();

            IQueryable<SemesterDAO> query = examRegContext.Semester;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<Semester> list = await query.Select(s => new Semester
            {
                Id = s.Id,
                Code = string.Format(s.StartYear + "_" + s.EndYear + "_" + (s.IsFirstHalf ? 1 : 2)),
                StartYear = s.StartYear,
                EndYear = s.EndYear,
                IsFirstHalf = s.IsFirstHalf
            }).ToListAsync();
            return list;

        }

        public async Task<bool> Update(Semester semester)
        {
            await examRegContext.Semester.Where(s => s.Id.Equals(semester.Id)).UpdateFromQueryAsync(s => new SemesterDAO()
            {
                Id = semester.Id,
                StartYear = semester.StartYear,
                EndYear = semester.EndYear,
                IsFirstHalf = semester.IsFirstHalf
            });
            return true;
        }

        public async Task<List<Semester>> BulkInsert(List<Semester> semesters)
        {
            throw new NotImplementedException();
        }

        private IQueryable<SemesterDAO> DynamicFilter(IQueryable<SemesterDAO> query, SemesterFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.Code != null)
            {
                string[] codeData = filter.Code.Equal.Split(".");
                query = query.Where(q => q.StartYear, new ShortFilter { Equal = short.Parse(codeData[0]) });
                query = query.Where(q => q.EndYear, new ShortFilter { Equal = short.Parse(codeData[1]) });
                query = query.Where(q => q.IsFirstHalf == (codeData[2] == "1" ? true : false));
            }
            if (filter.IsFirstHalf != null)
                query = query.Where(c => c.IsFirstHalf == filter.IsFirstHalf);      

            return query;
        }

        private IQueryable<SemesterDAO> DynamicOrder(IQueryable<SemesterDAO> query, SemesterFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case SemesterOrder.Code:
                            query = query.OrderBy(q => q.StartYear);
                            break;
                        case SemesterOrder.IsFirstHalf:
                            query = query.OrderBy(q => q.IsFirstHalf);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case SemesterOrder.Code:
                            query = query.OrderByDescending(q => q.StartYear);
                            break;
                        case SemesterOrder.IsFirstHalf:
                            query = query.OrderByDescending(q => q.IsFirstHalf);
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
