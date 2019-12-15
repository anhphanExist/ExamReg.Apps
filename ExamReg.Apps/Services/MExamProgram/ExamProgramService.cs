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
        private IExamProgramValidator ExamProgramValidator;
        public ExamProgramService(IUOW UOW, IExamProgramValidator ExamProgramValidator)
        {
            this.UOW = UOW;
            this.ExamProgramValidator = ExamProgramValidator;
        }
        public Task<int> Count(ExamProgramFilter filter)
        {
            return UOW.ExamProgramRepository.Count(filter);
        }

        public async Task<ExamProgram> Get(Guid Id)
        {
            return await UOW.ExamProgramRepository.Get(Id);
        }
        public async Task<ExamProgram> GetSemesterId(ExamProgram examProgram)
        {
            SemesterFilter filter = new SemesterFilter
            {
                Code = new StringFilter { Equal = examProgram.SemesterCode }
            };

            Semester semester = await UOW.SemesterRepository.Get(filter);
            examProgram.SemesterId = semester.Id;

            return examProgram;
        }

        public async Task<ExamProgram> Create(ExamProgram examProgram)
        {
            if (!await ExamProgramValidator.Create(examProgram))
                return examProgram;

            using (UOW.Begin())
            {
                try
                {
                    examProgram.Id = Guid.NewGuid();

                    examProgram = await GetSemesterId(examProgram);

                    await UOW.ExamProgramRepository.Create(examProgram);
                    await UOW.Commit();
                    return examProgram;
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examProgram.AddError(nameof(ExamProgramService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                    return examProgram;
                }
            }
        }

        public async Task<ExamProgram> Delete(ExamProgram examProgram)
        {
            if (!await ExamProgramValidator.Delete(examProgram))
                return examProgram;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.ExamProgramRepository.Delete(examProgram.Id);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examProgram.AddError(nameof(ExamProgramService), nameof(Delete), CommonEnum.ErrorCode.SystemError);
                }
            }
            return examProgram;
        }

        public Task<List<ExamProgram>> List(ExamProgramFilter filter)
        {
            return UOW.ExamProgramRepository.List(filter);
        }

        public async Task<ExamProgram> Update(ExamProgram examProgram)
        {
            if (!await ExamProgramValidator.Update(examProgram))
                return examProgram;

            using (UOW.Begin())
            {
                try
                {
                    examProgram = await GetSemesterId(examProgram);

                    await UOW.ExamProgramRepository.Update(examProgram);
                    await UOW.Commit();
                    return examProgram;
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examProgram.AddError(nameof(ExamProgramService), nameof(Update), CommonEnum.ErrorCode.SystemError);
                    return examProgram;
                }
            }
        }

        public async Task<ExamProgram> SetCurrentExamProgram(ExamProgram examProgram)
        {
            // Không cần validator
            // Đặt tất cả các ExamProgram có current==true về false và đặt ExamProgram trong params thành true
            using (UOW.Begin())
            {
                try
                {
                    /*ExamProgram currentExamProgram = await UOW.ExamProgramRepository.Get(new ExamProgramFilter
                    {
                        Id = new GuidFilter { NotEqual = examProgram.Id },
                        IsCurrent = true
                    });
                    
                    await this.UOW.ExamProgramRepository.Deactive(examProgram.Id);*/

                    await UOW.ExamProgramRepository.Active(examProgram.Id);
                    await UOW.Commit();
                    return await Get(examProgram.Id);
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
