namespace RedHerring.Studio.Models.Project.Processors;

public class CopyProcessor : AssetProcessor<object, object>
{
    public override object Process(object input) => input;
}