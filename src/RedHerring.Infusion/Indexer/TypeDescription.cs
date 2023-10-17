using System.Reflection;
using RedHerring.Infusion.Attributes;

namespace RedHerring.Infusion.Indexer;

internal sealed class TypeDescription
{
    private static readonly Type InjectAttributeType = typeof(InjectAttribute);
    private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public readonly ConstructorDescription[] Constructors;
    public readonly MemberDescription[] Members;
    public readonly MethodDescription[] Methods;

    public readonly ConstructorDescription DefaultConstructor;
        
    public TypeDescription(Type type)
    {
        Constructors = GetInjectableConstructors(type, out DefaultConstructor).ToArray();
        Members = GetInjectableMembers(type).ToArray();
        Methods = GetInjectableMethods(type).ToArray();
    }

    private static List<ConstructorDescription> GetInjectableConstructors(Type type, out ConstructorDescription @default)
    {
        @default = null;
        var constructors = type.GetConstructors(Flags);
        var result = new List<ConstructorDescription>(constructors.Length);

        foreach (var constructor in constructors)
        {
            var description = ReflectionInfoConverter.Constructor(type, constructor);
            if (@default == null || @default.Parameters.Length < description.Parameters.Length)
            {
                @default = description;
            }
                
            result.Add(description);
        }
            
        return result;
    }
        
    private static List<MemberDescription> GetInjectableMembers(Type type)
    {
        var members = type.GetMembers(Flags);
        var result = new List<MemberDescription>(members.Length);

        foreach (var member in members)
        {
            var attribute = member.GetCustomAttribute<InjectAttribute>();
            if (attribute == null)
            {
                continue;
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                result.Add(ReflectionInfoConverter.Field(type, field));
                continue;
            }

            var property = member as PropertyInfo;
            if (property != null)
            {
                result.Add(ReflectionInfoConverter.Property(type, property));
                continue;
            }
        }

        return result;
    }

    private static List<MethodDescription> GetInjectableMethods(Type type)
    {
        var methods = type.GetMethods(Flags);
        var result = new List<MethodDescription>(methods.Length);
            
        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<InjectAttribute>();
            if (attribute == null)
            {
                continue;
            }

            result.Add(ReflectionInfoConverter.Method(method));
        }

        return result;
    }
}