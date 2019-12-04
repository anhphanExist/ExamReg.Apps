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
        Task<bool> Create(Guid studentId, Guid examPeriodId);
        Task<bool> Delete(Guid studentId, Guid examPeriodId);
        Task<List<StudentExamPeriod>> List(StudentExamPeriodFilter filter);
    }
    public class StudentExamPeriodRepository : IStudentExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public StudentExamPeriodRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> Create(Guid studentId, Guid examPeriodId)
        {
            examRegContext.StudentExamPeriod.Add(new StudentExamPeriodDAO
            {
                StudentId = studentId,
                ExamPeriodId = examPeriodId
            });
            examRegContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(Guid studentId, Guid examPeriodId)
        {
            examRegContext.StudentExamPeriod
                .Where(s => s.StudentId.Equals(studentId) && s.ExamPeriodId.Equals(examPeriodId))
                .DeleteFromQuery();
            return true;
        }

        public async Task<StudentExamPeriod> Get(StudentExamPeriodFilter filter)
        {
            IQueryable<StudentExamPeriodDAO> studentExamPeriodDAOs = examRegContext.StudentExamPeriod.AsNoTracking();
            StudentExamPeriodDAO studentExamPeriodDAO = DynamicFilter(studentExamPeriodDAOs, filter).FirstOrDefault();
            if (studentExamPeriodDAO == null)
                return null;
            return new StudentExamPeriod()
            {
                StudentId = studentExamPeriodDAO.StudentId,
                ExamPeriodId = studentExamPeriodDAO.ExamPeriodId
            };
            
        }

        public async Task<List<StudentExamPeriod>> List(StudentExamPeriodFilter filter)
        {
            if (filter == null) return new List<StudentExamPeriod>();

            IQueryable<StudentExamPeriodDAO> query = examRegContext.StudentExamPeriod.AsNoTracking();
            query = DynamicFilter(query, filter);

            List<StudentExamPeriod> list = await query.Select(s => new StudentExamPeriod()
            {
                StudentId = s.StudentId,
                ExamPeriodId = s.ExamPeriodId

            }).ToListAsync();
            return list;

        }

        private IQueryable<StudentExamPeriodDAO> DynamicFilter(IQueryable<StudentExamPeriodDAO> query, StudentExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.StudentId != null)
                query = query.Where(q => q.StudentId, filter.StudentId);
            if (filter.ExamPeriodId != null)
                query = query.Where(q => q.ExamPeriodId, filter.ExamPeriodId);

            return query;
        }
    }
}
