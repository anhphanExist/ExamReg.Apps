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
    public interface ITermRepository
    {
        Task<Term> Get(Guid Id);
        Task<Term> Get(TermFilter filter);
        Task<bool> Create(Term term);
        Task<bool> Update(Term term);
        Task<bool> Delete(Term term);
        Task<int> Count(TermFilter filter);
        Task<List<Term>> List(TermFilter filter);
        Task<bool> BulkInsert(List<Term> terms);
    }
    public class TermRepository : ITermRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public TermRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(TermFilter filter)
        {
            IQueryable<TermDAO> termDAOs = examRegContext.Term;
            termDAOs = DynamicFilter(termDAOs, filter);
            return await termDAOs.CountAsync();
        }

        public async Task<bool> Create(Term term)
        {
            TermDAO termDAO = examRegContext.Term.Where(t => t.Id.Equals(term.Id)).FirstOrDefault();
            if(termDAO == null)
            {
                termDAO = new TermDAO()
                {
                    Id = term.Id,
                    SubjectName = term.SubjectName,
                    SemesterId = term.SemesterId
                };
                await examRegContext.Term.AddAsync(termDAO);
            }
            else
            {
                termDAO.Id = term.Id;
                termDAO.SubjectName = term.SubjectName;
                termDAO.SemesterId = term.SemesterId;
            };

            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Term term)
        {
            try
            {
                await examRegContext.StudentTerm
                .Where(t => t.TermId.Equals(term.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                await examRegContext.ExamPeriod
                .Where(t => t.TermId.Equals(term.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                TermDAO termDAO = examRegContext.Term
                    .Where(s => s.Id.Equals(term.Id))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.Term.Remove(termDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Term> Get(Guid Id)
        {
            TermDAO termDAO = examRegContext.Term
                .Where(t => t.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new Term()
            {
                Id = termDAO.Id,
                SubjectName = termDAO.SubjectName,
                SemesterId = termDAO.SemesterId,
                SemesterCode = string.Format(termDAO.Semester.StartYear + "_" + termDAO.Semester.EndYear + "_" + (termDAO.Semester.IsFirstHalf ? 1 : 2)),
                ExamPeriods = termDAO.ExamPeriods.Select(e => new ExamPeriod
                {
                    Id = e.Id,
                    ExamDate = e.ExamDate,
                    ExamProgramId = e.ExamProgramId,
                    ExamProgramName = e.ExamProgram.Name,
                    FinishHour = e.FinishHour,
                    StartHour = e.StartHour,
                    TermId = e.TermId,
                    SubjectName = e.Term.SubjectName
                }).ToList(),
                QualifiedStudents = termDAO.StudentTerms.Select(s => new Student
                {
                    Id = s.StudentId,
                    StudentNumber = s.Student.StudentNumber,
                    LastName = s.Student.LastName,
                    GivenName = s.Student.GivenName,
                    Email = s.Student.Email,
                    Birthday = s.Student.Birthday
                }).ToList()
            };
        }

        public async Task<Term> Get(TermFilter filter)
        {
            IQueryable<TermDAO> query = examRegContext.Term;
            TermDAO termDAO = DynamicFilter(query, filter).FirstOrDefault();

            return new Term()
            {
                Id = termDAO.Id,
                SubjectName = termDAO.SubjectName,
                SemesterId = termDAO.SemesterId,
                SemesterCode = string.Format(termDAO.Semester.StartYear + "_" + termDAO.Semester.EndYear + "_" + (termDAO.Semester.IsFirstHalf ? 1 : 2)),
                ExamPeriods = termDAO.ExamPeriods.Select(e => new ExamPeriod
                {
                    Id = e.Id,
                    ExamDate = e.ExamDate,
                    ExamProgramId = e.ExamProgramId,
                    ExamProgramName = e.ExamProgram.Name,
                    FinishHour = e.FinishHour,
                    StartHour = e.StartHour,
                    TermId = e.TermId,
                    SubjectName = e.Term.SubjectName
                }).ToList(),
                QualifiedStudents = termDAO.StudentTerms.Select(s => new Student
                {
                    Id = s.StudentId,
                    StudentNumber = s.Student.StudentNumber,
                    LastName = s.Student.LastName,
                    GivenName = s.Student.GivenName,
                    Email = s.Student.Email,
                    Birthday = s.Student.Birthday
                }).ToList()
            };
        }

        public async Task<List<Term>> List(TermFilter filter)
        {
            if (filter == null) return new List<Term>();
            IQueryable<TermDAO> query = examRegContext.Term;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<Term> list = await query.Select(t => new Term()
            {
                Id = t.Id,
                SubjectName = t.SubjectName,
                SemesterId = t.SemesterId,
                SemesterCode = string.Format(t.Semester.StartYear + "_" + t.Semester.EndYear + "_" + (t.Semester.IsFirstHalf ? 1 : 2)),
                ExamPeriods = t.ExamPeriods.Select(e => new ExamPeriod
                {
                    Id = e.Id,
                    ExamDate = e.ExamDate,
                    ExamProgramId = e.ExamProgramId,
                    ExamProgramName = e.ExamProgram.Name,
                    FinishHour = e.FinishHour,
                    StartHour = e.StartHour,
                    TermId = e.TermId,
                    SubjectName = e.Term.SubjectName
                }).ToList(),
                QualifiedStudents = t.StudentTerms.Select(s => new Student
                {
                    Id = s.StudentId,
                    StudentNumber = s.Student.StudentNumber,
                    LastName = s.Student.LastName,
                    GivenName = s.Student.GivenName,
                    Email = s.Student.Email,
                    Birthday = s.Student.Birthday
                }).ToList()
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(Term term)
        {
            await examRegContext.Term.Where(t => t.Id.Equals(term.Id)).UpdateFromQueryAsync(t => new TermDAO
            {
                SubjectName = term.SubjectName,
                SemesterId = term.SemesterId
            });
            return true;
        }

        public async Task<bool> BulkInsert(List<Term> terms)
        {
            List<TermDAO> termDAOs = terms.Select(s => new TermDAO()
            {
                Id = s.Id,
                SubjectName = s.SubjectName,
                SemesterId = s.SemesterId
            }).ToList();

            await examRegContext.Term.BulkInsertAsync(termDAOs);
            return true;
        }

        private IQueryable<TermDAO> DynamicFilter(IQueryable<TermDAO> query, TermFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.StudentNumber != null)
                query = query.Where(q => q.StudentTerms.Select(s => s.Student.StudentNumber), filter.StudentNumber);
            if (filter.SubjectName != null)
                query = query.Where(q => q.SubjectName, filter.SubjectName);
            if (filter.SemesterCode != null)
            {
                string[] codeData = filter.SemesterCode.Equal.Split(".");
                query = query.Where(q => q.Semester.StartYear, new ShortFilter { Equal = short.Parse(codeData[0]) });
                query = query.Where(q => q.Semester.EndYear, new ShortFilter { Equal = short.Parse(codeData[1]) });
                query = query.Where(q => q.Semester.IsFirstHalf == (codeData[2] == "1" ? true : false));
            }
            return query;
        }

        private IQueryable<TermDAO> DynamicOrder(IQueryable<TermDAO> query, TermFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TermOrder.SubjectName:
                            query = query.OrderBy(q => q.SubjectName);
                            break;
                        case TermOrder.SemesterCode:
                            query = query.OrderBy(q => q.Semester.StartYear);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TermOrder.SubjectName:
                            query = query.OrderByDescending(q => q.SubjectName);
                            break;
                        case TermOrder.SemesterCode:
                            query = query.OrderByDescending(q => q.Semester.StartYear);
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
