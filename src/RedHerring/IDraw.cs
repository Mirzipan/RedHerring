namespace RedHerring;

public interface IDraw
{
    bool IsVisible { get; }
    int DrawOrder { get; }

    bool BeginDraw();
    void Draw(GameTime gameTime);
    void EndDraw();
}