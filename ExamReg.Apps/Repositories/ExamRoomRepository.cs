using ExamReg.Apps.Entities;
using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IExamRoomRepository
    {
        Task<ExamRoom> Get(Guid Id);
        Task<ExamRoom> Get(ExamRoomFilter filter);
        Task<bool> Create(ExamRoom examRoom);
        Task<bool> Update(ExamRoom examRoom);
        Task<bool> Delete(Guid Id);
        Task<int> Count(ExamRoomFilter filter);
        Task<List<ExamRoom>> List(ExamRoomFilter filter);
    }
    public class ExamRoomRepository : IExamRoomRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public ExamRoomRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(ExamRoomFilter filter)
        {
            IQueryable<ExamRoomDAO> examRoomDAOs = examRegContext.ExamRoom.AsNoTracking();
            examRoomDAOs = DynamicFilter(examRoomDAOs, filter);
            return await examRoomDAOs.CountAsync();
        }

        public async Task<bool> Create(ExamRoom examRoom)
        {
            ExamRoomDAO examRoomDAO = examRegContext.ExamRoom.Where(e => e.Id.Equals(examRoom.Id)).FirstOrDefault();
            if (examRoomDAO == null)
            {
                examRoomDAO = new ExamRoomDAO()
                {
                    Id = examRoom.Id,
                    AmphitheaterName = examRoom.AmphitheaterName,
                    ComputerNumber = examRoom.ComputerNumber,
                    RoomNumber = examRoom.RoomNumber
                };
                await examRegContext.ExamRoom.AddAsync(examRoomDAO);
            }
            else
            {
                examRoomDAO.Id = examRoom.Id;
                examRoomDAO.AmphitheaterName = examRoom.AmphitheaterName;
                examRoomDAO.ComputerNumber = examRoom.ComputerNumber;
                examRoomDAO.RoomNumber = examRoom.RoomNumber;
            };

            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid Id)
        {
            await examRegContext.ExamRoomExamPeriod
            .Where(t => t.ExamRoomId.Equals(Id))
            .DeleteFromQueryAsync();

            ExamRoomDAO examRoomDAO = examRegContext.ExamRoom
                .Where(s => s.Id.Equals(Id))
                .FirstOrDefault();

            examRegContext.ExamRoom.Remove(examRoomDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<ExamRoom> Get(Guid Id)
        {
            ExamRoomDAO examRoomDAO = examRegContext.ExamRoom
                .AsNoTracking()
                .Where(e => e.Id.Equals(Id))
                .FirstOrDefault();
            if (examRoomDAO == null)
                return null;
            return new ExamRoom()
            {
                Id = examRoomDAO.Id,
                AmphitheaterName = examRoomDAO.AmphitheaterName,
                ComputerNumber = examRoomDAO.ComputerNumber,
                RoomNumber = examRoomDAO.RoomNumber,
                Code = string.Format(examRoomDAO.AmphitheaterName + "_" + examRoomDAO.RoomNumber)
            };
        }

        public async Task<ExamRoom> Get(ExamRoomFilter filter)
        {
            if (filter == null) return null;

            IQueryable<ExamRoomDAO> query = examRegContext.ExamRoom.AsNoTracking();
            query = DynamicFilter(query, filter);

            List<ExamRoom> list = await query.Select(e => new ExamRoom()
            {
                Id = e.Id,
                AmphitheaterName = e.AmphitheaterName,
                ComputerNumber = e.ComputerNumber,
                RoomNumber = e.RoomNumber,
                Code = string.Format(e.AmphitheaterName + "_" + e.RoomNumber)
            }).ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<List<ExamRoom>> List(ExamRoomFilter filter)
        {
            if (filter == null) return new List<ExamRoom>();
            IQueryable<ExamRoomDAO> query = examRegContext.ExamRoom.AsNoTracking();
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<ExamRoom> list = await query.Select(e => new ExamRoom()
            {
                Id = e.Id,
                RoomNumber = e.RoomNumber,
                ComputerNumber = e.ComputerNumber,
                AmphitheaterName = e.AmphitheaterName,
                Code = string.Format(e.AmphitheaterName + "_" + e.RoomNumber)
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(ExamRoom examRoom)
        {
            await examRegContext.ExamRoom.Where(e => e.Id.Equals(examRoom.Id)).UpdateFromQueryAsync(e => new ExamRoomDAO()
            {
                AmphitheaterName = examRoom.AmphitheaterName,
                ComputerNumber = examRoom.ComputerNumber,
                RoomNumber = examRoom.RoomNumber
            });

            return true;
        }

        private IQueryable<ExamRoomDAO> DynamicFilter(IQueryable<ExamRoomDAO> query, ExamRoomFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.AmphitheaterName != null)
                query = query.Where(q => q.AmphitheaterName, filter.AmphitheaterName);
            if (filter.ComputerNumber != null)
                query = query.Where(q => q.ComputerNumber, filter.ComputerNumber);
            if (filter.RoomNumber != null)
                query = query.Where(q => q.RoomNumber, filter.RoomNumber);
            if (filter.ExamDate != null)
            {
                query = query.Where(q => q.ExamRoomExamPeriods
                                  .Select(e => e.ExamPeriod.ExamDate.Equals(filter.ExamDate))
                                  .Contains(true));
                if (filter.ExceptStartHour != null && filter.ExceptFinishHour != null)
                    query = query.Where(q => !q.ExamRoomExamPeriods
                                     .Select(e =>
                                        (filter.ExceptStartHour <= e.ExamPeriod.StartHour && e.ExamPeriod.StartHour <= filter.ExceptFinishHour) ||
                                        (filter.ExceptStartHour <= e.ExamPeriod.FinishHour && e.ExamPeriod.FinishHour <= filter.ExceptFinishHour) ||
                                        (e.ExamPeriod.StartHour <= filter.ExceptStartHour && filter.ExceptFinishHour <= e.ExamPeriod.FinishHour))
                                     .Contains(true));
            }
            if (filter.ExceptExamDate != null)
                query = query.Where(q => !q.ExamRoomExamPeriods
                                 .Select(e => e.ExamPeriod.ExamDate == filter.ExceptExamDate).Contains(true));
            return query;
        }
        private IQueryable<ExamRoomDAO> DynamicOrder(IQueryable<ExamRoomDAO> query, ExamRoomFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ExamRoomOrder.ComputerNumber:
                            query = query.OrderBy(q => q.ComputerNumber);
                            break;
                        case ExamRoomOrder.AmphitheaterName:
                            query = query.OrderBy(q => q.AmphitheaterName);
                            break;
                        case ExamRoomOrder.RoomNumber:
                            query = query.OrderBy(q => q.RoomNumber);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ExamRoomOrder.ComputerNumber:
                            query = query.OrderByDescending(q => q.ComputerNumber);
                            break;
                        case ExamRoomOrder.AmphitheaterName:
                            query = query.OrderByDescending(q => q.AmphitheaterName);
                            break;
                        case ExamRoomOrder.RoomNumber:
                            query = query.OrderByDescending(q => q.RoomNumber);
                            break;
                        default:
                            query = query.OrderByDescending(q => q.CX);
                            break;
                    }
                    break;
                default:
                    query = query.OrderBy(q => q.CX);
                    break;
            }
            return query.Skip(filter.Skip).Take(filter.Take);
        }
    }
}

