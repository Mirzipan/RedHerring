namespace RedHerring.Alexandria;

public static class Comparison
{
    public static int Updatables(IUpdatable lhs, IUpdatable rhs) => lhs.UpdateOrder.CompareTo(rhs.UpdateOrder);
    
    public static int Drawables(IDrawable lhs, IDrawable rhs) => lhs.DrawOrder.CompareTo(rhs.DrawOrder);
}