using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamRoom
{
    public interface IExamRoomService : IServiceScoped
    {
        Task<int> Count(ExamRoomFilter filter);
        Task<List<ExamRoom>> List(ExamRoomFilter filter);
        Task<ExamRoom> Create(ExamRoom examRoom);
        Task<ExamRoom> Update(ExamRoom examRoom);
        Task<ExamRoom> Delete(ExamRoom examRoom);
        Task<List<ExamRoom>> ListAvailableExamRoom(ExamRoomFilter examRoomFilter);
    }
    public class ExamRoomService : IExamRoomService
    {
        private IUOW UOW;
        private IExamRoomValidator ExamRoomValidator;
        public ExamRoomService(IUOW UOW, IExamRoomValidator ExamRoomValidator)
        {
            this.UOW = UOW;
            this.ExamRoomValidator = ExamRoomValidator;
        }
        public async Task<int> Count(ExamRoomFilter filter)
        {
            return await UOW.ExamRoomRepository.Count(filter);
        }
        public async Task<ExamRoom> Get (Guid Id)
        {
            return await UOW.ExamRoomRepository.Get(Id);
        }

        public async Task<ExamRoom> Create(ExamRoom examRoom)
        {
            if (!await ExamRoomValidator.Create(examRoom))
                return examRoom;

            using (UOW.Begin())
            {
                try
                {
                    examRoom.Id = new Guid();

                    await UOW.ExamRoomRepository.Create(examRoom);
                    await UOW.Commit();
                    return await Get(examRoom.Id);
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examRoom.AddError(nameof(ExamRoomService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                    return examRoom;
                }
            }
        }

        public async Task<ExamRoom> Delete(ExamRoom examRoom)
        {
            if (!await ExamRoomValidator.Delete(examRoom))
                return examRoom;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.ExamRoomRepository.Delete(examRoom.Id);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examRoom.AddError(nameof(ExamRoomService), nameof(Delete), CommonEnum.ErrorCode.SystemError);
                }
            }
            return examRoom;
        }

        public async Task<List<ExamRoom>> List(ExamRoomFilter filter)
        {
            return await UOW.ExamRoomRepository.List(filter);
        }

        public async Task<ExamRoom> Update(ExamRoom examRoom)
        {
            if (!await ExamRoomValidator.Update(examRoom))
                return examRoom;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.ExamRoomRepository.Update(examRoom);
                    await UOW.Commit();
                    return await UOW.ExamRoomRepository.Get(examRoom.Id);
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examRoom.AddError(nameof(ExamRoomService), nameof(Update), CommonEnum.ErrorCode.SystemError);
                    return examRoom;
                }
            }
        }

        public async Task<List<ExamRoom>> ListAvailableExamRoom(ExamRoomFilter examRoomFilter)
        {
            // Lấy tất cả các phòng thi trống ứng với StartHour, FinishHour, ExamDate
            throw new NotImplementedException();
        }
    }
}
