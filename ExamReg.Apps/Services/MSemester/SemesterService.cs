using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MSemester
{
    public interface ISemesterService : IServiceScoped
    {
        Task<List<Semester>> List(SemesterFilter filter);
        Task<Semester> Create(Semester semester);
        Task<Semester> Delete(Semester semester);
    }
    public class SemesterService : ISemesterService
    {
        public async Task<Semester> Create(Semester semester)
        {
            throw new NotImplementedException();
        }

        public async Task<Semester> Delete(Semester semester)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Semester>> List(SemesterFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
