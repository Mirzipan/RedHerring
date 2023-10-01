using System.Numerics;

namespace RedHerring.Core.Maths;

public static class Vector3Direction
{
    public static readonly Vector3 Up = new Vector3(0, 1, 0);
    public static readonly Vector3 Down = new Vector3(0, -1, 0);
    
    public static readonly Vector3 Left = new Vector3(-1, 0, 0);
    public static readonly Vector3 Right = new Vector3(1, 0, 0);
    
    public static readonly Vector3 Forward = new Vector3(0, 0, -1);
    public static readonly Vector3 Backward = new Vector3(0, 0, 1);
}