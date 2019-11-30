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
            SemesterDAO semesterDAO = new SemesterDAO()
            {
                Id = semester.Id,
                StartYear = semester.StartYear,
                EndYear = semester.EndYear,
                IsFirstHalf = semester.IsFirstHalf
            };

            await examRegContext.Semester.AddAsync(semesterDAO);
            await examRegContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Semester semester)
        {
            try
            {
                // ràng buộc (?)
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
                Code = string.Format(semesterDAO.StartYear + semesterDAO.EndYear + "_" + (semesterDAO.IsFirstHalf ? 1 : 2)),
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
                Code = string.Format(semesterDAO.StartYear + semesterDAO.EndYear + "_" + (semesterDAO.IsFirstHalf ? 1 : 2)),
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

            List<Semester> list = await query.Select(s => new Semester
            {
                Id = s.Id,
                Code = string.Format(s.StartYear + s.EndYear + "_" + (s.IsFirstHalf ? 1 : 2)),
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

        private IQueryable<SemesterDAO> DynamicFilter(IQueryable<SemesterDAO> query, SemesterFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            /*if(filter.IsFirstHalf.HasValue)
                query = query.Where(q => q.Id, filter.IsFirstHalf.HasValue);*/

            return query;
        }
    }
}
