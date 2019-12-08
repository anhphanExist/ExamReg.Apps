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
    public interface IExamRoomExamPeriodRepository
    {
        Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter);
        Task<bool> Create(ExamRoomExamPeriod ExamRoomExamPeriod);
        Task<bool> Delete(Guid ExamRoomId, Guid ExamPeriodId);
        Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);
    }
    public class ExamRoomExamPeriodRepository : IExamRoomExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public ExamRoomExamPeriodRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<bool> Create(ExamRoomExamPeriod examRoomExamPeriod)
        {
            ExamRoomExamPeriodDAO examRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(examRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(examRoomExamPeriod.ExamPeriodId)))
                .FirstOrDefault();
            if (examRoomExamPeriodDAO == null)
            {
                examRoomExamPeriodDAO = new ExamRoomExamPeriodDAO()
                {
                    ExamRoomId = examRoomExamPeriod.ExamRoomId,
                    ExamPeriodId = examRoomExamPeriod.ExamPeriodId
                };

                await examRegContext.ExamRoomExamPeriod.AddAsync(examRoomExamPeriodDAO);
            }
            else
            {
                examRoomExamPeriodDAO.ExamRoomId = examRoomExamPeriod.ExamRoomId;
                examRoomExamPeriodDAO.ExamPeriodId = examRoomExamPeriod.ExamPeriodId;
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid ExamRoomId, Guid ExamPeriodId)
        {
            ExamRoomExamPeriodDAO ExamRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(ExamRoomId) && s.ExamPeriodId.Equals(ExamPeriodId)))
                .FirstOrDefault();

            examRegContext.ExamRoomExamPeriod.Remove(ExamRoomExamPeriodDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter)
        {
            IQueryable<ExamRoomExamPeriodDAO> examRoomExamPeriodDAOs = examRegContext.ExamRoomExamPeriod.AsNoTracking();
            ExamRoomExamPeriodDAO examRoomExamPeriodDAO = DynamicFilter(examRoomExamPeriodDAOs, filter).FirstOrDefault();
            if (examRoomExamPeriodDAO == null)
                return null;
            return new ExamRoomExamPeriod()
            {
                ExamProgramId = examRoomExamPeriodDAO.ExamPeriod.ExamProgramId,
                ExamRoomId = examRoomExamPeriodDAO.ExamRoomId,
                ExamPeriodId = examRoomExamPeriodDAO.ExamPeriodId,
                TermId = examRoomExamPeriodDAO.ExamPeriod.TermId,
                ExamProgramName = examRoomExamPeriodDAO.ExamPeriod.ExamProgram.Name,
                ExamDate = examRoomExamPeriodDAO.ExamPeriod.ExamDate,
                StartHour = examRoomExamPeriodDAO.ExamPeriod.StartHour,
                FinishHour = examRoomExamPeriodDAO.ExamPeriod.FinishHour,
                SubjectName = examRoomExamPeriodDAO.ExamPeriod.Term.SubjectName,
                ExamRoomNumber = examRoomExamPeriodDAO.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = examRoomExamPeriodDAO.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = examRoomExamPeriodDAO.ExamRoom.ComputerNumber,
                Students = examRoomExamPeriodDAO.ExamRegisters.Select(r => new Student
                {
                    Id = r.StudentId,
                    Username = r.Student.Users.FirstOrDefault().Username,
                    Password = r.Student.Users.FirstOrDefault().Password,
                    StudentNumber = r.Student.StudentNumber,
                    LastName = r.Student.LastName,
                    GivenName = r.Student.GivenName,
                    Birthday = r.Student.Birthday,
                    Email = r.Student.Email
                }).OrderBy(st => st.GivenName).ToList()
            };
        }

        public async Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return new List<ExamRoomExamPeriod>();

            IQueryable<ExamRoomExamPeriodDAO> query = examRegContext.ExamRoomExamPeriod.AsNoTracking();
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<ExamRoomExamPeriod> list = await query.Select(s => new ExamRoomExamPeriod()
            {
                ExamProgramId = s.ExamPeriod.ExamProgramId,
                ExamRoomId = s.ExamRoomId,
                ExamPeriodId = s.ExamPeriodId,
                TermId = s.ExamPeriod.TermId,
                ExamProgramName = s.ExamPeriod.ExamProgram.Name,
                ExamDate = s.ExamPeriod.ExamDate,
                StartHour = s.ExamPeriod.StartHour,
                FinishHour = s.ExamPeriod.FinishHour,
                SubjectName = s.ExamPeriod.Term.SubjectName,
                ExamRoomNumber = s.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = s.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = s.ExamRoom.ComputerNumber,
                Students = s.ExamRegisters.Select(r => new Student
                {
                    Id = r.StudentId,
                    Username = r.Student.Users.FirstOrDefault().Username,
                    Password = r.Student.Users.FirstOrDefault().Password,
                    StudentNumber = r.Student.StudentNumber,
                    LastName = r.Student.LastName,
                    GivenName = r.Student.GivenName,
                    Birthday = r.Student.Birthday,
                    Email = r.Student.Email
                }).OrderBy(st => st.GivenName).ToList()
            }).ToListAsync();
            return list;
        }

        private IQueryable<ExamRoomExamPeriodDAO> DynamicFilter(IQueryable<ExamRoomExamPeriodDAO> query, ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.StudentId != null)
                query = query.Where(q => q.ExamRegisters
                                 .Select(r => r.StudentId.Equals(filter.StudentId))
                                 .Contains(true));
            if (filter.StudentNumber != null)
                query = query.Where(q => q.ExamRegisters
                                 .Select(r => r.Student.StudentNumber == filter.StudentNumber)
                                 .Contains(true));
            if (filter.ExamProgramId != null)
                query = query.Where(q => q.ExamPeriod.ExamProgramId, filter.ExamProgramId);
            if (filter.ExamProgramName != null)
                query = query.Where(q => q.ExamPeriod.ExamProgram.Name, filter.ExamProgramName);
            if (filter.ExamPeriodId != null)
                query = query.Where(q => q.ExamPeriodId, filter.ExamPeriodId);
            if (filter.ExamRoomId != null)
                query = query.Where(q => q.ExamRoomId, filter.ExamRoomId);
            if (filter.TermId != null)
                query = query.Where(q => q.ExamPeriod.TermId, filter.TermId);
            return query;
        }

        private IQueryable<ExamRoomExamPeriodDAO> DynamicOrder(IQueryable<ExamRoomExamPeriodDAO> query, ExamRoomExamPeriodFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ExamOrder.SubjectName:
                            query = query.OrderBy(q => q.ExamPeriod.Term.SubjectName);
                            break;
                        case ExamOrder.ExamProgramName:
                            query = query.OrderBy(q => q.ExamPeriod.ExamProgram.Name);
                            break;
                        case ExamOrder.ExamDate:
                            query = query.OrderBy(q => q.ExamPeriod.ExamDate);
                            break;
                        case ExamOrder.StartHour:
                            query = query.OrderBy(q => q.ExamPeriod.StartHour);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ExamOrder.SubjectName:
                            query = query.OrderByDescending(q => q.ExamPeriod.Term.SubjectName);
                            break;
                        case ExamOrder.ExamProgramName:
                            query = query.OrderByDescending(q => q.ExamPeriod.ExamProgram.Name);
                            break;
                        case ExamOrder.ExamDate:
                            query = query.OrderByDescending(q => q.ExamPeriod.ExamDate);
                            break;
                        case ExamOrder.StartHour:
                            query = query.OrderByDescending(q => q.ExamPeriod.StartHour);
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
