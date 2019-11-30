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
    public interface IStudentExamPeriodRepository
    {
        Task<StudentExamPeriod> Get(StudentExamPeriodFilter filter);
        //Task<bool> Create(StudentExamPeriod StudentExamPeriod);
        //Task<bool> Update(StudentExamPeriod StudentExamPeriod);
        //Task<bool> Delete(StudentExamPeriod StudentExamPeriod);
        //Task<List<StudentExamPeriod>> List(StudentExamPeriodFilter filter);
        Task<List<StudentExamPeriod>> List();
    }
    public class StudentExamPeriodRepository : IStudentExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        public StudentExamPeriodRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        /*public async Task<bool> Create(StudentExamPeriod studentExamPeriod)
        {
            StudentExamPeriodDAO studentExamPeriodDAO = examRegContext.StudentExamPeriod
                .Where(s => (s.StudentId.Equals(studentExamPeriod.StudentId) && s.ExamPeriodId.Equals(studentExamPeriod.ExamPeriodId)))
                .FirstOrDefault();
            if (studentExamPeriodDAO == null)
            {
                studentExamPeriodDAO = new StudentExamPeriodDAO()
                {
                    StudentId = studentExamPeriod.StudentId,
                    ExamPeriodId = studentExamPeriod.ExamPeriodId
                };

                await examRegContext.StudentExamPeriod.AddAsync(studentExamPeriodDAO);
            }
            else
            {
                //
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }*/

        /*public async Task<bool> Delete(StudentExamPeriod studentExamPeriod)
        {
            try
            {
                StudentExamPeriodDAO studentExamPeriodDAO = examRegContext.StudentExamPeriod
                    .Where(s => (s.StudentId.Equals(studentExamPeriod.StudentId) && s.ExamPeriodId.Equals(studentExamPeriod.ExamPeriodId)))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.StudentExamPeriod.Remove(studentExamPeriodDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/

        public async Task<StudentExamPeriod> Get(StudentExamPeriodFilter filter)
        {
            IQueryable<StudentExamPeriodDAO> studentExamPeriodDAOs = examRegContext.StudentExamPeriod;
            StudentExamPeriodDAO studentExamPeriodDAO = DynamicFilter(studentExamPeriodDAOs, filter).FirstOrDefault();

            return new StudentExamPeriod()
            {
                StudentId = studentExamPeriodDAO.StudentId,
                ExamPeriodId = studentExamPeriodDAO.ExamPeriodId
            };
        }
        public async Task<List<StudentExamPeriod>> List()
        {
            List<StudentExamPeriod> list = await examRegContext.StudentExamPeriod.Select(s => new StudentExamPeriod()
            {
                StudentId = s.StudentId,
                ExamPeriodId = s.ExamPeriodId
            }).ToListAsync();
            return list;
        }

        /*public async Task<List<StudentExamPeriod>> List(StudentExamPeriodFilter filter)
        {
            if (filter == null) return new List<StudentExamPeriod>();

            IQueryable<StudentExamPeriodDAO> query = examRegContext.StudentExamPeriod;
            query = DynamicFilter(query, filter);

            List<StudentExamPeriod> list = await query.Select(s => new StudentExamPeriod()
            {
                StudentId = s.StudentId,
                ExamPeriodId = s.ExamPeriodId

            }).ToListAsync();
            return list;

        }*/

        /*public async Task<bool> Update(StudentExamPeriod StudentExamPeriod)
        {
            await examRegContext.StudentExamPeriod
                .Where(s => (s.StudentId.Equals(StudentExamPeriod.StudentId) && s.ExamPeriodId.Equals(StudentExamPeriod.ExamPeriodId)))
                .UpdateFromQueryAsync(s => new StudentExamPeriodDAO()
                {
                    //
                });

            await examRegContext.SaveChangesAsync();
            return true;
        }*/
        private IQueryable<StudentExamPeriodDAO> DynamicFilter(IQueryable<StudentExamPeriodDAO> query, StudentExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            query = query.Where(q => q.StudentId, filter.StudentId);
            query = query.Where(q => q.ExamPeriodId, filter.ExamPeriodId);

            return query;
        }
    }
}
