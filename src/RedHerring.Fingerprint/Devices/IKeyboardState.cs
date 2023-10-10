namespace RedHerring.Fingerprint.Devices;

public interface IKeyboardState : IInputState
{
    bool IsKeyUp(Key key);
    bool IsKeyPressed(Key key);
    bool IsKeyDown(Key key);
    bool IsKeyReleased(Key key);
    bool IsAnyKeyDown();
    void GetKeysDown(IList<Key> keys);
}