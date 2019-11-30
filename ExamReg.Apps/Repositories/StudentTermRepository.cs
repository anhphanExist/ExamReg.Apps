using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IStudentStudentTermRepository
    {
        Task<StudentTerm> Get(Guid Id);
        Task<StudentTerm> Get(StudentTermFilter filter);
        Task<bool> Create(StudentTerm studentTerm);
        Task<bool> Update(StudentTerm studentTerm);
        Task<bool> Delete(StudentTerm studentTerm);
        Task<int> Count(StudentTermFilter filter);
        Task<List<StudentTerm>> List(StudentTermFilter filter);
        Task<bool> BulkMerge(List<StudentTerm> studentTerms);
    }
    public class StudentStudentTermRepository : IStudentStudentTermRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public StudentStudentTermRepository(ExamRegContext examRegContext, ICurrentContext CurrentContext)
        {
            this.examRegContext = examRegContext;
            this.CurrentContext = CurrentContext;
        }
        public async Task<StudentTerm> Get(Guid Id)
        {
            throw new NotImplementedException();
        }
        public async Task<StudentTerm> Get(StudentTermFilter filter)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Create(StudentTerm studentTerm)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Update(StudentTerm studentTerm)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Delete(StudentTerm studentTerm)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Count(StudentTermFilter filter)
        {
            throw new NotImplementedException();
        }
        public async Task<List<StudentTerm>> List(StudentTermFilter filter)
        {
            throw new NotImplementedException();
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
    }
}
