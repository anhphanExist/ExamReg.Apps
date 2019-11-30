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
        IExamPeriodRepository ExamPeriodRepository { get; }
        IExamProgramRepository ExamProgramRepository { get; }
        IExamRoomExamPeriodRepository ExamRoomExamPeriodRepository { get; }
        IExamRoomRepository ExamRoomRepository { get; }
        ISemesterRepository SemesterRepository { get; }
        IStudentExamPeriodRepository StudentExamPeriodRepository { get; }
        IStudentExamRoomRepository StudentExamRoomRepository { get; }
        IStudentRepository StudentRepository { get; }
        IStudentTermRepository StudentTermRepository { get; }
        ITermRepository TermRepository { get; }
    }
    public class UOW : IUOW
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
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
        public UOW(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
            UserRepository = new UserRepository(this.examRegContext, CurrentContext);
            ExamPeriodRepository = new ExamPeriodRepository(this.examRegContext);
            ExamProgramRepository = new ExamProgramRepository(this.examRegContext);
            ExamRoomExamPeriodRepository = new ExamRoomExamPeriodRepository(this.examRegContext);
            ExamRoomRepository = new ExamRoomRepository(this.examRegContext);
            SemesterRepository = new SemesterRepository(this.examRegContext);
            StudentExamPeriodRepository = new StudentExamPeriodRepository(this.examRegContext);
            StudentExamRoomRepository = new StudentExamRoomRepository(this.examRegContext);
            StudentRepository = new StudentRepository(this.examRegContext, CurrentContext);
            StudentTermRepository = new StudentTermRepository(this.examRegContext, CurrentContext);
            TermRepository = new TermRepository(this.examRegContext, CurrentContext);
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
