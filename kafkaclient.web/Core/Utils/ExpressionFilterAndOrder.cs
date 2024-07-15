using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using kafkaclient.web.Core.Dto;

namespace kafkaclient.web.Core.Utils;

public static class ExpressionFilterAndOrder
{
    public static Expression<Func<T, bool>> BuildLogicFilterGeneric<T, TFilter>(
        List<FilterGroup> filterGroups,
        TFilter filter,
        Dictionary<string, string> operators)
    {
        PropertyInfo[] properties = typeof(TFilter).GetProperties();
        var parameterExp = Expression.Parameter(typeof(T), "type");

        if(filterGroups == null)
        {
            return null;
        }

        var grouped = filterGroups.GroupBy(x => x.Group);
        Expression finalExpression = null;

        foreach(var group in grouped)
        {
            Expression groupExpression = null;
            foreach(var item in group)
            {
                if(item.MultipleProperties != null && item.MultipleProperties.Length > 0)
                {
                    foreach (var nestedPropName in item.MultipleProperties)
                    {
                        PropertyInfo[] propertiesEntity = typeof(T).GetProperties();
                        groupExpression = GetExpression<T, TFilter>(
                            filter,
                            propertiesEntity,
                            nestedPropName,
                            operators,
                            groupExpression,
                            item.LogicalOperator,
                            parameterExp,
                            propertyNameParent: item.PropertyName,
                            propertiesParent: properties
                        );
                    }
                }else
                {
                    groupExpression = GetExpression<T, TFilter>(
                        filter,
                        properties,
                        item.PropertyName,
                        operators,
                        groupExpression,
                        item.LogicalOperator,
                        parameterExp
                    );
                }
            }

            if(finalExpression == null)
            {
                finalExpression = groupExpression;
            }else if(groupExpression != null)
            {
                finalExpression = Expression.AndAlso(finalExpression, groupExpression);
            }
        }

        if(finalExpression == null){
            return null;
        }

        return Expression.Lambda<Func<T, bool>>(finalExpression, parameterExp);
    }

