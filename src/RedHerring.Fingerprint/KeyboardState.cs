using System.Collections;
using Silk.NET.Input;

namespace RedHerring.Fingerprint;

internal struct KeyboardState
{
    private const int BufferLength = 512;
    private const int BitsPerByte = 8;
    
    private readonly BitArray _buffer = new(BufferLength);
    private static int[] _tmpBuffer = new int[BufferLength / sizeof(int) / BitsPerByte];

    public KeyboardState()
    {
    }

    public bool IsKeyUp(Key key) => !_buffer.Get((int)key);
    public bool IsKeyDown(Key key) => _buffer.Get((int)key);
    public bool IsAnyKeyDown() => IsBufferEmpty();

    internal bool IsBufferEmpty()
    {
        _buffer.CopyTo(_tmpBuffer, 0);
        return _tmpBuffer.All(value => value == 0);
    }

    public void SetKey(Key key, bool isPressed)
    {
        _buffer[(int)key] = isPressed;
    }
}