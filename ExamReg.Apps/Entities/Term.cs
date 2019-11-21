﻿using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class Term : DataEntity
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public Guid SemesterId { get; set; }
    }
    public class TermFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public StringFilter SubjectName { get; set; }
        public GuidFilter SemesterId { get; set; }
        public TermOrder OrderBy { get; set;  }
        public TermFilter() : base() { }
    }
    public enum TermOrder
    {
        Id,
        SubjectName,
        SemesterId
    }
}