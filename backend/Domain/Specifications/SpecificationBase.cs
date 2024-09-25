using DbLevel.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Specifications
{
    public abstract class SpecificationBase<T> : SpecificationBase, ISpecification<T>
    {
        // List of criteria for filtering entities
        public virtual List<Expression<Func<T, bool>>> Criterias { get; } = new();

        // List of navigation properties to include in the query
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();

        // Expressions for sorting the results
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }

        // Pagination properties
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; set; }

        // Adds a navigation property to include in the query
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // Adds a navigation property to include using a string name
        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        // Applies a list of string-based includes
        protected void ApplyIncludeList(IEnumerable<string> includes)
        {
            foreach (var include in includes)
            {
                AddInclude(include);
            }
        }

        // Applies a list of expression-based includes
        protected void ApplyIncludeList(IEnumerable<Expression<Func<T, object>>> includes)
        {
            foreach (var include in includes)
            {
                AddInclude(include);
            }
        }

        // Adds a filter expression to the specification
        protected ISpecification<T> ApplyFilter(Expression<Func<T, bool>> expr)
        {
            Criterias.Add(expr);
            return this;
        }

        // Applies pagination to the query
        protected void ApplyPaging(int skip, int take)
        {
            // Ensure skip is at least 1
            if (skip < 1)
            {
                skip = 1;
            }
            Skip = (skip - 1) * take; // Calculate the number of records to skip
            Take = take;               // Set the number of records to take
            IsPagingEnabled = true;    // Enable paging
        }

        // Sets the order by expression for sorting
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression) =>
            OrderBy = orderByExpression;

        // Sets the order by descending expression for sorting
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) =>
            OrderByDescending = orderByDescendingExpression;

        // Sets the group by expression for grouping results
        protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression) =>
            GroupBy = groupByExpression;
    }

    public abstract class SpecificationBase
    {
        // Method info for string.ToLower()
        protected static readonly MethodInfo ToLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), new Type[] { });

        // Method info for string.Contains(string)
        protected static readonly MethodInfo ContainsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
    }
}

