namespace RedHerring.Alexandria;

public interface IDrawable
{
    bool IsVisible { get; }
    int DrawOrder { get; }

    bool BeginDraw();
    void Draw(GameTime gameTime);
    void EndDraw();
}