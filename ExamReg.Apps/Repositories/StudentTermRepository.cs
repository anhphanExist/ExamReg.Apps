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
        Task<bool> Delete(Guid studentId, Guid TermId);
        Task<List<StudentTerm>> List(StudentTermFilter filter);
        Task<bool> BulkMerge(List<StudentTerm> studentTerms);
    }
    public class StudentTermRepository : IStudentTermRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public StudentTermRepository(ExamRegContext examRegContext, ICurrentContext CurrentContext)
        {
            this.examRegContext = examRegContext;
            this.CurrentContext = CurrentContext;
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

        public async Task<bool> Delete(Guid studentId, Guid TermId)
        {
            try
            {
                StudentTermDAO studentTermDAO = examRegContext.StudentTerm
                    .Where(s => (s.StudentId.Equals(studentId) && s.TermId.Equals(TermId)))
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
            if (filter == null)
                return null;

            IQueryable<StudentTermDAO> query = examRegContext.StudentTerm.AsNoTracking();
            query = DynamicFilter(query, filter);

            List<StudentTerm> list = await query.Select(s => new StudentTerm()
            {
                StudentId = s.StudentId,
                TermId = s.TermId,
                IsQualified = s.IsQualified,
                SubjectName = s.Term.SubjectName,
                StudentGivenName = s.Student.GivenName,
                StudentLastName = s.Student.LastName,
                StudentNumber = s.Student.StudentNumber
            }).ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<List<StudentTerm>> List(StudentTermFilter filter)
        {
            if (filter == null) return new List<StudentTerm>();

            IQueryable<StudentTermDAO> query = examRegContext.StudentTerm.AsNoTracking();
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<StudentTerm> list = await query.Select(s => new StudentTerm()
            {
                StudentId = s.StudentId,
                TermId = s.TermId,
                IsQualified = s.IsQualified,
                SubjectName = s.Term.SubjectName,
                StudentGivenName = s.Student.GivenName,
                StudentLastName = s.Student.LastName,
                StudentNumber = s.Student.StudentNumber
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

        public async Task<bool> BulkMerge(List<StudentTerm> studentTerms)
        {
            List<StudentTermDAO> studentTermDAOs = studentTerms.Select(st => new StudentTermDAO
            {
                StudentId = st.StudentId,
                TermId = st.TermId,
                IsQualified = st.IsQualified
            }).ToList();

            await examRegContext.StudentTerm.BulkMergeAsync(studentTermDAOs, options =>
            {
                options.IgnoreOnUpdateExpression = column => new
                {
                    column.TermId,
                    column.StudentId
                };
            });

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
            if (filter.IsQualified != null)
                query = query.Where(c => c.IsQualified == filter.IsQualified);

            return query;
        }

        private IQueryable<StudentTermDAO> DynamicOrder(IQueryable<StudentTermDAO> query, StudentTermFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StudentTermOrder.StudentNumber:
                            query = query.OrderBy(q => q.Student.StudentNumber);
                            break;
                        case StudentTermOrder.SubjectName:
                            query = query.OrderBy(q => q.Term.SubjectName);
                            break;
                        case StudentTermOrder.StudentLastName:
                            query = query.OrderBy(q => q.Student.LastName);
                            break;
                        case StudentTermOrder.StudentGivenName:
                            query = query.OrderBy(q => q.Student.GivenName);
                            break;
                        case StudentTermOrder.IsQualified:
                            query = query.OrderBy(q => q.IsQualified);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StudentTermOrder.StudentNumber:
                            query = query.OrderByDescending(q => q.Student.StudentNumber);
                            break;
                        case StudentTermOrder.SubjectName:
                            query = query.OrderByDescending(q => q.Term.SubjectName);
                            break;
                        case StudentTermOrder.StudentLastName:
                            query = query.OrderByDescending(q => q.Student.LastName);
                            break;
                        case StudentTermOrder.StudentGivenName:
                            query = query.OrderByDescending(q => q.Student.GivenName);
                            break;
                        case StudentTermOrder.IsQualified:
                            query = query.OrderByDescending(q => q.IsQualified);
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
