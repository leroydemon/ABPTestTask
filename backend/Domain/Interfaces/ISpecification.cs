﻿using System.Linq.Expressions;

namespace DbLevel.Interfaces
{
    public interface ISpecification<T>
    {
        List<Expression<Func<T, bool>>> Criterias { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; set; }
    }
}