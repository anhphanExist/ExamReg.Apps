using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamRoom
{
    public interface IExamRoomValidator : IServiceScoped
    {
        Task<bool> Create(ExamRoom examRoom);
        Task<bool> Update(ExamRoom examRoom);
        Task<bool> Delete(ExamRoom examRoom);
    }
    public class ExamRoomValidator : IExamRoomValidator
    {
        public enum ERROR
        {
            ExamRoomExisted,
            NotExisted,
            AmphitheaterNameEmpty,
            AmphitheaterNameInvalid,
            RoomNumberEmpty,
            RoomNumberInvalid,
            ComputerNumberInvalid
        }
        private IUOW UOW;

        public ExamRoomValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        private async Task<bool> ValidateNotExist(ExamRoom ExamRoom)
        {
            ExamRoomFilter filter = new ExamRoomFilter
            {
                Take = Int32.MaxValue,
                RoomNumber = new ShortFilter { Equal = ExamRoom.RoomNumber }
            };

            int count = await UOW.ExamRoomRepository.Count(filter);
            if (count > 0)
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom), ERROR.ExamRoomExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateExist(ExamRoom ExamRoom)
        {
            ExamRoomFilter filter = new ExamRoomFilter
            {
                Take = Int32.MaxValue,
                RoomNumber = new ShortFilter { Equal = ExamRoom.RoomNumber }
            };

            int count = await UOW.ExamRoomRepository.Count(filter);
            if (count == 0)
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom), ERROR.NotExisted);
                return false;
            }
            return true;
        }
        private bool ValidateStringLength(ExamRoom ExamRoom)
        {
            if (ExamRoom.RoomNumber.ToString() == null)
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom), ERROR.RoomNumberEmpty);
                return false;
            }
            else if (ExamRoom.RoomNumber < 0)
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom), ERROR.RoomNumberInvalid);
                return false;
            }
            if (ExamRoom.ComputerNumber < 0)
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom), ERROR.ComputerNumberInvalid);
                return false;
            }
            if (string.IsNullOrEmpty(ExamRoom.AmphitheaterName))
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom), ERROR.AmphitheaterNameEmpty);
                return false;
            }
            else if (ExamRoom.AmphitheaterName != null & ExamRoom.AmphitheaterName.Length > 100)
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom), ERROR.AmphitheaterNameInvalid);
                return false;
            }
            return true;
        }
        public async Task<bool> Create(ExamRoom examRoom)
        {
            bool IsValid = true;
            IsValid &= await ValidateNotExist(examRoom);
            IsValid &= ValidateStringLength(examRoom);
            return IsValid;
        }

        public async Task<bool> Delete(ExamRoom examRoom)
        {
            bool IsValid = true;
            IsValid &= await ValidateExist(examRoom);
            return IsValid;
        }

        public async Task<bool> Update(ExamRoom examRoom)
        {
            bool IsValid = true;

            IsValid &= await ValidateExist(examRoom);
            IsValid &= ValidateStringLength(examRoom);
            return IsValid;
        }
    }
}
