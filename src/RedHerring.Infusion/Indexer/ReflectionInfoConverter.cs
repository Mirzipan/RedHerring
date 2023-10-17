using System.Linq.Expressions;
using System.Reflection;
using RedHerring.Infusion.Delegates;

namespace RedHerring.Infusion.Indexer;

internal static class ReflectionInfoConverter
{
    public static ConstructorDescription Constructor(Type type, ConstructorInfo constructor)
    {
        var param = constructor.GetParameters().Select(e => e.ParameterType).ToArray();
        return new ConstructorDescription(TryCreateActivator(type, constructor), param);
    }

    public static MethodDescription Method(MethodInfo method)
    {
        var methodInfo = method;
        var action = TryCreateMethod(method);

        if (action is null)
        {
            action = (obj, args) => method.Invoke(obj, args);
        }

        var param = methodInfo.GetParameters().Select(e => e.ParameterType).ToArray();
        return new MethodDescription(methodInfo.Name, action, param);
    }
    
    public static MemberDescription Field(Type type, FieldInfo field)
    {
        return new MemberDescription(field.FieldType, TryCreateSetter(type, field)!);
    }

    public static MemberDescription Property(Type type, PropertyInfo property)
    {
        return new MemberDescription(property.PropertyType, TryCreateSetter(type, property)!);
    }

    #region Expressions

    private static ObjectActivator? TryCreateActivator(Type type, ConstructorInfo constructor)
    {
        if (type.ContainsGenericParameters)
        {
            return null;
        }

        ParameterExpression param = Expression.Parameter(typeof(object[]));
        ParameterInfo[] paramInfos = constructor.GetParameters();
        Expression[] args = new Expression[paramInfos.Length];

        for (int i = 0; i < paramInfos.Length; i++)
        {
            args[i] = Expression
                .Convert(Expression.ArrayIndex(param, Expression.Constant(i)), paramInfos[i].ParameterType);
        }

        return Expression
            .Lambda<ObjectActivator>(Expression.Convert(Expression.New(constructor, args), typeof(object)), param)
            .Compile();
    }

    private static InjectMethod? TryCreateMethod(MethodInfo method)
    {
        if (method.DeclaringType!.ContainsGenericParameters)
        {
            return null;
        }
        
        ParameterInfo[] paramInfos = method.GetParameters();
        if (paramInfos.Any(e => e.ParameterType.ContainsGenericParameters))
        {
            return null;
        }

        Expression[] args = new Expression[paramInfos.Length];
        ParameterExpression argsParam = Expression.Parameter(typeof(object[]));
        ParameterExpression instanceParam = Expression.Parameter(typeof(object));

        for (int i = 0; i < paramInfos.Length; i++)
        {
            args[i] = Expression
                .Convert(Expression.ArrayIndex(argsParam, Expression.Constant(i)), paramInfos[i].ParameterType);
        }
        
        return Expression
            .Lambda<InjectMethod>(Expression.Call(Expression.Convert(instanceParam, method.DeclaringType), method, args), instanceParam, argsParam)
            .Compile();
    }

    private static MemberSetter? TryCreateSetter(Type type, MemberInfo member)
    {
        if (member is FieldInfo field)
        {
            return (target, value) => field.SetValue(target, value);
        }

        if (member is PropertyInfo property && property.CanWrite)
        {
            return (target, value) => property.SetValue(target, value, null);
        }

        return null;
    }
    

    #endregion Expressions
}