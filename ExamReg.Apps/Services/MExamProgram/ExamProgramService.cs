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

        public async Task<ExamProgram> Create(ExamProgram examProgram)
        {
            if (!await ExamProgramValidator.Create(examProgram))
                return examProgram;

            using (UOW.Begin())
            {
                try
                {
                    examProgram.Id = new Guid();

                    await UOW.ExamProgramRepository.Create(examProgram);
                    await UOW.Commit();
                    return await Get(examProgram.Id);
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
            if (!await ExamProgramValidator.Update(examProgram))
                return examProgram;

            using (UOW.Begin())
            {
                try
                {
                    // Đặt tất cả các ExamProgram về false
                    List<ExamProgram> exams = await UOW.ExamProgramRepository.List(new ExamProgramFilter
                    {
                        Name = new StringFilter { NotEqual = examProgram.Name },
                        SemesterCode = new StringFilter { NotEqual = examProgram.SemesterCode }
                    });
                    foreach(var e in exams)
                    {
                        e.IsCurrent = false;
                        await UOW.ExamProgramRepository.Update(e);
                    }

                    //Đặt ExamProgram trong params thành true
                    ExamProgramFilter filter = new ExamProgramFilter
                    {
                        Name = new StringFilter { Equal = examProgram.Name},
                        SemesterCode = new StringFilter { Equal = examProgram.SemesterCode}
                    };
                    ExamProgram ep = await UOW.ExamProgramRepository.Get(filter);
                    ep.IsCurrent = true;
                    await UOW.ExamProgramRepository.Update(ep);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examProgram.AddError(nameof(ExamProgramService), nameof(Update), CommonEnum.ErrorCode.SystemError);
                }
            }
            return examProgram;
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
