namespace RedHerring.Clues;

[Serializable]
public abstract class SerializedDefinition
{
    public string PrimaryId;
    public string SecondaryId;
    
    public bool IsDefault;
}