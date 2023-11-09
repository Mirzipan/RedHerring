namespace RedHerring.Alexandria;

public interface Drawable
{
    bool IsVisible { get; }
    int DrawOrder { get; }

    bool BeginDraw();
    void Draw(GameTime gameTime);
    void EndDraw();
}