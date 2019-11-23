using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IUOW : IServiceScoped
    {
        Task Begin();
        Task Commit();
        Task Rollback();
        IUserRepository UserRepository { get; }
    }
    public class UOW : IUOW
    {
        private ExamRegContext examRegContext;
        public IUserRepository UserRepository { get; }
        public IExamPeriodRepository ExamPeriodRepository { get; }
        public IExamProgramRepository ExamProgramRepository { get; }
        public IExamRoomExamPeriodRepository ExamRoomExamPeriodRepository { get; }
        public IExamRoomRepository ExamRoomRepository { get; }
        public ISemesterRepository SemesterRepository { get; }
        public IStudentExamPeriodRepository StudentExamPeriodRepository { get; }
        public IStudentExamRoomRepository StudentExamRoomRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public IStudentTermRepository StudentTermRepository { get; }
        public ITermRepository TermRepository { get; }
        public UOW(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
            UserRepository = new UserRepository(this.examRegContext);
            ExamPeriodRepository = new ExamPeriodRepository(this.examRegContext);
            ExamProgramRepository = new ExamProgramRepository(this.examRegContext);
            ExamRoomExamPeriodRepository = new ExamRoomExamPeriodRepository(this.examRegContext);
            ExamRoomRepository = new ExamRoomRepository(this.examRegContext);
            SemesterRepository = new SemesterRepository(this.examRegContext);
            StudentExamPeriodRepository = new StudentExamPeriodRepository(this.examRegContext);
            StudentExamRoomRepository = new StudentExamRoomRepository(this.examRegContext);
            StudentRepository = new StudentRepository(this.examRegContext);
            StudentTermRepository = new StudentTermRepository(this.examRegContext);
            TermRepository = new TermRepository(this.examRegContext);
        }

        public async Task Begin()
        {
            await examRegContext.Database.BeginTransactionAsync();
        }

        public Task Commit()
        {
            examRegContext.Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            examRegContext.Database.RollbackTransaction();
            return Task.CompletedTask;
        }
    }
}
