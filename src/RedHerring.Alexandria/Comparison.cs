namespace RedHerring.Alexandria;

public static class Comparison
{
    public static int Updatables(Updatable lhs, Updatable rhs) => lhs.UpdateOrder.CompareTo(rhs.UpdateOrder);
    
    public static int Drawables(Drawable lhs, Drawable rhs) => lhs.DrawOrder.CompareTo(rhs.DrawOrder);
}