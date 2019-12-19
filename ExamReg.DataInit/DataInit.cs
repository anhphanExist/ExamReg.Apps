using ExamReg.Apps.Repositories.Models;
using ExamReg.DataInit.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamReg.DataInit
{
    public class DataInit
    {
        private ExamRegContext examRegContext;
        private UserInit userInit;
        private TermInit termInit;
        private StudentTermInit studentTermInit;
        private StudentInit studentInit;
        private SemesterInit semesterInit;
        private ExamRoomInit examRoomInit;
        private ExamRoomExamPeriodInit examRoomExamPeriodInit;
        private ExamRegisterInit examRegisterInit;
        private ExamProgramInit examProgramInit;
        private ExamPeriodInit examPeriodInit;

        public DataInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
            userInit = new UserInit(examRegContext);
            termInit = new TermInit(examRegContext);
            studentTermInit = new StudentTermInit(examRegContext);
            studentInit = new StudentInit(examRegContext);
            semesterInit = new SemesterInit(examRegContext);
            examRoomInit = new ExamRoomInit(examRegContext);
            examRoomExamPeriodInit = new ExamRoomExamPeriodInit(examRegContext);
            examRegisterInit = new ExamRegisterInit(examRegContext);
            examProgramInit = new ExamProgramInit(examRegContext);
            examPeriodInit = new ExamPeriodInit(examRegContext);
        }
        public void Clean()
        {
            string command = string.Format(
              "TRUNCATE"
                    + "\u0022" + "ExamRegister" + "\u0022" + ", "
                    + "\u0022" + "ExamRoomExamPeriod" + "\u0022" + ", "
                    + "\u0022" + "ExamRoom" + "\u0022" + ", "
                    + "\u0022" + "ExamPeriod" + "\u0022" + ", "
                    + "\u0022" + "ExamProgram" + "\u0022" + ", "
                    + "\u0022" + "StudentTerm" + "\u0022" + ", "
                    + "\u0022" + "Term" + "\u0022" + ", "
                    + "\u0022" + "Semester" + "\u0022" + ", "
                    + "\u0022" + "User" + "\u0022" + ", "
                    + "\u0022" + "Student" + "\u0022" + " "
                + "RESTART IDENTITY;");
            var result = examRegContext.Database.ExecuteSqlCommand(command);
        }

        public void Init()
        {
            Clean();
            InitStudent();
            InitUser();
            InitSemester();
            InitTerm();
            InitStudentTerm();
            InitExamProgram();
            InitExamPeriod();
            InitExamRoom();
            InitExamRoomExamPeriod();
            InitExamRegister();
            examRegContext.SaveChanges();
        }

        private void InitStudent()
        {
            studentInit.Init();
        }

        private void InitUser()
        {
            userInit.Init(studentInit.StudentCodes);
        }

        private void InitSemester()
        {
            semesterInit.Init();
        }

        private void InitTerm()
        {
            termInit.Init(semesterInit.SemesterCodes);
        }

        private void InitStudentTerm()
        {
            studentTermInit.Init(studentInit.StudentCodes, termInit.TermCodes);
        }

        private void InitExamProgram()
        {
            examProgramInit.Init(semesterInit.SemesterCodes);
        }

        private void InitExamPeriod()
        {
            examPeriodInit.Init(termInit.TermCodes, examProgramInit.ExamProgramCodes);
        }

        private void InitExamRoom()
        {
            examRoomInit.Init();
        }

        private void InitExamRoomExamPeriod()
        {
            examRoomExamPeriodInit.Init(examPeriodInit.ExamPeriodCodes, examRoomInit.ExamRoomCodes);
        }

        private void InitExamRegister()
        {
            //examRegisterInit.Init()
        }
    }
}
