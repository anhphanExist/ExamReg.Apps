using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamPeriod
{
    public interface IExamPeriodService : IServiceScoped
    {
        Task<int> Count(ExamPeriodFilter filter);
        Task<List<ExamPeriod>> List(ExamPeriodFilter filter);
        Task<ExamPeriod> Create(ExamPeriod examPeriod);
        Task<ExamPeriod> Update(ExamPeriod examPeriod);
        Task<ExamPeriod> Delete(ExamPeriod examPeriod);
    }

    public class ExamPeriodService : IExamPeriodService
    {
        private IUOW UOW;
        private IExamPeriodValidator ExamPeriodValidator;
        public ExamPeriodService(IUOW UOW,
            IExamPeriodValidator ExamPeriodValidator)
        {
            this.UOW = UOW;
            this.ExamPeriodValidator = ExamPeriodValidator;
        }
        public async Task<int> Count(ExamPeriodFilter filter)
        {
            return await UOW.ExamPeriodRepository.Count(filter);
        }
        public async Task<ExamPeriod> Get(Guid Id)
        {
            return await UOW.ExamPeriodRepository.Get(Id);
        }

        public async Task<ExamPeriod> Create(ExamPeriod examPeriod)
        {
            if (!await ExamPeriodValidator.Create(examPeriod))
                return examPeriod;

            using (UOW.Begin())
            {
                try
                {
                    examPeriod.Id = new Guid();
                    await UOW.ExamPeriodRepository.Create(examPeriod);
                    await UOW.Commit();
                    return await Get(examPeriod.Id);
                }
                catch(Exception e)
                {
                    await UOW.Rollback();
                    examPeriod.AddError(nameof(ExamPeriodService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                    return examPeriod;
                }
            }
        }

        public async Task<ExamPeriod> Delete(ExamPeriod examPeriod)
        {
            if (!await ExamPeriodValidator.Delete(examPeriod))
                return examPeriod;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.ExamPeriodRepository.Delete(examPeriod.Id);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examPeriod.AddError(nameof(ExamPeriodService), nameof(Delete), CommonEnum.ErrorCode.SystemError);
                }
            }
            return examPeriod;
        }

        public async Task<List<ExamPeriod>> List(ExamPeriodFilter filter)
        {
            return await UOW.ExamPeriodRepository.List(filter);
        }

        public async Task<ExamPeriod> Update(ExamPeriod examPeriod)
        {
            if (!await ExamPeriodValidator.Update(examPeriod))
                return examPeriod;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.ExamPeriodRepository.Update(examPeriod);
                    await UOW.Commit();
                    return await UOW.ExamPeriodRepository.Get(examPeriod.Id);
                }
                catch (Exception)
                {
                    await UOW.Rollback();
                    examPeriod.AddError(nameof(ExamPeriodService), nameof(Update), CommonEnum.ErrorCode.SystemError);
                    return examPeriod;
                }
            }
        }
    }
}
