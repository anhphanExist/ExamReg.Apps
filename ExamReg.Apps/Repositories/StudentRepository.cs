using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> Get(Guid Id);
        Task<Student> Get(StudentFilter filter);
        Task<bool> Create(Student student);
        Task<bool> Update(Student student);
        Task<bool> Delete(Student student);
        Task<int> Count(StudentFilter filter);
        Task<List<Student>> List(StudentFilter filter);
        Task<bool> BulkMerge(List<Student> students);
        Task<int> GetMaxStudentNumber();
    }
    public class StudentRepository : IStudentRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public StudentRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<bool> BulkMerge(List<Student> students)
        {
            List<StudentDAO> studentDAOs = students.Select(s => new StudentDAO()
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                LastName = s.LastName,
                GivenName = s.GivenName,
                Birthday = s.Birthday,
                Email = s.Email
            }).ToList();

            await examRegContext.Student.BulkMergeAsync(studentDAOs, options => 
                {
                    options.IgnoreOnMergeUpdateExpression = column => new
                    {
                        column.Id,
                        column.StudentNumber
                    };
                });
            return true;
        }

        public async Task<int> Count(StudentFilter filter)
        {
            IQueryable<StudentDAO> studentDAOs = examRegContext.Student;
            studentDAOs = DynamicFilter(studentDAOs, filter);
            return await studentDAOs.CountAsync();
        }

        public async Task<bool> Create(Student student)
        {
            StudentDAO studentDAO = examRegContext.Student.Where(s => s.Id.Equals(student.Id)).FirstOrDefault();
            if(studentDAO == null)
            {
                studentDAO = new StudentDAO()
                {
                    Id = student.Id,
                    StudentNumber = student.StudentNumber,
                    LastName = student.LastName,
                    GivenName = student.GivenName,
                    Birthday = student.Birthday,
                    Email = student.Email
                };
                await examRegContext.Student.AddAsync(studentDAO);
            }
            else
            {
                studentDAO.StudentNumber = student.StudentNumber;
                studentDAO.LastName = student.LastName;
                studentDAO.GivenName = student.GivenName;
                studentDAO.Birthday = student.Birthday;
                studentDAO.Email = student.Email;
            };

            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Student student)
        {
            try
            {
                await examRegContext.User
                .Where(s => s.StudentId.Equals(student.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                await examRegContext.StudentExamPeriod
                .Where(s => s.StudentId.Equals(student.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                await examRegContext.StudentExamRoom
                .Where(s => s.StudentId.Equals(student.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                await examRegContext.StudentTerm
                .Where(s => s.StudentId.Equals(student.Id))
                .AsNoTracking()
                .DeleteFromQueryAsync();

                StudentDAO studentDAO = examRegContext.Student
                    .Where(s => s.Id.Equals(student.Id))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.Student.Remove(studentDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Student> Get(Guid Id)
        {
            Student student = await examRegContext.Student.Where(s => s.Id == Id).Select(s => new Student()
            {
                Id = s.Id,
                Username = s.Users.FirstOrDefault().Username,
                Password = s.Users.FirstOrDefault().Password,
                StudentNumber = s.StudentNumber,
                LastName = s.LastName,
                GivenName = s.GivenName,
                Birthday = s.Birthday,
                Email = s.Email
            }).FirstOrDefaultAsync();

            if (student == null) return null;
            return student;
        }

        public async Task<Student> Get(StudentFilter filter)
        {
            if (filter == null) return null;
            IQueryable<StudentDAO> students = examRegContext.Student.AsNoTracking();
            StudentDAO studentDAO = DynamicFilter(students, filter).FirstOrDefault();
            return new Student()
            {
                Id = studentDAO.Id,
                Username = studentDAO.Users.FirstOrDefault().Username,
                Password = studentDAO.Users.FirstOrDefault().Password,
                StudentNumber = studentDAO.StudentNumber,
                LastName = studentDAO.LastName,
                GivenName = studentDAO.GivenName,
                Birthday = studentDAO.Birthday,
                Email = studentDAO.Email
            };
        }

        public async Task<int> GetMaxStudentNumber()
        {
            int count = await examRegContext.Student.CountAsync();
            return count;
        }

        public async Task<List<Student>> List(StudentFilter filter)
        {
            if (filter == null) return new List<Student>();
            IQueryable<StudentDAO> query = examRegContext.Student;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<Student> list = await query.Select(s => new Student()
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                Username = s.Users.FirstOrDefault().Username,
                Password = s.Users.FirstOrDefault().Password,
                LastName = s.LastName,
                GivenName = s.GivenName,
                Birthday = s.Birthday,
                Email = s.Email

            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(Student student)
        {
            await examRegContext.Student.Where(s => s.Id.Equals(student.Id)).UpdateFromQueryAsync(s => new StudentDAO
            {
                StudentNumber = student.StudentNumber,
                LastName = student.LastName,
                GivenName = student.GivenName,
                Birthday = student.Birthday,
                Email = student.Email
            });
            return true;
        }
        private IQueryable<StudentDAO> DynamicFilter(IQueryable<StudentDAO> query, StudentFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.StudentNumber != null)
                query = query.Where(q => q.StudentNumber, filter.StudentNumber);
            if (filter.LastName != null)
                query = query.Where(q => q.LastName, filter.LastName);
            if (filter.GivenName != null)
                query = query.Where(q => q.GivenName, filter.GivenName);
            if (filter.Birthday != null)
                query = query.Where(q => q.Birthday, filter.Birthday);

            return query;
        }

        private IQueryable<StudentDAO> DynamicOrder(IQueryable<StudentDAO> query, StudentFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case StudentOrder.StudentNumber:
                            query = query.OrderBy(q => q.StudentNumber);
                            break;
                        case StudentOrder.LastName:
                            query = query.OrderBy(q => q.LastName);
                            break;
                        case StudentOrder.GivenName:
                            query = query.OrderBy(q => q.GivenName);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:           
                    switch (filter.OrderBy)
                    {
                        case StudentOrder.StudentNumber:
                            query = query.OrderByDescending(q => q.StudentNumber);
                            break;
                        case StudentOrder.LastName:
                            query = query.OrderByDescending(q => q.LastName);
                            break;
                        case StudentOrder.GivenName:
                            query = query.OrderByDescending(q => q.GivenName);
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
