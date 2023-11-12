namespace RedHerring.Numbers;

public static class IntExtensions
{
    public static byte ClampToByte(this int @this)
    {
        return @this switch
        {
            < 0 => 0,
            > 255 => 255,
            _ => (byte)@this,
        };
    }
}