namespace RedHerring.Detective;

public record struct RecordedData(int Id, ProfilingKey Key, RecordedDataKind Kind, TimeSpan Timestamp, TimeSpan ElapsedTime);