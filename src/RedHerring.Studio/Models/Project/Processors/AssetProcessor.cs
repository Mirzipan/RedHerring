namespace RedHerring.Studio.Models.Project.Processors;

public abstract class AssetProcessor<TInput, TOutput> : Processor
{
    object Processor.Process(object input) => Process((TInput)input)!;

    public abstract TOutput Process(TInput input);
}