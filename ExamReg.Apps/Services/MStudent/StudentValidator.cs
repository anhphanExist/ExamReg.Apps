using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MStudent
{
    public interface IStudentValidator : IServiceScoped
    {
        Task<bool> Create(Student student);
        Task<bool> Update(Student student);
        Task<bool> Delete(Student student);
        Task<bool> Import(List<Student> students);
    }
    public class StudentValidator : IStudentValidator
    {
        public enum ERROR
        {
            IdNotFound,
            StudentNumberExisted,
            BirthdayInvalid,
            EmailInvalid,
            InvalidPassword,
            NotExisted,
            StringEmpty,
            StringLimited
        }
        private IUOW UOW;

        public StudentValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        private async Task<bool> ValidateNotExist(Student student)
        {
            StudentFilter filter = new StudentFilter
            {
                Take = Int32.MaxValue,
                StudentNumber = new IntFilter { Equal = student.StudentNumber }
            };

            int count = await UOW.StudentRepository.Count(filter);
            if (count > 0)
            {
                student.AddError(nameof(StudentValidator), nameof(student.StudentNumber), ERROR.StudentNumberExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateId(Student student)
        {
            StudentFilter filter = new StudentFilter
            {
                Id = new GuidFilter { Equal = student.Id}
            };

            int count = await UOW.StudentRepository.Count(filter);
            if (count == 0)
            {
                student.AddError(nameof(StudentValidator), nameof(student.StudentNumber), ERROR.NotExisted);
                return false;
            }
            return true;
        }

        private bool ValidateStringLength(Student student)
        {
            if (string.IsNullOrEmpty(student.GivenName))
            {
                student.AddError(nameof(StudentValidator), nameof(student.GivenName), ERROR.StringEmpty);
                return false;
            }                  
            else if (student.GivenName.Length > 100)
            {
                student.AddError(nameof(StudentValidator), nameof(student.GivenName), ERROR.StringLimited);
                return false;
            }
                    
            if (string.IsNullOrEmpty(student.LastName))
            {
                student.AddError(nameof(StudentValidator), nameof(student.LastName), ERROR.StringEmpty);
                return false;
            }                   
            else if (student.LastName.Length > 100)
            {
                student.AddError(nameof(StudentValidator), nameof(student.LastName), ERROR.StringLimited);
                return false;
            }
                    
            if (student.Email.Length > 100)
            {
                student.AddError(nameof(StudentValidator), nameof(student.Email), ERROR.EmailInvalid);
                return false;
            }                                

            return true;
        }

        public async Task<bool> Create(Student student)
        {
            bool IsValid = true;
            IsValid &= await ValidateNotExist(student);
            IsValid &= ValidateStringLength(student);
            return IsValid;
        }

        public async Task<bool> Delete(Student student)
        {
            bool IsValid = true;
            IsValid &= await ValidateId(student);
            return IsValid;
        }

        public async Task<bool> Import(List<Student> students)
        {
            bool IsValid = true;
            foreach(var student in students)
            {
                //IsValid &= await ValidateNotExist(student);
                IsValid &= ValidateStringLength(student);
            }
            return IsValid;
        }

        public async Task<bool> Update(Student student)
        {
            bool IsValid = true;

            IsValid &= await ValidateId(student);
            IsValid &= ValidateStringLength(student);
            return IsValid;
        }
    }
}
