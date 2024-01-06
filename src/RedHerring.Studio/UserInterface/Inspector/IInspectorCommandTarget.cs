using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

public interface IInspectorCommandTarget
{
	void Commit(Command command);
}