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
    public interface IStudentTermRepository
    {
        Task<StudentTerm> Get(StudentTermFilter filter);
        Task<bool> Create(StudentTerm studentTerm);
        Task<bool> Update(StudentTerm studentTerm);
        Task<bool> Delete(StudentTerm studentTerm);
        Task<List<StudentTerm>> List(StudentTermFilter filter);
    }
    public class StudentTermRepository : IStudentTermRepository
    {
        private ExamRegContext examRegContext;
        public StudentTermRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<bool> Create(StudentTerm studentTerm)
        {
            StudentTermDAO studentTermDAO = examRegContext.StudentTerm
                .Where(s => (s.StudentId.Equals(studentTerm.StudentId) && s.TermId.Equals(studentTerm.TermId)))
                .FirstOrDefault();
            if (studentTermDAO == null)
            {
                studentTermDAO = new StudentTermDAO()
                {
                    StudentId = studentTerm.StudentId,
                    TermId = studentTerm.TermId,
                    IsQualified = studentTerm.IsQualified
                };

                await examRegContext.StudentTerm.AddAsync(studentTermDAO);
            }
            else
            {
                studentTermDAO.IsQualified = studentTerm.IsQualified;
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(StudentTerm studentTerm)
        {
            try
            {
                StudentTermDAO studentTermDAO = examRegContext.StudentTerm
                    .Where(s => (s.StudentId.Equals(studentTerm.StudentId) && s.TermId.Equals(studentTerm.TermId)))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.StudentTerm.Remove(studentTermDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<StudentTerm> Get(StudentTermFilter filter)
        {
            IQueryable<StudentTermDAO> studentTermDAOs = examRegContext.StudentTerm;
            StudentTermDAO studentTermDAO = DynamicFilter(studentTermDAOs, filter).FirstOrDefault();

            return new StudentTerm()
            {
                StudentId = studentTermDAO.StudentId,
                TermId = studentTermDAO.TermId,
                IsQualified = studentTermDAO.IsQualified
            };
        }

        public async Task<List<StudentTerm>> List(StudentTermFilter filter)
        {
            if (filter == null) return new List<StudentTerm>();

            IQueryable<StudentTermDAO> query = examRegContext.StudentTerm;
            query = DynamicFilter(query, filter);

            List<StudentTerm> list = await query.Select(s => new StudentTerm()
            {
                StudentId = s.StudentId,
                TermId = s.TermId,
                IsQualified = s.IsQualified

            }).ToListAsync();
            return list;

        }

        public async Task<bool> Update(StudentTerm studentTerm)
        {
            await examRegContext.StudentTerm
                .Where(s => (s.StudentId.Equals(studentTerm.StudentId) && s.TermId.Equals(studentTerm.TermId)))
                .UpdateFromQueryAsync(s => new StudentTermDAO()
            {
                IsQualified = studentTerm.IsQualified
            });

            await examRegContext.SaveChangesAsync();
            return true;
        }
        private IQueryable<StudentTermDAO> DynamicFilter(IQueryable<StudentTermDAO> query, StudentTermFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.StudentId != null)
                query = query.Where(q => q.StudentId, filter.StudentId);
            if (filter.TermId != null)
                query = query.Where(q => q.TermId, filter.TermId);
            //if (filter.IsQualified != null)
                query = query.Where(c => c.IsQualified == filter.IsQualified);

            return query;
        }
    }
}
