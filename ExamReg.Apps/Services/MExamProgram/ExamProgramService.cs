using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamProgram
{
    public interface IExamProgramService : IServiceScoped
    {
        Task<int> Count(ExamProgramFilter filter);
        Task<List<ExamProgram>> List(ExamProgramFilter filter);
        Task<ExamProgram> Create(ExamProgram examProgram);
        Task<ExamProgram> Update(ExamProgram examProgram);
        Task<ExamProgram> Delete(ExamProgram examProgram);
        Task<ExamProgram> SetCurrentExamProgram(ExamProgram examProgram);
    }
    public class ExamProgramService : IExamProgramService
    {
        public Task<int> Count(ExamProgramFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamProgram> Create(ExamProgram examProgram)
        {
            throw new NotImplementedException();
        }

        public Task<ExamProgram> Delete(ExamProgram examProgram)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExamProgram>> List(ExamProgramFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamProgram> Update(ExamProgram examProgram)
        {
            throw new NotImplementedException();
        }

        public Task<ExamProgram> SetCurrentExamProgram(ExamProgram examProgram)
        {
            throw new NotImplementedException();
        }
    }
}
