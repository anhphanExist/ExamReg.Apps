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
    public interface IStudentExamRoomRepository
    {

    }
    public class StudentExamRoomRepository : IStudentExamRoomRepository
    {
        private ExamRegContext examRegContext;
        public StudentExamRoomRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
    }
}
