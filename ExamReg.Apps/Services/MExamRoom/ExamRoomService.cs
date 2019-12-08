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
        Task<List<ExamRoom>> ListAvailableExamRoom(ExamRoomFilter filter);
    }
    public class ExamRoomService : IExamRoomService
    {
        private IUOW UOW;
        public ExamRoomService(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public Task<int> Count(ExamRoomFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoom> Create(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoom> Delete(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExamRoom>> List(ExamRoomFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoom> Update(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExamRoom>> ListAvailableExamRoom(ExamRoomFilter filter)
        {
            // Lấy tất cả các phòng thi trống ứng với StartHour, FinishHour, ExamDate
            List<ExamRoom> examRooms = await UOW.ExamRoomRepository.List(filter);
            throw new NotImplementedException();
        }
    }
}
