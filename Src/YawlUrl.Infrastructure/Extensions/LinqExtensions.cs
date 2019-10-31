using System;
using System.Linq;
using System.Linq.Expressions;

namespace YawlUrl.Infrastructure.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Conditional where extension method.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source,
                                                           bool condition,
                                                           Expression<Func<TSource, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
