using RedHerring.Studio.Models.Project;

namespace RedHerring.Studio.Tools;

public abstract class ATool
{
	protected ProjectModel _projectModel;

	protected ATool(ProjectModel projectModel)
	{
		_projectModel = projectModel;
	}
	
	public abstract void Update(out bool finished);
}