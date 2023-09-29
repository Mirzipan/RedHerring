namespace RedHerring;

public interface IDrawable
{
    bool IsVisible { get; }
    int Order { get; }

    bool BeginDraw();
    void Draw(GameTime gameTime);
    void EndDraw();
}