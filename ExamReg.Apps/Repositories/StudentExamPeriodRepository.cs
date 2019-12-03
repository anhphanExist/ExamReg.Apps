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
    public interface IStudentExamPeriodRepository
    {
        Task<StudentExamPeriod> Get(StudentExamPeriodFilter filter);
        Task<bool> Create(StudentExamPeriod StudentExamPeriod);
        Task<bool> Update(StudentExamPeriod StudentExamPeriod);
        Task<bool> Delete(StudentExamPeriod StudentExamPeriod);
        Task<List<StudentExamPeriod>> List(StudentExamPeriodFilter filter);
        Task<List<StudentExamPeriod>> List();
    }
    public class StudentExamPeriodRepository : IStudentExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public StudentExamPeriodRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<bool> Create(StudentExamPeriod studentExamPeriod)
        {
            StudentExamPeriodDAO studentExamPeriodDAO = examRegContext.StudentExamPeriod
                .Where(s => (s.StudentId.Equals(studentExamPeriod.StudentId) && s.ExamPeriodId.Equals(studentExamPeriod.ExamPeriodId)))
                .FirstOrDefault();
            if (studentExamPeriodDAO == null)
            {
                studentExamPeriodDAO = new StudentExamPeriodDAO()
                {
                    StudentId = studentExamPeriod.StudentId,
                    ExamPeriodId = studentExamPeriod.ExamPeriodId
                };

                await examRegContext.StudentExamPeriod.AddAsync(studentExamPeriodDAO);
            }
            else
            {
                studentExamPeriodDAO.ExamPeriodId = studentExamPeriod.ExamPeriodId;
                studentExamPeriodDAO.StudentId = studentExamPeriod.StudentId;
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(StudentExamPeriod studentExamPeriod)
        {
            try
            {
                StudentExamPeriodDAO studentExamPeriodDAO = examRegContext.StudentExamPeriod
                    .Where(s => (s.StudentId.Equals(studentExamPeriod.StudentId) && s.ExamPeriodId.Equals(studentExamPeriod.ExamPeriodId)))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.StudentExamPeriod.Remove(studentExamPeriodDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<StudentExamPeriod> Get(StudentExamPeriodFilter filter)
        {
            IQueryable<StudentExamPeriodDAO> studentExamPeriodDAOs = examRegContext.StudentExamPeriod;
            StudentExamPeriodDAO studentExamPeriodDAO = DynamicFilter(studentExamPeriodDAOs, filter).FirstOrDefault();

            return new StudentExamPeriod()
            {
                StudentNumber = studentExamPeriodDAO.Student.StudentNumber,
                LastName = studentExamPeriodDAO.Student.LastName,
                GivenName = studentExamPeriodDAO.Student.GivenName,
                ExamPeriodId = studentExamPeriodDAO.ExamPeriodId,
                ExamDate = studentExamPeriodDAO.ExamPeriod.ExamDate,
                StartHour = studentExamPeriodDAO.ExamPeriod.StartHour,
                FinishHour = studentExamPeriodDAO.ExamPeriod.FinishHour
            };
        }
        public async Task<List<StudentExamPeriod>> List()
        {
            List<StudentExamPeriod> list = await examRegContext.StudentExamPeriod.Select(s => new StudentExamPeriod()
            {
                StudentId = s.StudentId,
                StudentNumber = s.Student.StudentNumber,
                LastName = s.Student.LastName,
                GivenName = s.Student.GivenName,
                ExamPeriodId = s.ExamPeriodId,
                ExamDate = s.ExamPeriod.ExamDate,
                StartHour = s.ExamPeriod.StartHour,
                FinishHour = s.ExamPeriod.FinishHour
            }).ToListAsync();

            return list;
        }

        public async Task<List<StudentExamPeriod>> List(StudentExamPeriodFilter filter)
        {
            if (filter == null) return new List<StudentExamPeriod>();

            IQueryable<StudentExamPeriodDAO> query = examRegContext.StudentExamPeriod;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<StudentExamPeriod> list = await query.Select(s => new StudentExamPeriod()
            {
                StudentId = s.StudentId,
                StudentNumber = s.Student.StudentNumber,
                LastName = s.Student.LastName,
                GivenName = s.Student.GivenName,
                ExamPeriodId = s.ExamPeriodId,
                ExamDate = s.ExamPeriod.ExamDate,
                StartHour = s.ExamPeriod.StartHour,
                FinishHour = s.ExamPeriod.FinishHour

            }).ToListAsync();
            return list;

        }

        public async Task<bool> Update(StudentExamPeriod StudentExamPeriod)
        {
            await examRegContext.StudentExamPeriod
                .Where(s => (s.StudentId.Equals(StudentExamPeriod.StudentId) && s.ExamPeriodId.Equals(StudentExamPeriod.ExamPeriodId)))
                .UpdateFromQueryAsync(s => new StudentExamPeriodDAO()
                {
                    // (?)
                });
            return true;
        }
        private IQueryable<StudentExamPeriodDAO> DynamicFilter(IQueryable<StudentExamPeriodDAO> query, StudentExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.StudentNumber != null)
                query = query.Where(q => q.Student.StudentNumber, filter.StudentNumber);
            if (filter.LastName != null)
                query = query.Where(q => q.Student.LastName, filter.LastName);
            if (filter.GivenName != null)
                query = query.Where(q => q.Student.GivenName, filter.GivenName);
            if (filter.ExamDate != null)
                query = query.Where(q => q.ExamPeriod.ExamDate, filter.ExamDate);
            if (filter.StartHour != null)
                query = query.Where(q => q.ExamPeriod.StartHour, filter.StartHour);
            if (filter.FinishHour != null)
                query = query.Where(q => q.ExamPeriod.FinishHour, filter.FinishHour);

            return query;
        }
        private IQueryable<StudentExamPeriodDAO> DynamicOrder(IQueryable<StudentExamPeriodDAO> query, StudentExamPeriodFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StudentExamPeriodOrder.StudentNumber:
                            query = query.OrderBy(q => q.Student.StudentNumber);
                            break;
                        case StudentExamPeriodOrder.LastName:
                            query = query.OrderBy(q => q.Student.LastName);
                            break;
                        case StudentExamPeriodOrder.GivenName:
                            query = query.OrderBy(q => q.Student.GivenName);
                            break;
                        case StudentExamPeriodOrder.ExamDate:
                            query = query.OrderBy(q => q.ExamPeriod.ExamDate);
                            break;
                        case StudentExamPeriodOrder.StartHour:
                            query = query.OrderBy(q => q.ExamPeriod.StartHour);
                            break;
                        case StudentExamPeriodOrder.FinishHour:
                            query = query.OrderBy(q => q.ExamPeriod.FinishHour);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case StudentExamPeriodOrder.StudentNumber:
                            query = query.OrderByDescending(q => q.Student.StudentNumber);
                            break;
                        case StudentExamPeriodOrder.LastName:
                            query = query.OrderByDescending(q => q.Student.LastName);
                            break;
                        case StudentExamPeriodOrder.GivenName:
                            query = query.OrderByDescending(q => q.Student.GivenName);
                            break;
                        case StudentExamPeriodOrder.ExamDate:
                            query = query.OrderByDescending(q => q.ExamPeriod.ExamDate);
                            break;
                        case StudentExamPeriodOrder.StartHour:
                            query = query.OrderByDescending(q => q.ExamPeriod.StartHour);
                            break;
                        case StudentExamPeriodOrder.FinishHour:
                            query = query.OrderByDescending(q => q.ExamPeriod.FinishHour);
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
