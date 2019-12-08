using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
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
        Task<Semester> Update(Semester semester);
        Task<Semester> Delete(Semester semester);
    }
    public class SemesterService : ISemesterService
    {
        private IUOW UOW;
        private ISemesterValidator SemesterValidator;
        public SemesterService(IUOW UOW, ISemesterValidator SemesterValidator)
        {
            this.UOW = UOW;
            this.SemesterValidator = SemesterValidator;
        }
        public async Task<Semester> Get(Guid Id)
        {
            return await UOW.SemesterRepository.Get(Id);
        }
        public async Task<Semester> Create(Semester semester)
        {
            if (!await SemesterValidator.Create(semester))
                return semester;

            using (UOW.Begin())
            {
                try
                {
                    semester.Id = new Guid();

                    await UOW.SemesterRepository.Create(semester);
                    await UOW.Commit();
                    return await Get(semester.Id);
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    semester.AddError(nameof(SemesterService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                    return semester;
                }
            }
        }

        public async Task<Semester> Delete(Semester semester)
        {
            if (!await SemesterValidator.Delete(semester))
                return semester;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.SemesterRepository.Delete(semester.Id);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    semester.AddError(nameof(SemesterService), nameof(Delete), CommonEnum.ErrorCode.SystemError);
                }
            }
            return semester;
        }

        public async Task<List<Semester>> List(SemesterFilter filter)
        {
            return await UOW.SemesterRepository.List(filter);
        }

        public async Task<Semester> Update(Semester semester)
        {
            if (!await SemesterValidator.Update(semester))
                return semester;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.SemesterRepository.Update(semester);
                    await UOW.Commit();
                    return await UOW.SemesterRepository.Get(semester.Id);
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    semester.AddError(nameof(SemesterService), nameof(Update), CommonEnum.ErrorCode.SystemError);
                    return semester;
                }
            }
        }
    }
}
