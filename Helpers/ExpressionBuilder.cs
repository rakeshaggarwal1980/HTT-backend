﻿using HTTAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace HTTAPI.Helpers
{
    public static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        /// <summary>
        /// Create search expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetExpression<T>(IList<Filter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        /// <summary>
        /// Get Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.ExpOperation)
            {
                case ExpressionOperation.Equals:
                    return Expression.Equal(member, constant);

                case ExpressionOperation.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case ExpressionOperation.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case ExpressionOperation.LessThan:
                    return Expression.LessThan(member, constant);

                case ExpressionOperation.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case ExpressionOperation.Contains:
                    return Expression.Call(constant, containsMethod, member);

                case ExpressionOperation.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case ExpressionOperation.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        /// <summary>
        /// Get Joined expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <returns></returns>
        private static BinaryExpression GetExpression<T>(ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }

    }
}
