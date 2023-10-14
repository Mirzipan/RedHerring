namespace RedHerring.Fingerprint.Layers;

public record struct ActionEvent(string Action, InputState State, float Value)
{
    public bool Consumed;
}