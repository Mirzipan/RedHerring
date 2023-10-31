namespace RedHerring.Studio.Models.Project.Processors;

public interface IProcessAsset
{
    object Process(object input);
}