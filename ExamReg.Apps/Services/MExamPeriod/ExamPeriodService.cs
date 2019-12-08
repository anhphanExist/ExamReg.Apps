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
        public Task<int> Count(ExamPeriodFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamPeriod> Create(ExamPeriod examPeriod)
        {
            throw new NotImplementedException();
        }

        public Task<ExamPeriod> Delete(ExamPeriod examPeriod)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExamPeriod>> List(ExamPeriodFilter filter)
        {
            return await UOW.ExamPeriodRepository.List(filter);
        }

        public async Task<ExamPeriod> Update(ExamPeriod examPeriod)
        {
            // Thêm validator
            if (!await ExamPeriodValidator.Update(examPeriod))
                return examPeriod;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.ExamPeriodRepository.Update(examPeriod);
                    await UOW.Commit();
                }
                catch (Exception)
                {
                    await UOW.Rollback();
                    examPeriod.AddError(nameof(ExamPeriodService), nameof(Update), CommonEnum.ErrorCode.SystemError);
                }
                return examPeriod;
            }
        }
    }
}
