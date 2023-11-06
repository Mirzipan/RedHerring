namespace RedHerring.Infusion.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
public sealed class InfuseAttribute : Attribute
{
}