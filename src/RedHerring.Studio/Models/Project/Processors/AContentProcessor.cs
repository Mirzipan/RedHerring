namespace RedHerring.Studio.Models.Project.Processors;

public abstract class AContentProcessor<TInput, TOutput> : IProcessAsset
{
    object IProcessAsset.Process(object input) => Process((TInput)input)!;

    public abstract TOutput Process(TInput input);
}