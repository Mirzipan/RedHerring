namespace RedHerring.Fingerprint.States;

public interface IKeyboardState : IInputState
{
    bool IsKeyUp(Key key);
    bool IsKeyPressed(Key key);
    bool IsKeyDown(Key key);
    bool IsKeyReleased(Key key);
    bool IsAnyKeyDown();
    void GetKeysPressed(IList<Key> keys);
    void GetKeysDown(IList<Key> keys);
    void GetKeysReleased(IList<Key> keys);
    void GetChars(IList<char> chars);
}