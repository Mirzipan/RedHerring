namespace RedHerring.Studio.Models.Project.Processors;

public class CopyProcessor : AContentProcessor<object, object>
{
    public override object Process(object input) => input;
}