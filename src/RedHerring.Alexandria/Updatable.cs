namespace RedHerring.Alexandria;

public interface Updatable
{
    bool IsEnabled { get; }
    int UpdateOrder { get; }

    void Update(GameTime gameTime);
}