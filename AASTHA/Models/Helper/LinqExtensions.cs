﻿using AASTHAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AASTHAEntity
{
    public static class LinqExtensions
    {
        /// <summary>Orders the sequence by specific column and direction.</summary>
        /// <param name="query">The query.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="ascending">if set to true [ascending].</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortColumn, string direction)
        {
            string methodName = string.Format("OrderBy{0}",
                direction.ToLower() == "asc" ? "" : "descending");

            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = null;
            foreach (var property in sortColumn.Split('.'))
                memberAccess = MemberExpression.Property
                   (memberAccess ?? (parameter as Expression), property);

            LambdaExpression orderByLambda = Expression.Lambda(memberAccess, parameter);

            MethodCallExpression result = Expression.Call(
                      typeof(Queryable),
                      methodName,
                      new[] { query.ElementType, memberAccess.Type },
                      query.Expression,
                      Expression.Quote(orderByLambda));

            return query.Provider.CreateQuery<T>(result);
        }

        private static Expression HandleNullableExpression(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                return Expression.Convert(e2, e1.Type);
            return e2;
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }


        public static IQueryable<T> Where<T>(this IQueryable<T> query,
            string column, object value, WhereOperation operation)
        {
            if (string.IsNullOrEmpty(column))
                return query;

            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = null;
            foreach (var property in column.Split('.'))
                memberAccess = MemberExpression.Property
                   (memberAccess ?? (parameter as Expression), property);
            //change param value type
            //necessary to getting bool from string

            object conType;
            if (memberAccess.Type.IsGenericType && memberAccess.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                conType = Convert.ChangeType(value, Nullable.GetUnderlyingType(memberAccess.Type));
            else
                conType = Convert.ChangeType(value, memberAccess.Type);
            ConstantExpression filter = Expression.Constant
                (
                    conType
                //Convert.ChangeType(value, memberAccess.Type)
                );


            //switch operation
            Expression condition = null;
            LambdaExpression lambda = null;
            switch (operation)
            {
                //equal ==
                case WhereOperation.Equal:
                    condition = Expression.Equal(memberAccess, HandleNullableExpression(memberAccess, filter));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //not equal !=
                case WhereOperation.NotEqual:
                    condition = Expression.NotEqual(memberAccess, HandleNullableExpression(memberAccess, filter));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.LessThan:
                    condition = Expression.LessThan(memberAccess, HandleNullableExpression(memberAccess, filter));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.LessThanOrEqual:
                    condition = Expression.LessThanOrEqual(memberAccess, HandleNullableExpression(memberAccess, filter));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.GreaterThan:
                    condition = Expression.GreaterThan(memberAccess, HandleNullableExpression(memberAccess, filter));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.GreaterThanOrEqual:
                    condition = Expression.GreaterThanOrEqual(memberAccess, HandleNullableExpression(memberAccess, filter));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //string.Contains()
                case WhereOperation.Contains:
                    condition = Expression.Call(memberAccess,
                        typeof(string).GetMethod("Contains"),
                        Expression.Constant(value));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
            }


            MethodCallExpression result = Expression.Call(
                   typeof(Queryable), "Where",
                   new[] { query.ElementType },
                   query.Expression,
                   lambda);

            return query.Provider.CreateQuery<T>(result);
        }


        public static T[] GridCommonSettings<T>(this IQueryable<T> query, GridSettings grid, out int count)
        {
            if (grid.IsSearch)
            {
                //And
                if (grid.Where.groupOp == "AND")
                {
                    foreach (var rule in grid.Where.rules)
                    {
                        //Hack to resolve datetime issue for equal , add one more where condition with lessthan of next date
                        ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");
                        MemberExpression memberAccess = null;
                        foreach (var property in rule.field.Split('.'))
                            memberAccess = MemberExpression.Property
                               (memberAccess ?? (parameter as Expression), property);

                        if ((memberAccess.Type == typeof(DateTime) || memberAccess.Type == typeof(DateTime?)) && (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op) == WhereOperation.Equal)
                        {
                            query = query.Where(
                                rule.field, rule.data,
                                (WhereOperation)StringEnum.Parse(typeof(WhereOperation), "ge"));

                            query = query.Where(
                            rule.field, DateTime.Parse(rule.data).AddDays(1).Date,
                            (WhereOperation)StringEnum.Parse(typeof(WhereOperation), "lt"));
                        }
                        else
                        {
                            query = query.Where(
                                rule.field, rule.data,
                                (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                        }

                    }
                   
                }
                else
                {
                    //Or
                    var temp = query;
                    foreach (var rule in grid.Where.rules)
                    {
                        //Hack to resolve datetime issue for equal , add one more where condition with lessthan of next date
                        ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");
                        MemberExpression memberAccess = null;
                        foreach (var property in rule.field.Split('.'))
                            memberAccess = MemberExpression.Property
                               (memberAccess ?? (parameter as Expression), property);

                        if ((memberAccess.Type == typeof(DateTime) || memberAccess.Type == typeof(DateTime?)) && (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op) == WhereOperation.Equal)
                        {
                            var t = query.Where(
                            rule.field, rule.data,
                            (WhereOperation)StringEnum.Parse(typeof(WhereOperation), "ge"));
                            temp = temp.Concat(t);

                            t = query.Where(
                            rule.field, DateTime.Parse(rule.data).AddDays(1).Date,
                            (WhereOperation)StringEnum.Parse(typeof(WhereOperation), "lt"));
                            temp = temp.Concat(t);
                        }
                        else
                        {
                            var t = query.Where(
                            rule.field, rule.data,
                            (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                            temp = temp.Concat(t);
                        }

                    }

                    //remove repeating records
                    query = temp.Distinct();
                }
            }

            //sorting

            query = query.OrderBy(grid.SortColumn,
                grid.SortOrder);

            //count the total number of count
            count = query.Count();

            // return data with paging
            return query.Skip((grid.PageIndex - 1) * grid.PageSize).Take(grid.PageSize).ToArray();

        }

        public static T[] GridCommonSettingswithCustomFilter<T>(this IQueryable<T> query, GridSettings grid, out int count)
        {
            if (grid.IsSearch)
            {
                //And
                if (grid.Where.groupOp == "AND")
                {
                    foreach (var rule in grid.Where.rules)
                    {
                        if (rule.field != "Pending" && rule.field != "Confirmed")
                        {
                            query = query.Where(
                                rule.field, rule.data,
                                (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                        }
                    }
                }
                else
                {
                    //Or
                    var temp = query;
                    foreach (var rule in grid.Where.rules)
                    {
                        if (rule.field != "Pending" && rule.field != "Confirmed")
                        {
                            var t = query.Where(
                            rule.field, rule.data,
                            (WhereOperation)StringEnum.Parse(typeof(WhereOperation), rule.op));
                            temp = temp.Concat(t);
                        }
                    }
                    //remove repeating records
                    query = temp.Distinct();
                }
            }

            //sorting
            query = query.OrderBy(grid.SortColumn,
                grid.SortOrder);

            //count the total number of count
            count = query.Count();

            // return data with paging
            return query.Skip((grid.PageIndex - 1) * grid.PageSize).Take(grid.PageSize).ToArray();
        }
    }

}
