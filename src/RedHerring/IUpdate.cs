namespace RedHerring;

public interface IUpdate
{
    bool IsEnabled { get; }
    int UpdateOrder { get; }

    void Update(GameTime gameTime);
}