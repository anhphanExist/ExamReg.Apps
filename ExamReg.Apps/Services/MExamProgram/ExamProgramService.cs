using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
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
        Task<ExamProgram> GetCurrentExamProgram();
    }
    public class ExamProgramService : IExamProgramService
    {
        private IUOW UOW;
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

        public async Task<ExamProgram> SetCurrentExamProgram(ExamProgram examProgram)
        {
            // Không cần validator
            // Đặt tất cả các ExamProgram có current==true về false và đặt ExamProgram trong params thành true
            using (UOW.Begin())
            {
                try
                {
                    ExamProgram currentExamProgram = await UOW.ExamProgramRepository.Get(new ExamProgramFilter
                    {
                        IsCurrent = true
                    });
                    currentExamProgram.IsCurrent = false;
                    examProgram = await UOW.ExamProgramRepository.Get(examProgram.Id);
                    examProgram.IsCurrent = true;

                    await UOW.ExamProgramRepository.Update(currentExamProgram);
                    await UOW.ExamProgramRepository.Update(examProgram);
                    await UOW.Commit();
                    return examProgram;
                }
                catch (Exception)
                {
                    await UOW.Rollback();
                    examProgram.AddError(nameof(ExamProgramService), nameof(SetCurrentExamProgram), CommonEnum.ErrorCode.SystemError);
                    return examProgram;
                }
            }
        }

        public async Task<ExamProgram> GetCurrentExamProgram()
        {
            return await UOW.ExamProgramRepository.Get(new ExamProgramFilter
            {
                IsCurrent = true
            });
        }
    }
}
