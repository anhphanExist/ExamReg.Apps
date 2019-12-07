using ExamReg.Apps.Common;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ExamReg.Apps.Common
{
    public class StringFilter
    {
        public string Equal { get; set; }
        public string NotEqual { get; set; }
        public string Contains { get; set; }
        public string NotContains { get; set; }
        public string StartsWith { get; set; }
        public string NotStartsWith { get; set; }
        public string EndsWith { get; set; }
        public string NotEndsWith { get; set; }

        public StringFilter ToLower()
        {
            if (Equal != null) Equal = Equal.ToLower();
            if (NotEqual != null) NotEqual = NotEqual.ToLower();
            if (Contains != null) Contains = Contains.ToLower();
            if (NotContains != null) NotContains = NotContains.ToLower();
            if (StartsWith != null) StartsWith = StartsWith.ToLower();
            if (NotStartsWith != null) NotStartsWith = NotStartsWith.ToLower();
            if (EndsWith != null) EndsWith = EndsWith.ToLower();
            if (NotEndsWith != null) NotEndsWith = NotEndsWith.ToLower();
            return this;
        }

        public StringFilter ToUpper()
        {
            if (Equal != null) Equal = Equal.ToUpper();
            if (NotEqual != null) NotEqual = NotEqual.ToUpper();
            if (Contains != null) Contains = Contains.ToUpper();
            if (NotContains != null) NotContains = NotContains.ToUpper();
            if (StartsWith != null) StartsWith = StartsWith.ToUpper();
            if (NotStartsWith != null) NotStartsWith = NotStartsWith.ToUpper();
            if (EndsWith != null) EndsWith = EndsWith.ToUpper();
            if (NotEndsWith != null) NotEndsWith = NotEndsWith.ToUpper();
            return this;
        }
    }

    public class IntFilter
    {
        public int? Equal { get; set; }
        public int? NotEqual { get; set; }
        public int? Less { get; set; }
        public int? LessEqual { get; set; }
        public int? Greater { get; set; }
        public int? GreaterEqual { get; set; }
    }

    public class ShortFilter
    {
        public short? Equal { get; set; }
        public short? NotEqual { get; set; }
        public short? Less { get; set; }
        public short? LessEqual { get; set; }
        public short? Greater { get; set; }
        public short? GreaterEqual { get; set; }
    }

    public class LongFilter
    {
        public long? Equal { get; set; }
        public long? NotEqual { get; set; }
        public long? Less { get; set; }
        public long? LessEqual { get; set; }
        public long? Greater { get; set; }
        public long? GreaterEqual { get; set; }
    }

    public class DoubleFilter
    {
        public double? Equal { get; set; }
        public double? NotEqual { get; set; }
        public double? Less { get; set; }
        public double? LessEqual { get; set; }
        public double? Greater { get; set; }
        public double? GreaterEqual { get; set; }
    }

    public class DecimalFilter
    {
        public decimal? Equal { get; set; }
        public decimal? NotEqual { get; set; }
        public decimal? Less { get; set; }
        public decimal? LessEqual { get; set; }
        public decimal? Greater { get; set; }
        public decimal? GreaterEqual { get; set; }
    }

    public class DateTimeFilter
    {
        public DateTime? Equal { get; set; }
        public DateTime? NotEqual { get; set; }
        public DateTime? Less { get; set; }
        public DateTime? LessEqual { get; set; }
        public DateTime? Greater { get; set; }
        public DateTime? GreaterEqual { get; set; }
        public bool HasValue
        {
            get
            {
                return Equal.HasValue || NotEqual.HasValue || Less.HasValue || LessEqual.HasValue || Greater.HasValue  || GreaterEqual.HasValue;
            }
        }
    }

    public class GuidFilter
    {
        public Guid? Equal { get; set; }
        public Guid? NotEqual { get; set; }
    }



    public static class FilterExtension
    {
        public static string ChangeToEnglishChar(this string str)
        {
            string[] VietNamChar = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };
            //Replace   
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }

            return str;
        }
        public static Guid ToGuid(this string name)
        {
            MD5 md5 = MD5.Create();
            Byte[] myStringBytes = ASCIIEncoding.Default.GetBytes(name);
            Byte[] hash = md5.ComputeHash(myStringBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return Guid.Parse(sb.ToString());
        }
        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, StringFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Equal))
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal));

            if (!string.IsNullOrEmpty(filter.NotEqual))
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual));

            if (!string.IsNullOrEmpty(filter.Contains))
                source = source.Where(BuildPredicate(propertyName, "Contains", filter.Contains));

            if (!string.IsNullOrEmpty(filter.NotContains))
                source = source.Where(BuildPredicate(propertyName, "NotContains", filter.NotContains));

            if (!string.IsNullOrEmpty(filter.StartsWith))
                source = source.Where(BuildPredicate(propertyName, "StartsWith", filter.StartsWith));

            if (!string.IsNullOrEmpty(filter.NotStartsWith))
                source = source.Where(BuildPredicate(propertyName, "NotStartsWith", filter.NotStartsWith));

            if (!string.IsNullOrEmpty(filter.EndsWith))
                source = source.Where(BuildPredicate(propertyName, "EndsWith", filter.EndsWith));

            if (!string.IsNullOrEmpty(filter.NotEndsWith))
                source = source.Where(BuildPredicate(propertyName, "NotEndsWith", filter.NotEndsWith));
            return source;
        }
        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, IntFilter filter)
        {
            if (filter.Equal.HasValue)
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal.Value.ToString()));
            if (filter.NotEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual.Value.ToString()));
            if (filter.Less.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<", filter.Less.Value.ToString()));
            if (filter.LessEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<=", filter.LessEqual.Value.ToString()));
            if (filter.Greater.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">", filter.Greater.Value.ToString()));
            if (filter.GreaterEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">=", filter.GreaterEqual.Value.ToString()));

            return source;
        }
        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, DoubleFilter filter)
        {
            if (filter.Equal.HasValue)
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal.Value.ToString()));
            if (filter.NotEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual.Value.ToString()));
            if (filter.Less.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<", filter.Less.Value.ToString()));
            if (filter.LessEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<=", filter.LessEqual.Value.ToString()));
            if (filter.Greater.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">", filter.Greater.Value.ToString()));
            if (filter.GreaterEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">=", filter.GreaterEqual.Value.ToString()));

            return source;
        }

        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, DecimalFilter filter)
        {
            if (filter.Equal.HasValue)
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal.Value.ToString()));
            if (filter.NotEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual.Value.ToString()));
            if (filter.Less.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<", filter.Less.Value.ToString()));
            if (filter.LessEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<=", filter.LessEqual.Value.ToString()));
            if (filter.Greater.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">", filter.Greater.Value.ToString()));
            if (filter.GreaterEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">=", filter.GreaterEqual.Value.ToString()));

            return source;
        }
        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, ShortFilter filter)
        {
            if (filter.Equal.HasValue)
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal.Value.ToString()));
            if (filter.NotEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual.Value.ToString()));
            if (filter.Less.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<", filter.Less.Value.ToString()));
            if (filter.LessEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<=", filter.LessEqual.Value.ToString()));
            if (filter.Greater.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">", filter.Greater.Value.ToString()));
            if (filter.GreaterEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">=", filter.GreaterEqual.Value.ToString()));

            return source;
        }
        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, LongFilter filter)
        {
            if (filter.Equal.HasValue)
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal.Value.ToString()));
            if (filter.NotEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual.Value.ToString()));
            if (filter.Less.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<", filter.Less.Value.ToString()));
            if (filter.LessEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<=", filter.LessEqual.Value.ToString()));
            if (filter.Greater.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">", filter.Greater.Value.ToString()));
            if (filter.GreaterEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">=", filter.GreaterEqual.Value.ToString()));

            return source;
        }
        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, DateTimeFilter filter)
        {
            if (filter.Equal.HasValue)
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal.Value.ToString()));
            if (filter.NotEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual.Value.ToString()));
            if (filter.Less.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<", filter.Less.Value.ToString()));
            if (filter.LessEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "<=", filter.LessEqual.Value.ToString()));
            if (filter.Greater.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">", filter.Greater.Value.ToString()));
            if (filter.GreaterEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, ">=", filter.GreaterEqual.Value.ToString()));

            return source;
        }

        public static IQueryable<TSource> Where<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> propertyName, GuidFilter filter)
        {
            if (filter.Equal.HasValue)
                source = source.Where(BuildPredicate(propertyName, "==", filter.Equal.Value.ToString()));
            if (filter.NotEqual.HasValue)
                source = source.Where(BuildPredicate(propertyName, "!=", filter.NotEqual.Value.ToString()));

            return source;
        }

        public static bool Contains(this Enum source, Enum destination)
        {
            long sourceValue = Convert.ToInt64(source);
            long destValue = Convert.ToInt64(destination);
            return (sourceValue & destValue) == destValue;
        }


        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");


        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = modelGenerator.ParseQuery(query.Expression);
            var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }

        public static Expression<Func<TSource, bool>> BuildPredicate<TSource, TKey>(Expression<Func<TSource, TKey>> propertyName, string comparison, string value)
        {
            var left = propertyName.Body;
            Expression body;
            switch (comparison)
            {
                case "NotContains":
                    body = Expression.Not(MakeComparison(left, "Contains", value));
                    break;
                case "NotStartsWith":
                    body = Expression.Not(MakeComparison(left, "StartsWith", value));
                    break;
                case "NotEndsWith":
                    body = Expression.Not(MakeComparison(left, "EndsWith", value));
                    break;
                default:
                    body = MakeComparison(left, comparison, value);
                    break;
            }

            return Expression.Lambda<Func<TSource, bool>>(body, propertyName.Parameters);
        }

        private static Expression MakeComparison(Expression left, string comparison, string value)
        {
            switch (comparison)
            {
                case "==":
                    return MakeBinary(ExpressionType.Equal, left, value);
                case "!=":
                    return MakeBinary(ExpressionType.NotEqual, left, value);
                case ">":
                    return MakeBinary(ExpressionType.GreaterThan, left, value);
                case ">=":
                    return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);
                case "<":
                    return MakeBinary(ExpressionType.LessThan, left, value);
                case "<=":
                    return MakeBinary(ExpressionType.LessThanOrEqual, left, value);
                case "Contains":
                case "StartsWith":
                case "EndsWith":
                    return Expression.Call(MakeString(left), comparison, Type.EmptyTypes, Expression.Constant(value, typeof(string)));
                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
        }

        private static Expression MakeString(Expression source)
        {
            return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
        }

        private static Expression MakeBinary(ExpressionType type, Expression left, string value)
        {
            object typedValue = value;
            if (left.Type != typeof(string))
            {
                if (string.IsNullOrEmpty(value))
                {
                    typedValue = null;
                    if (Nullable.GetUnderlyingType(left.Type) == null)
                        left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                }
                else
                {
                    var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                        valueType == typeof(Guid) ? Guid.Parse(value) :
                        Convert.ChangeType(value, valueType);
                }
            }
            var right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }
    }
}

