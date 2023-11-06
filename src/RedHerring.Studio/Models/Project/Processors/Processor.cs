namespace RedHerring.Studio.Models.Project.Processors;

public interface Processor
{
    object Process(object input);
}