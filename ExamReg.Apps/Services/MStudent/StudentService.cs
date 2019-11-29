using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MStudent
{
    public interface IStudentService : IServiceScoped
    {
        Task<int> Count(StudentFilter filter);
        Task<List<Student>> List(StudentFilter filter);
        Task<Student> Create(Student student);
        Task<Student> Update(Student student);
        Task<Student> Delete(Student student);
        Task<Student> BulkInsert(byte[] file);
    }
    public class StudentService : IStudentService
    {
        
        public Task<Student> BulkInsert(byte[] file)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count(StudentFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Create(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Delete(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> List(StudentFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Student> Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