    public static Expression GetExpression<T, TFilter>(
        TFilter filter,
        PropertyInfo[] properties,
        string propertyName,
        Dictionary<string, string> operators,
        Expression groupExpression,
        string logicalOperator,
        ParameterExpression parameterExp,
        string propertyNameParent = null,
        PropertyInfo[] propertiesParent = null)
    {
        var prop = properties.Where(_ => _.Name == propertyName).FirstOrDefault();
        if(prop != null)
        {
            object value = null;
            if(!string.IsNullOrEmpty(propertyNameParent) && propertiesParent != null)
            {
                var propParent = propertiesParent.Where(_ => _.Name == propertyNameParent).FirstOrDefault();
                if(propParent != null)
                {
                    value = propParent.GetValue(filter);
                }else
                {
                    throw new NotSupportedException($"Property '{propertyNameParent}' is not supported.");
                }
            }else
            {
                value = prop.GetValue(filter);
            }

            var op = operators.TryGetValue(prop.Name, out var opValue);
            if(op && value != null)
            {
                var currentExpression = BuildFilterGeneric<T>(prop.Name, value, opValue, parameterExp);
                if(groupExpression == null)
                {
                    groupExpression = currentExpression;
                }else
                {
                    switch (logicalOperator)
                    {
                        case "And":
                            groupExpression = Expression.AndAlso(groupExpression, currentExpression);
                            break;
                        case "Or":
                            groupExpression = Expression.OrElse(groupExpression, currentExpression);
                            break;
                        case "Not":
                            groupExpression = Expression.Not(groupExpression);
                            break;
                        case "Nand":
                            groupExpression = Expression.Not(Expression.AndAlso(groupExpression, currentExpression));
                            break;
                        case "Nor":
                            groupExpression = Expression.Not(Expression.OrElse(groupExpression, currentExpression));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return groupExpression;
    }

    public static Expression BuildFilterGeneric<T>(
        string name,
        object value,
        string opValue,
        ParameterExpression parameterExp)
    {
        Expression comparisonExp;
        var propertyExp = Expression.Property(parameterExp, name);
        var valueExpression = ConvertObjectValue(propertyExp, value);
        
        // Convertir la propiedad a string si es un entero
        Expression propertyExpAs = propertyExp.Type == typeof(int) && valueExpression.Type == typeof(string)
            ? (Expression)Expression.Call(propertyExp, "ToString", Type.EmptyTypes)
            : propertyExp;

        switch (opValue)
        {
            case "like":
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                comparisonExp = Expression.Call(propertyExpAs, containsMethod, valueExpression);
                break;
            case "=":
                comparisonExp = Expression.Equal(propertyExpAs, valueExpression);
                break;
            case "!=":
                comparisonExp = Expression.NotEqual(propertyExpAs, valueExpression);
                break;
            case ">":
                comparisonExp = Expression.GreaterThan(propertyExpAs, valueExpression);
                break;
            case "<":
                comparisonExp = Expression.LessThan(propertyExpAs, valueExpression);
                break;
            case ">=":
                comparisonExp = Expression.GreaterThanOrEqual(propertyExpAs, valueExpression);
                break;
            case "<=":
                comparisonExp = Expression.LessThanOrEqual(propertyExpAs, valueExpression);
                break;
            case "Between":
                var (startDate, endDate) = (Tuple<DateTime, DateTime>)value;
                var startConstant = Expression.Constant(startDate);
                var endConstant = Expression.Constant(endDate);

                var startComparison = Expression.GreaterThanOrEqual(propertyExpAs, startConstant);
                var endComparison = Expression.LessThanOrEqual(propertyExpAs, endConstant);

                comparisonExp = Expression.AndAlso(startComparison, endComparison);
                break;
            default:
                throw new NotSupportedException($"Operator '{opValue}' is not supported.");
        }

        return comparisonExp;
    }

    public static ConstantExpression ConvertObjectValue(
        MemberExpression propertyExp, 
        object value)
    {
        ConstantExpression convertedExpression;
        if(propertyExp.Type.IsGenericType &&
           propertyExp.Type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
           value != null)
        {
            var converter = TypeDescriptor.GetConverter(propertyExp.Type);
            if(converter != null && converter.CanConvertFrom(value.GetType()))
            {
                var convertedValue = converter.ConvertFrom(value);
                convertedExpression = Expression.Constant(convertedValue, propertyExp.Type);
            }else
            {
                throw new InvalidOperationException($"Cannot convert from {value.GetType()} to {propertyExp.Type}");
            }
        }
        else if (propertyExp.Type == typeof(int) && value is string stringValue)
        {
            if (int.TryParse(stringValue, out int intValue))
            {
                convertedExpression = Expression.Constant(intValue, propertyExp.Type);
            }
            else
            {
               convertedExpression = Expression.Constant(stringValue);
            }
        }else
        {
            convertedExpression = Expression.Constant(value, propertyExp.Type);
        }

        return convertedExpression;
    }

    public static Expression<Func<T, bool>> BuildFilterByUnique<T>(
        string name,
        object value,
        string opValue)
    {
        var parameterExp = Expression.Parameter(typeof(T), "type");
        var expression = BuildFilterGeneric<T>(name, value, opValue, parameterExp);
        return Expression.Lambda<Func<T, bool>>(expression, parameterExp);
    }

    public static IOrderedQueryable<T> ApplyOrdering<T>(
        string name,
        string value,
        IQueryable<T> query,
        IOrderedQueryable<T> orderedQuery = null)
    {
        var propertyName = name;
        var direction = value;

        var type = typeof(T);
        propertyName = propertyName.Replace("OrderBy", "");
        var property = type.GetProperty(propertyName);
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        var method = direction == "Desc" ? "ThenByDescending" : "ThenBy";

        if(orderedQuery == null)
        {
            method = direction == "Desc" ? "OrderByDescending" : "OrderBy";
            orderedQuery = (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(
                Expression.Call(
                    typeof(Queryable),
                    method,
                    new Type[] { type, property.PropertyType },
                    query.Expression,
                    Expression.Quote(orderByExp)
                )
            );
        }else
        {
            orderedQuery = (IOrderedQueryable<T>)orderedQuery.Provider.CreateQuery<T>(
                Expression.Call(
                    typeof(Queryable),
                    method,
                    new Type[] { type, property.PropertyType },
                    orderedQuery.Expression,
                    Expression.Quote(orderByExp)
                )
            );
        }

        return orderedQuery ?? (IOrderedQueryable<T>)query;
    }
}